using System.ComponentModel.DataAnnotations;

namespace AcmeCorporation.Core.Models;

public class SerialNumber
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string SN { get; set; } = string.Empty;

    public int UseCount { get; set; } = 0;
    
    public int MaxUseCount { get; set; } = 2;
    
    public bool IsValid =>  UseCount < MaxUseCount;

}