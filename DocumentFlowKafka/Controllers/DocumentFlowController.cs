using DocumentFlowKafka.Model;
using DocumentFlowKafka.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Get()
        {
            var user = _context.Users.Single(s => s.Id == Convert.ToInt32(User.FindFirst(JwtRegisteredClaimNames.Sub).Value));

            return Ok(new
            {
                flows = _context.Flows.Where(s => s.SenderId.Uid == user.Uid || s.ReceiverId.Uid == user.Uid).ToList()
            });
        }

        /// <summary>
        /// Получить документооборот
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get([FromQuery] string id)
        {
            return Ok(new
            {
                flow = _context.Flows.Single(s => s.Id == id)
            });
        }

        /// <summary>
        /// Записать новый документооборот
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] Flows flow)
        {
            _context.Flows.Add(flow);
            await _context.SaveChangesAsync();

            await _kafka.SendAsync("flow-created", new
            {
                Id = flow.Id,
                Name = flow.Name,
                FlowId = flow.Id,
                CreatedAt = flow.DateCreate,
                UserId = User.FindFirst(JwtRegisteredClaimNames.Sub).Value
            });

            return Ok();
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
    }
}
