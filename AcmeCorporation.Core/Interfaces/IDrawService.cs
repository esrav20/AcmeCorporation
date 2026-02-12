namespace AcmeCorporation.Core.Interfaces;

public record DrawEntry(
    string FirstName,
    string LastName,
    string Email,
    DateTime DateOfBirth,
    string SerialNumber,
    string userId);

public record DrawResult(
    bool Success,
    string? ErrorMessage,
    string? ErrorField);

public interface IDrawService
{
    Task<DrawResult> SubmitEntryAsync(DrawEntry entry);

}