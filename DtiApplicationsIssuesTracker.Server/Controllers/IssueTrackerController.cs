using DtiApplicationsIssuesTracker.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DtiApplicationsIssuesTracker.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssueTrackerController : ControllerBase
    {
        private readonly IssueTrackingContext _context;

        public IssueTrackerController(IssueTrackingContext context)
        {
            _context = context;
        }
        [HttpGet("repositories")]
        public async Task<IActionResult> GetRepositories()
        {
            var repos = await _context.Repositories.OrderBy(r => r.Id).ToListAsync();
            return Ok(repos);
        }

        [HttpPost("repositories")]
        public async Task<IActionResult> AddRepository([FromBody] Repository repo)
        {
            if (string.IsNullOrWhiteSpace(repo.Name)) return BadRequest();
            _context.Repositories.Add(repo);
            await _context.SaveChangesAsync();
            return Ok(repo);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var cats = await _context.Categories.OrderBy(c => c.Id).ToListAsync();
            return Ok(cats);
        }

        [HttpPost("categories")]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name)) return BadRequest();
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return Ok(category);
        }

        [HttpGet("datasources")]
        public async Task<IActionResult> GetDataSources()
        {
            var dss = await _context.DataSources.OrderBy(d => d.Id).ToListAsync();
            return Ok(dss);
        }

        [HttpPost("datasources")]
        public async Task<IActionResult> AddDataSource([FromBody] DataSource dataSource)
        {
            if (string.IsNullOrWhiteSpace(dataSource.Name)) return BadRequest();
            _context.DataSources.Add(dataSource);
            await _context.SaveChangesAsync();
            return Ok(dataSource);
        }

        [HttpGet("issues")]
        public async Task<IActionResult> GetIssues()
        {
            var issues = await _context.Issues.OrderBy(i => i.Id).ToListAsync();
            return Ok(issues);
        }

        [HttpPost("issues")]
        public async Task<IActionResult> AddIssue([FromBody] Issue issue)
        {
            if (!await _context.Repositories.AnyAsync(r => r.Id == issue.RepositoryId)) return BadRequest("Invalid repository");
            if (!await _context.Categories.AnyAsync(c => c.Id == issue.CategoryId)) return BadRequest("Invalid category");
            if (issue.DataSourceId.HasValue && !await _context.DataSources.AnyAsync(d => d.Id == issue.DataSourceId.Value)) return BadRequest("Invalid data source");
            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();
            return Ok(issue);
        }

        [HttpGet("issues/{id}")]
        public async Task<IActionResult> GetIssue(int id)
        {
            var issue = await _context.Issues.FindAsync(id);
            if (issue == null) return NotFound();
            return Ok(issue);
        }

        [HttpPut("issues/{id}")]
        public async Task<IActionResult> UpdateIssue(int id, [FromBody] Issue updated)
        {
            var issue = await _context.Issues.FindAsync(id);
            if (issue == null) return NotFound();
            if (!await _context.Repositories.AnyAsync(r => r.Id == updated.RepositoryId)) return BadRequest("Invalid repository");
            if (!await _context.Categories.AnyAsync(c => c.Id == updated.CategoryId)) return BadRequest("Invalid category");
            if (updated.DataSourceId.HasValue && !await _context.DataSources.AnyAsync(d => d.Id == updated.DataSourceId.Value)) return BadRequest("Invalid data source");

            issue.RepositoryId = updated.RepositoryId;
            issue.CategoryId = updated.CategoryId;
            issue.DataSourceId = updated.DataSourceId;
            issue.Status = updated.Status;
            issue.Description = updated.Description;
            issue.Resolution = updated.Resolution;

            await _context.SaveChangesAsync();
            return Ok(issue);
        }

        [HttpDelete("issues/{id}")]
        public async Task<IActionResult> DeleteIssue(int id)
        {
            var issue = await _context.Issues.FindAsync(id);
            if (issue == null) return NotFound();
            _context.Issues.Remove(issue);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
