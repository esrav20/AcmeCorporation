using Microsoft.AspNetCore.Mvc;
using AcmeCorporation.Core.Interfaces;
using AcmeCorporation.Core.Validators;
using AcmeCorporation.Web.Models;
using System.Security.Claims;

namespace AcmeCorporation.Web.Controllers;

public class DrawController : Controller
{
    // Dependency Injection of IDrawService
    private readonly IDrawService _drawService;

    // Constructor
    public DrawController(IDrawService drawService)
    {
        _drawService = drawService;
    }

    // GET: /Draw
    public IActionResult Index() => View();

    // POST: /Draw
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(SubmissionViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (!AgeValidator.IsAdult(model.DateOfBirth))
        {
            ModelState.AddModelError("DateOfBirth", "You must be at least 18 years old.");
            return View(model);
        }
        
        // Create DrawEntry
        var entry = new DrawEntry(
            model.FirstName, model.LastName, model.Email,
            model.DateOfBirth, model.SerialNumber);

        // Submit entry
        var result = await _drawService.SubmitEntryAsync(entry);

        // Return result
        if (!result.Success)
        {
            ModelState.AddModelError(result.ErrorField ?? "", result.ErrorMessage ?? "An error occurred.");
            return View(model);
        }

        return RedirectToAction(nameof(Success));
    }

    // GET: /Draw/Success
    public IActionResult Success() => View();
}