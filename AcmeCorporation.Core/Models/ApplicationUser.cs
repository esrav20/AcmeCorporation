using Microsoft.AspNetCore.Identity;

namespace AcmeCorporation.Core.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    
    public ICollection<DrawSubmission> Submissions { get; set; } = new List<DrawSubmission>();
}