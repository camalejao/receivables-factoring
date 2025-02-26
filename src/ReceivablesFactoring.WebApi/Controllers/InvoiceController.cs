using ReceivablesFactoring.Application.Abstractions;
using ReceivablesFactoring.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReceivablesFactoring.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;
    private readonly ITokenProvider _tokenProvider;

    public InvoiceController(IInvoiceService invoiceService, ITokenProvider tokenProvider)
    {
        _invoiceService = invoiceService;
        _tokenProvider = tokenProvider;
    }

    [HttpPost]
    public async Task<ActionResult<InvoiceDto>> CreateInvoice([FromBody] InvoiceDto invoiceDto)
    {
        return StatusCode(StatusCodes.Status201Created, await _invoiceService.CreateInvoiceAsync(_tokenProvider.GetCompanyIdFromToken(), invoiceDto));
    }
}
