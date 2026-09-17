using DocumentFlowKafka.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DocumentFlowKafka.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentFlowController : ControllerBase
    {
        private DocumentFlowContext _context;
        public DocumentFlowController(DocumentFlowContext _context)
        {
            this._context = _context;
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
