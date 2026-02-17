using AcmeCorporation.Core.Data;
using AcmeCorporation.Core.Interfaces;
using AcmeCorporation.Core.Validators;
using AcmeCorporation.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace AcmeCorporation.Web.Services;

public class DrawService : IDrawService
{
    // Dependency Injection of AppDbContext and SerialNumberService
    private readonly AppDbContext _dbContext;
    private readonly SerialNumberService _serialNumberService;

    // Constructor
    public DrawService(AppDbContext dbContext, SerialNumberService serialNumberService)
    {
        _dbContext = dbContext;
        _serialNumberService = serialNumberService;
    }

    // Submits a new entry to the draw
    public async Task<DrawResult> SubmitEntryAsync(DrawEntry entry)
    {
        // Find serial number in Database
        var serial = await _dbContext.SerialNumbers.FirstOrDefaultAsync(s => s.Number == entry.SerialNumber);
        
        // Find amount of submissions per email
        var emailCount = await _dbContext.Submissions
            .CountAsync(s => s.Email.ToLower() == entry.Email.ToLower());
        
        // is serial number valid?
        if (serial == null)
            return new DrawResult(false, "SerialNumber", "Serial number not found");
        
        // has serial number already been used? (MaxUseCount = 1)
        if (!serial.IsValid)
            return new DrawResult(false, "SerialNumber", "Serial number has already been used");
        
        // has the user already entered more than once?
        if (emailCount >= 2)
            return new DrawResult(false, "Email", "You have already entered the draw twice.");
        
        // Check Age
        if (!AgeValidator.IsAdult(entry.DateOfBirth))
            return new DrawResult(false, "DateOfBirth", "You must be at least 18 years old to enter");
        
        
        // If everything is alright, put serial number into submission
        var serialNumber = await _dbContext.SerialNumbers
            .FirstAsync(s => s.Number == entry.SerialNumber);

        // Create submission with data
        var submission = new DrawSubmission
        {
            FirstName = entry.FirstName,
            LastName = entry.LastName,
            DateOfBirth = entry.DateOfBirth,
            Email = entry.Email,
            SerialNumberId = serialNumber.Id


        };
        
        // Add submission to database
        _dbContext.Submissions.Add(submission);
        
        // Increment the use count for serial number in database
        await _serialNumberService.IncrementUseCountAsync(entry.SerialNumber);
        
        // Save changes asynchronously
        await _dbContext.SaveChangesAsync();
    
        // Return success
        return new DrawResult(true, null, null);
    }

}