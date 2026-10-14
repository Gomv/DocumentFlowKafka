using DocumentFlowKafka.Classes;
using DocumentFlowKafka.Model;
using DocumentFlowKafka.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace DocumentFlowKafka.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DocumentController : ControllerBase
{
    private readonly DocumentFlowContext _context;
    private readonly KafkaProducerService _kafka;

    public DocumentController(DocumentFlowContext context, KafkaProducerService kafka)
    {
        _context = context;
        _kafka = kafka;
    }

    /// <summary>
    /// Получить список документов текущего пользователя.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        if (user is null)
            return Unauthorized();

        var documents = await _context.Documents
            .AsNoTracking()
            .Where(d => d.FlowId.SenderId.Uid == user.Uid || d.FlowId.ReceiverId.Uid == user.Uid)
            .OrderByDescending(d => d.DateCreate)
            .ToListAsync(cancellationToken);

        return Ok(new { documents });
    }

    /// <summary>
    /// Получить документ по идентификатору.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        if (user is null)
            return Unauthorized();

        var document = await _context.Documents
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        if (document is null)
            return NotFound($"Документ '{id}' не найден.");

        if (document.FlowId.SenderId.Uid != user.Uid && document.FlowId.ReceiverId.Uid != user.Uid)
            return Forbid();

        return Ok(new { document });
    }

    /// <summary>
    /// Добавить документ в документооборот через DocumnetFlowConnector.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] string body, CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        if (user is null)
            return Unauthorized();

        try
        {
            var connector = await DocumnetFlowConnector.CreateAsync(_context, body, cancellationToken);

            if (connector.Flow.SenderId.Uid != user.Uid && connector.Flow.ReceiverId.Uid != user.Uid)
                return Forbid();

            await _kafka.SendAsync("document-created", new
            {
                Id = connector.Document.Id,
                FlowId = connector.Flow.Id,
                DocumentType = connector.Document.Type.Name,
                StepType = connector.Current.Type.Name,
                CreatedAt = connector.Document.DateCreate,
                UserId = user.Id
            });

            return CreatedAtAction(nameof(Get), new { id = connector.Document.Id }, new
            {
                document = connector.Document,
                flow = connector.Flow,
                currentStep = connector.Current
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    /// <summary>
    /// Аннулировать документ.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        if (user is null)
            return Unauthorized();

        var document = await _context.Documents
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        if (document is null)
            return NotFound($"Документ '{id}' не найден.");

        if (document.FlowId.SenderId.Uid != user.Uid)
            return Forbid("Аннулировать документ может только отправитель.");

        //Анулирование пока не реализовал
        //if (document.IsCanceled)
        //    return Conflict("Документ уже аннулирован.");

        //document.IsCanceled = true;
        //document.DateCancel = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        await _kafka.SendAsync("document-canceled", new
        {
            Id = document.Id,
            FlowId = document.FlowId.Id,
            //CanceledAt = document.DateCancel,
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