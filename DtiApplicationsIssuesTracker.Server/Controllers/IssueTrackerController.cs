using DtiApplicationsIssuesTracker.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace DtiApplicationsIssuesTracker.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssueTrackerController : ControllerBase
    {
        [HttpGet("repositories")]
        public IActionResult GetRepositories()
        {
            return Ok(DataStore.Repositories.Values.OrderBy(r => r.Id));
        }

        [HttpPost("repositories")]
        public IActionResult AddRepository([FromBody] Repository repo)
        {
            if (string.IsNullOrWhiteSpace(repo.Name)) return BadRequest();
            var added = DataStore.AddRepository(repo.Name);
            return Ok(added);
        }

        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            return Ok(DataStore.Categories.Values.OrderBy(c => c.Id));
        }

        [HttpPost("categories")]
        public IActionResult AddCategory([FromBody] Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name)) return BadRequest();
            var added = DataStore.AddCategory(category.Name);
            return Ok(added);
        }

        [HttpGet("datasources")]
        public IActionResult GetDataSources()
        {
            return Ok(DataStore.DataSources.Values.OrderBy(d => d.Id));
        }

        [HttpPost("datasources")]
        public IActionResult AddDataSource([FromBody] DataSource dataSource)
        {
            if (string.IsNullOrWhiteSpace(dataSource.Name)) return BadRequest();
            var added = DataStore.AddDataSource(dataSource.Name);
            return Ok(added);
        }

        [HttpGet("issues")]
        public IActionResult GetIssues()
        {
            return Ok(DataStore.Issues.Values.OrderBy(i => i.Id));
        }

        [HttpPost("issues")]
        public IActionResult AddIssue([FromBody] Issue issue)
        {
            if (!DataStore.Repositories.ContainsKey(issue.RepositoryId)) return BadRequest("Invalid repository");
            if (!DataStore.Categories.ContainsKey(issue.CategoryId)) return BadRequest("Invalid category");
            if (issue.DataSourceId.HasValue && !DataStore.DataSources.ContainsKey(issue.DataSourceId.Value)) return BadRequest("Invalid data source");
            var added = DataStore.AddIssue(issue);
            return Ok(added);
        }
    }
}
