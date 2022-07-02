namespace Document_Request.Models.Auth
{
    public class Logout
    {
        [Required]
        public Guid UserId { get; set; }
    }
}
