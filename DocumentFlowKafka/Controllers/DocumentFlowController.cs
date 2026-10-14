using DocumentFlowKafka.Model;
using DocumentFlowKafka.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DocumentFlowKafka.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentFlowController : ControllerBase
    {
        private DocumentFlowContext _context;
        private KafkaProducerService _kafka;

        public DocumentFlowController(DocumentFlowContext _context, KafkaProducerService _kafka)
        {
            this._context = _context;
            this._kafka = _kafka;
        }
        /// <summary>
        /// Получить список документооборотов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var user = _context.Users.Single(s => s.Id == Convert.ToInt32(User.FindFirst(JwtRegisteredClaimNames.Sub).Value));

            return Ok(new
            {
                flows = _context.Flows.Where(s => s.SenderId.Uid == user.Uid || s.ReceiverId.Uid == user.Uid).ToList()
            });
        }

        /// <summary>
        /// Получить документооборот по идентификатору.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, CancellationToken cancellationToken)
        {
            var user = await GetCurrentUserAsync(cancellationToken);
            if (user is null)
                return Unauthorized();

            var flow = await _context.Flows
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

            if (flow is null)
                return NotFound($"Документооборот '{id}' не найден.");

            if (flow.SenderId.Uid != user.Uid && flow.ReceiverId.Uid != user.Uid)
                return Forbid();

            return Ok(new { flow });
        }

        /// <summary>
        /// Записать новый документооборот
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] Flows flow, CancellationToken cancellationToken)
        {
            var user = await GetCurrentUserAsync(cancellationToken);
            if (user is null)
                return Unauthorized();

            if (flow.SenderId?.Uid != user.Uid)
                return Forbid("Создавать документооборот может только его отправитель.");

            flow.Id = Guid.NewGuid().ToString();
            flow.DateCreate = DateTime.UtcNow;

            _context.Flows.Add(flow);
            await _context.SaveChangesAsync(cancellationToken);

            await _kafka.SendAsync("flow-created", new
            {
                Id = flow.Id,
                Name = flow.Name,
                FlowId = flow.Id,
                CreatedAt = flow.DateCreate,
                UserId = user.Id
            });

            return CreatedAtAction(nameof(Get), new { id = flow.Id }, new { flow });
        }

        /// <summary>
        /// Записать новый документ в документооборот
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<IActionResult> Post([FromBody] Documents doc, [FromQuery] string id)
        {
            try
            {
                doc.FlowId = _context.Flows.Single(s => s.Id == id);
                _context.Documents.Add(doc);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest("Ошибка записи документа");
            }

            return Ok();
        }
        /// <summary>
        /// Удалить документооборот, если в нём нет документов.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var user = await GetCurrentUserAsync(cancellationToken);
            if (user is null)
                return Unauthorized();

            var flow = (await _context.Documents
                .Include(f => f.FlowId).FirstAsync(s => s.FlowId.Id == id, cancellationToken)).FlowId;

            if (flow is null)
                return NotFound($"Документооборот '{id}' не найден.");

            if (flow.SenderId.Uid != user.Uid)
                return Forbid("Удалить документооборот может только его отправитель.");

            var flowCount = (await _context.Documents
                .Include(f => f.FlowId).CountAsync(s => s.FlowId.Id == id, cancellationToken));

            if (flowCount != 0)
                return Conflict("Нельзя удалить документооборот, в котором есть документы.");

            _context.Flows.Remove(flow);
            await _context.SaveChangesAsync(cancellationToken);

            await _kafka.SendAsync("flow-deleted", new
            {
                Id = flow.Id,
                FlowId = flow.Id,
                DeletedAt = DateTime.UtcNow,
                UserId = user.Id
            });

            return NoContent();
        }

        private async Task<Users?> GetCurrentUserAsync(CancellationToken cancellationToken)
        {
            var userIdValue = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!int.TryParse(userIdValue, out var userId))
                return null;

            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }
    }
}
