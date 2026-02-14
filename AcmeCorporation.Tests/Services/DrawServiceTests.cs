using System.ComponentModel.DataAnnotations;
using AcmeCorporation.Core.Data;
using AcmeCorporation.Core.Interfaces;
using AcmeCorporation.Web.Data;
using AcmeCorporation.Web.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AcmeCorporation.Tests.Services;

public class DrawServiceTests
{
    // Creating a local database context, to not interfere with Docker hosted database.
    private static AppDbContext TestDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid
                .NewGuid()
                .ToString()).Options;
        
        var db = new AppDbContext(options);
        
        // Seeding first serial number
        db.SerialNumbers.Add(new SerialNumber
        {
           Id = 1, Number = "ACME-TEST-0000-0001",
           UseCount = 0, MaxUseCount = 1
        });
        
        // Seeding second serial number
        db.SerialNumbers.Add(new SerialNumber
        {
            Id = 2, Number = "ACME-TEST-0000-0002",
            UseCount = 0, MaxUseCount = 1
        });
        
        // Seeding third serial number
        db.SerialNumbers.Add(new SerialNumber
        {
            Id = 3, Number ="ACME-TEST-0000-0003",
            UseCount =  0, MaxUseCount = 1
        });
        
        db.SaveChanges();
        return db;
    }

    private static DrawEntry TestEntry(
        string serial, string? email = "user1@acme.com") =>
        new("Esben", "Ravnholt", "user1@acme.com",
            new DateTime(1990, 1, 1), serial);
    
    
    // TESTS FOR RULE: Serial Number can only be used 1 time
    [Fact]
    public async Task SubmitSucceedsFirstTimeWithSerial()
    {
        var db = TestDb();
        var svc = new DrawService(db);
        
        var result = await svc.SubmitEntryAsync(TestEntry("ACME-TEST-0000-0001"));
        
        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task SubmitFailsSecondTimeWithSerial()
    {
        var db = TestDb();
        var svc = new DrawService(db);
        
        // First use
        await svc.SubmitEntryAsync(TestEntry("ACME-TEST-0000-0001", "user1@acme.com"));
        
        // Second use, should fail
        var result = await svc.SubmitEntryAsync(TestEntry("ACME-TEST-0000-0001", "user2@acme.com"));
        
        result.Success.Should().BeFalse();
        result.ErrorField.Should().Be("Serial number has already been used");
    }
    
    
    // TESTS FOR RULE: Any user can enter Draw twice.
    [Fact]
    public async Task Draw_Succeeds_SecondTime()
    {
        var db = TestDb();
        var svc = new DrawService(db);
        
        await svc.SubmitEntryAsync(
            TestEntry("ACME-TEST-0000-0001", "user1@acme.com"));
        
        var result = await svc.SubmitEntryAsync(TestEntry("ACME-TEST-0000-0002", "user1@acme.com"));
        
        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task Draw_Fails_ThirdTime()
    {
        var db = TestDb();
        var svc = new DrawService(db);
        
        // First two entries, should succeed
        await svc.SubmitEntryAsync(
            TestEntry("ACME-TEST-0000-0001", "user1@acme.com"));
        await svc.SubmitEntryAsync(
            TestEntry("ACME-TEST-0000-0002", "user1@acme.com"));
        
        // Third entry, should fail
        var result = await svc.SubmitEntryAsync(TestEntry("ACME-TEST-0000-0003", "user1@acme.com"));
        
        result.Success.Should().BeFalse();
        result.ErrorField.Should().Be("You have already entered the draw twice.");
    }
}