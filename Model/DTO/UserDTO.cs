namespace MASAR.Model.DTO
{
    public class UserDTO
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public bool IsProfileCreated { get; set; }
        public IList<string> Roles { get; set; }
    }
}