using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AcmeCorporation.Core.Models;


public class Submission
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string SerialNumber { get; set; } = string.Empty;
    
    public DateTime SubmissionDate { get; set; }
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    
}