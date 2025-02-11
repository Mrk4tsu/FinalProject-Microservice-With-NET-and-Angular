namespace FN.ViewModel.Systems.User
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public string Avatar { get; set; } = string.Empty;
        public DateTime TimeCreated { get; set; }
    }
}
