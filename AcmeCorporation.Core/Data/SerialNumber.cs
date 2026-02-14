using System.ComponentModel.DataAnnotations;

namespace AcmeCorporation.Core.Data;

public class SerialNumber
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Number { get; set; } = string.Empty;

    public int UseCount { get; set; } = 0;
    
    public int MaxUseCount { get; set; } = 1;
    
    public bool IsValid =>  UseCount < MaxUseCount;

    public ICollection<DrawSubmission> Submissions { get; set; } = new List<DrawSubmission>();
}