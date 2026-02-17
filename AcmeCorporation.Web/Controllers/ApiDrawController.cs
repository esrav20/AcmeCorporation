using AcmeCorporation.Core.Validators;
using Microsoft.AspNetCore.Mvc;
using AcmeCorporation.Core.Interfaces;

namespace AcmeCorporation.Web.Controllers;

// API for submitting draw entries
[ApiController]
[Route("/api/[controller]")]
public class ApiDrawController : ControllerBase
{
    // Dependency Injection of IDrawService
    private readonly IDrawService _drawService;
    
    // Constructor
    public ApiDrawController(IDrawService drawService)
    {
        _drawService = drawService;
    }

    // Submits a new entry to the draw asynchronously
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit([FromBody] ApiDrawRequest request)
    {
        // Validate input
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                success = false,
                errorField = "",
                errorMessage = "Invalid input"
            });
        }
        // Validate age
        if (!AgeValidator.IsAdult(request.DateOfBirth))
            return BadRequest(new
            {
                success = false,
                errorField = "DateOfBirth ",
                errorMessage = "You must be at least 18 years old"
            });
        
        // Create DrawEntry
        var entry = new DrawEntry(
            request.FirstName,
            request.LastName,
            request.Email,
            request.DateOfBirth,
            request.SerialNumber);
        
        // Submit entry
        var result = await _drawService.SubmitEntryAsync(entry);
        
        // Return result
        if (!result.Success)
            return BadRequest(new { result.Success,result.ErrorField, result.ErrorMessage });
        
        return Ok(new { Success = true, Message = "Entry submitted successfully."});
    }
    
}
// 
public record ApiDrawRequest(string FirstName, string LastName, string Email, DateTime DateOfBirth, string SerialNumber);