using Microsoft.AspNetCore.Mvc;
using AcmeCorporation.Core.Interfaces;
using AcmeCorporation.Core.Validators;
using AcmeCorporation.Web.Models;
using System.Security.Claims;

namespace AcmeCorporation.Web.Controllers;

public class DrawController : Controller
{
    private readonly IDrawService _drawService;

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

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var entry = new DrawEntry(
            model.FirstName, model.LastName, model.Email,
            model.DateOfBirth, model.SerialNumber, userId);

        var result = await _drawService.SubmitEntryAsync(entry);

        if (!result.Success)
        {
            ModelState.AddModelError(result.ErrorField ?? "", result.ErrorMessage ?? "An error occurred.");
            return View(model);
        }

        return RedirectToAction(nameof(Success));
    }

    public IActionResult Success() => View();
}