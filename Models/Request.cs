namespace Document_Request.Models
{
    public class Request
    {
        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Purpose { get; set; }
        public string Sex { get; set; }
        public string Rank { get; set; }
        public string Document { get; set; }
        public string Status { get; set; }
        public string DateCreated { get; set; }
    }
}
