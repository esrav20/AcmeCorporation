using System.ComponentModel.DataAnnotations;

namespace AcmeCorporation.Web.Models;

public class SubmissionViewModel
{
    // Submission must contain:
    // First Name, Last Name, Email Address,
    // Product Serial Number and Date of Birth
    
    [Required]
    [StringLength(100)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [ StringLength(100)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Product Serial Number")]
    public string SerialNumber { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateTime DateOfBirth { get; set; }
}