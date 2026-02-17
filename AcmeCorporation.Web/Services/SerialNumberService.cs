using AcmeCorporation.Core.Data;
using AcmeCorporation.Core.Interfaces;
using AcmeCorporation.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace AcmeCorporation.Web.Services;

public class SerialNumberService : ISerialNumberService
{
    // Dependency Injection of AppDbContext
    private readonly AppDbContext _dbContext;

    // Constructor
    public SerialNumberService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Checks if a serial number is valid
    public async Task<bool> IsValidAsync(string serialNumber)
    {
        return await _dbContext.SerialNumbers
            .AnyAsync(s => s.Number == serialNumber);
    }
    
    // Checks if a serial number is usable (has not been used yet)
    public async Task<bool> IsUsableAsync(string serialNumber)
    {
        var sn = await _dbContext.SerialNumbers
            .FirstOrDefaultAsync(s => s.Number == serialNumber);
        return sn is not null && sn.IsValid;
    }

    // Increments the use count of a serial number, when used
    public async Task IncrementUseCountAsync(string serialNumber)
    {
        var sn = await _dbContext.SerialNumbers
            .FirstOrDefaultAsync(s => s.Number == serialNumber);
        if (sn != null) sn.UseCount++;
        await _dbContext.SaveChangesAsync();
    }
}