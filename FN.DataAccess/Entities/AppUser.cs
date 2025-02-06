using Microsoft.AspNetCore.Identity;

namespace FN.DataAccess.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FullName { get; set; } = string.Empty;
    }
}
