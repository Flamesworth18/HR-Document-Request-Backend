namespace Document_Request.Models
{
    public class SubUser
    {
        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
    }
}
