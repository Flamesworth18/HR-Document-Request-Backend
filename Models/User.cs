
namespace Document_Request.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public byte[]?  PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordTokenExpires { get; set; }
        public string Sex { get; set; }
        public string Role { get; set; }
        public int NumberOfRequests { get; set; } = 0;
        public ICollection<Request> Requests { get; set; }
    }
}
