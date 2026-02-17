namespace AcmeCorporation.Core.Interfaces;

public record DrawEntry(
    string FirstName,
    string LastName,
    string Email,
    DateTime DateOfBirth,
    string SerialNumber);

public record DrawResult(
    bool Success,
    string? ErrorMessage,
    string? ErrorField);

public interface IDrawService
{
    // Contract for DrawService
    
    Task<DrawResult> SubmitEntryAsync(DrawEntry entry);

}