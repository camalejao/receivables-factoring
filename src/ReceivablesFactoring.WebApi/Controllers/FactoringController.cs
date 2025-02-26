using ReceivablesFactoring.Application.Abstractions;
using ReceivablesFactoring.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReceivablesFactoring.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class FactoringController : ControllerBase
{
    private readonly IFactoringService _factoringService;
    private readonly ITokenProvider _tokenProvider;
    
    public FactoringController(IFactoringService factoringService, ITokenProvider tokenProvider)
    {
        _factoringService = factoringService;
        _tokenProvider = tokenProvider;
    }
    
    [HttpGet("checkout")]
    public async Task<FactoringDto> GetFactoring()
    {
        return await _factoringService.GetFactoringAsync(_tokenProvider.GetCompanyIdFromToken());
    }

    [HttpPost("invoice")]
    public async Task<ActionResult<InvoiceDto>> AddInvoice([FromBody] InvoiceNumberDto invoiceDto)
    {
        return StatusCode(StatusCodes.Status201Created, await _factoringService.AddInvoiceAsync(_tokenProvider.GetCompanyIdFromToken(), invoiceDto));
    }

    [HttpDelete("invoice/{number}")]
    public async Task<IActionResult> RemoveInvoice([FromRoute] int number)
    {
        await _factoringService.RemoveInvoiceAsync(_tokenProvider.GetCompanyIdFromToken(), new InvoiceNumberDto(number));
        return NoContent();
    }
}
