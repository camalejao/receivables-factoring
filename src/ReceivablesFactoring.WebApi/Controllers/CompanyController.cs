using ReceivablesFactoring.Application.Abstractions;
using ReceivablesFactoring.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReceivablesFactoring.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;
    
    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<CompanyDto>> CreateCompany([FromBody] CompanyDto companyDto)
    {
        return StatusCode(StatusCodes.Status201Created, await _companyService.CreateCompanyAsync(companyDto));
    }

    [HttpPost("auth/{cnpj}")]
    [AllowAnonymous]
    public async Task<string> Auth([FromRoute] string cnpj)
    {
        return await _companyService.Auth(cnpj);
    }
}
