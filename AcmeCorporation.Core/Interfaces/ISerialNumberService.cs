namespace AcmeCorporation.Core.Interfaces;

public interface ISerialNumberService
{
    Task<bool> IsValidAsync(string serialNumber);
    Task<bool> IsUsableAsync(string serialNumber);
    Task IncrementUseCountAsync(string serialNumber);
}