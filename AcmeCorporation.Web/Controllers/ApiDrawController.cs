using AcmeCorporation.Core.Validators;

namespace AcmeCorporation.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AcmeCorporation.Core.Interfaces;


[ApiController]
[Route("/api/[controller]")]
public class ApiDrawController : ControllerBase
{
    private readonly IDrawService _drawService;
    
    public ApiDrawController(IDrawService drawService)
    {
        _drawService = drawService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit([FromBody] ApiDrawRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                success = false,
                errorField = "",
                errorMessage = "Invalid input"
            });
        }
        if (!AgeValidator.IsAdult(request.DateOfBirth))
            return BadRequest(new
            {
                success = false,
                errorField = "DateOfBirth ",
                errorMessage = "You must be at least 18 years old"
            });

        var entry = new DrawEntry(
            request.FirstName,
            request.LastName,
            request.Email,
            request.DateOfBirth,
            request.SerialNumber);

        var result = await _drawService.SubmitEntryAsync(entry);
        
        if (!result.Success)
            return BadRequest(new { result.Success,result.ErrorField, result.ErrorMessage });
        
        return Ok(new { Success = true, Message = "Entry submitted successfully."});
    }
    
}
public record ApiDrawRequest(string FirstName, string LastName, string Email, DateTime DateOfBirth, string SerialNumber);