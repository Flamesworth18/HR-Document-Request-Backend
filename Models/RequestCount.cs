namespace Document_Request.Models
{
    public class RequestCount
    {
        [Key]
        public Guid Id { get; set; }
        public int TotalCount { get; set; }
        public int TodayCount { get; set; }
    }
}
