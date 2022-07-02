namespace Document_Request.Models
{
    public class DocumentType
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
