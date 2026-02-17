using AcmeCorporation.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcmeCorporation.Web.Data;

namespace AcmeCorporation.Web.Controllers;

[Authorize]
public class SubmissionsController : Controller
{
    // Dependency Injection of AppDbContext
    private readonly AppDbContext _context;
    
    // Constructor
    public SubmissionsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: /Submissions?page=1
    public async Task<IActionResult> Index(int page = 1)
    {
        // Only have 10 submissions per page
        const int pageSize = 10;
        var totalCount = await _context.Submissions.CountAsync();
        
        // Get submissions ordered by date descending
        var submissions = await _context.Submissions
            .Include(s => s.SerialNumber)
            .OrderByDescending(s => s.SubmittedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        // Set ViewBag values
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return View(submissions);
    }
}