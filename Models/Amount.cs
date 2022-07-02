namespace Document_Request.Models
{
    public class Amount
    {
        [Key]
        public Guid Id { get; set; }
        public string Number { get; set; }
    }
}
