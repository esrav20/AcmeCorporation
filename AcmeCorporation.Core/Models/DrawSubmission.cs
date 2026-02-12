using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AcmeCorporation.Core.Models;


public class DrawSubmission
{
    public int Id { get; set; }

    [Required] 
    [StringLength(100)] 
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [ StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    // FK → SerialNumber
    public int SerialNumberId { get; set; }
    public SerialNumber SerialNumber { get; set; } = null!;

    // FK → ApplicationUser
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }
}