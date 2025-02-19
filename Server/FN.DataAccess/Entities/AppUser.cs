using Microsoft.AspNetCore.Identity;

namespace FN.DataAccess.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FullName { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public DateTime TimeCreated { get; set; } = DateTime.Now;
        public List<Item> Items { get; set; } = new List<Item>();
    }
}
