using AcmeCorporation.Core.Data;
using AcmeCorporation.Core.Interfaces;
using AcmeCorporation.Core.Validators;
using AcmeCorporation.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace AcmeCorporation.Web.Services;

public class DrawService : IDrawService
{
    private readonly AppDbContext _dbContext;
    private readonly ISerialNumberService _serialNumberService;

    public DrawService(AppDbContext dbContext, ISerialNumberService serialNumberService)
    {
        _dbContext = dbContext;
        _serialNumberService = serialNumberService;
    }

    public async Task<DrawResult> SubmitEntryAsync(DrawEntry entry)
    {
        // Check Age
        if (!AgeValidator.IsAdult(entry.DateOfBirth))
            return new DrawResult(false, "DateOfBirth", "You must be at least 18 years old to enter");

        // Check if Serial Number is valid
        if (!await _serialNumberService.IsValidAsync(entry.SerialNumber))
            return new DrawResult(false, "SerialNumber", "Invalid Serial number");
        
        // Check if Serial Number is usable
        if (!await _serialNumberService.IsUsableAsync(entry.SerialNumber))
            return new DrawResult(false, "SerialNumber", "Max entries for this serial number has been reached");
        
        // If everything is alright, put serial number into submission
        var serialNumber = await _dbContext.SerialNumbers
            .FirstAsync(s => s.Number == entry.SerialNumber);

        var submission = new DrawSubmission
        {
            FirstName = entry.FirstName,
            LastName = entry.LastName,
            DateOfBirth = entry.DateOfBirth,
            Email = entry.Email,
            SerialNumberId = serialNumber.Id,
            UserId = entry.UserId,

        };
        
        _dbContext.Submissions.Add(submission);
        
        // Increment the use count for serial number
        await _serialNumberService.IncrementUseCountAsync(entry.SerialNumber);

        return new DrawResult(true, null, null);
    }

}