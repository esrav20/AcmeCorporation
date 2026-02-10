using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AcmeCorporation.Core.Models;

public class User : IdentityUser
{
    [Required]
    [StringLength(100)]
    public string? FirstName { get; set; }
    
    [Required]
    [StringLength(100)]
    public string? LastName { get; set; }
    
    
    public IEnumerable<Submission>? Submissions { get; set; } = new List<Submission>();
}