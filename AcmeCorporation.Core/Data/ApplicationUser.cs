using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AcmeCorporation.Core.Data;

// User entity, inherits IdentityUser from Microsoft.AspNetCore.Identity
public class ApplicationUser : IdentityUser
{
    // Setting FirstName and LastName as required properties
    [Required]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    public string LastName { get; set; } = string.Empty;

    // FK -> DrawSubmission
    public ICollection<DrawSubmission> Submissions { get; set; } = new List<DrawSubmission>();
}