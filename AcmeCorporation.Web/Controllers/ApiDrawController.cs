namespace AcmeCorporation.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AcmeCorporation.Core.Interfaces;


[ApiController]
[Route("api/draw")]
public class ApiDrawController : ControllerBase
{
    private readonly IDrawService _drawService;
    
    public ApiDrawController(IDrawService drawService)
    {
        _drawService = drawService;
    }

    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] ApiDrawRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var entry = new DrawEntry(
            request.FirstName,
            request.LastName,
            request.Email,
            request.DateOfBirth,
            request.SerialNumber,
            userId);

        var result = await _drawService.SubmitEntryAsync(entry);
        
        if (!result.Success)
            return BadRequest(new { result.Success,result.ErrorField, result.ErrorMessage });
        
        return Ok(new { Success = true, Message = "Entry submitted successfully."});
    }
    
}
public record ApiDrawRequest(string FirstName, string LastName, string Email, DateTime DateOfBirth, string SerialNumber);