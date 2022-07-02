namespace Document_Request.Data
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<DocumentType> Documents { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Purpose> Purposes { get; set; }
        public DbSet<Amount> Amounts { get; set; }
        public DbSet<RequestCount> RequestCounts { get; set; }
    }
}
