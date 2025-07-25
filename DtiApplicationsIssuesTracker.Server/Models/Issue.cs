using System.ComponentModel.DataAnnotations.Schema;

namespace DtiApplicationsIssuesTracker.Server.Models
{
    [Table("ApplicationIssuesTracking")]
    public class Issue
    {
        public int Id { get; set; }
        public int RepositoryId { get; set; }
        public int CategoryId { get; set; }
        public int? DataSourceId { get; set; }
        public IssueStatus Status { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? Resolution { get; set; }
    }
}
