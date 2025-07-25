using Microsoft.EntityFrameworkCore;

namespace DtiApplicationsIssuesTracker.Server.Models
{
    public class IssueTrackingContext : DbContext
    {
        public IssueTrackingContext(DbContextOptions<IssueTrackingContext> options)
            : base(options)
        {
        }

        public DbSet<Repository> Repositories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<DataSource> DataSources { get; set; }
        public DbSet<Issue> Issues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Issue>().ToTable("ApplicationIssuesTracking");
        }
    }
}
