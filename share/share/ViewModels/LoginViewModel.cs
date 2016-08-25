namespace share
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class LoginGroupViewModel
    {
        public int UGroupId { get; set; }
        public string Password { get; set; }
    }

    public class SelectViewModel
    {
        public int UMemberId { get; set; }
        public string UUserId { get; set; }
    }
}
