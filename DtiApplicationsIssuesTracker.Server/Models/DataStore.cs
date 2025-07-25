using System.Collections.Concurrent;

namespace DtiApplicationsIssuesTracker.Server.Models
{
    public static class DataStore
    {
        public static ConcurrentDictionary<int, Repository> Repositories { get; } = new();
        public static ConcurrentDictionary<int, Category> Categories { get; } = new();
        public static ConcurrentDictionary<int, DataSource> DataSources { get; } = new();
        public static ConcurrentDictionary<int, Issue> Issues { get; } = new();

        private static int _repoId;
        private static int _catId;
        private static int _dsId;
        private static int _issueId;

        static DataStore()
        {
            // Seed with a couple of categories
            var dataCategory = AddCategory("Data");
            AddCategory("Bug");

            // Add sample repository
            AddRepository("SampleRepo");

            // Add sample data source
            AddDataSource("DefaultDB");
        }

        public static Repository AddRepository(string name)
        {
            var repo = new Repository { Id = ++_repoId, Name = name };
            Repositories[repo.Id] = repo;
            return repo;
        }

        public static Category AddCategory(string name)
        {
            var cat = new Category { Id = ++_catId, Name = name };
            Categories[cat.Id] = cat;
            return cat;
        }

        public static DataSource AddDataSource(string name)
        {
            var ds = new DataSource { Id = ++_dsId, Name = name };
            DataSources[ds.Id] = ds;
            return ds;
        }

        public static Issue AddIssue(Issue issue)
        {
            issue.Id = ++_issueId;
            Issues[issue.Id] = issue;
            return issue;
        }
    }
}
