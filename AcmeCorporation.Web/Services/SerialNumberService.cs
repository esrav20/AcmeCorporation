using AcmeCorporation.Core.Data;
using AcmeCorporation.Core.Interfaces;
using AcmeCorporation.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace AcmeCorporation.Web.Services;

public class SerialNumberService : ISerialNumberService
{
    private readonly AppDbContext _dbContext;

    public SerialNumberService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsValidAsync(string serialNumber)
    {
        return await _dbContext.SerialNumbers
            .AnyAsync(s => s.Number == serialNumber);
    }

    public async Task<bool> IsUsableAsync(string serialNumber)
    {
        var sn = await _dbContext.SerialNumbers
            .FirstOrDefaultAsync(s => s.Number == serialNumber);
        return sn is not null && sn.IsValid;
    }

    public async Task IncrementUseCountAsync(string serialNumber)
    {
        var sn = await _dbContext.SerialNumbers
            .FirstOrDefaultAsync(s => s.Number == serialNumber);
        sn.UseCount++;
        await _dbContext.SaveChangesAsync();
    }
}