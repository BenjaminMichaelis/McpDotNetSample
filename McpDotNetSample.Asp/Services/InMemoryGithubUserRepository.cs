namespace McpDotNetSample.Asp.Services;

/// <summary>
/// In-memory implementation of GitHub user repository.
/// This can be replaced with a real GitHub API implementation in the future.
/// </summary>
public class InMemoryGithubUserRepository : IGithubUserRepository
{
    // Dictionary mapping: (FirstName, LastName) -> GitHubUsername
    private readonly Dictionary<(string FirstName, string LastName), string> _users = new(new NameComparer())
    {
        { ("Kevin", "Bost"), "keboo" },
        { ("Jane", "Smith"), "janesmith" },
        { ("Bob", "Johnson"), "bobjohnson" },
        { ("Alice", "Williams"), "alicew" },
        { ("Charlie", "Brown"), "charlieb" },
        { ("David", "Miller"), "dmiller" },
        { ("Emma", "Davis"), "emmadavis" },
        { ("Frank", "Wilson"), "fwilson" },
        { ("Grace", "Moore"), "gracemoore" },
        { ("Henry", "Taylor"), "htaylor" }
    };

    public IEnumerable<string> FindByFirstName(string firstName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);

        return _users
            .Where(kvp => kvp.Key.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase))
            .Select(kvp => kvp.Value)
            .ToList();
    }

    public IEnumerable<string> FindByLastName(string lastName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);

        return _users
            .Where(kvp => kvp.Key.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase))
            .Select(kvp => kvp.Value)
            .ToList();
    }

    public string? FindByFullName(string firstName, string lastName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);

        var key = (firstName, lastName);
        return _users.TryGetValue(key, out var username) ? username : null;
    }

    // Custom comparer for case-insensitive tuple comparison
    private class NameComparer : IEqualityComparer<(string FirstName, string LastName)>
    {
        public bool Equals((string FirstName, string LastName) x, (string FirstName, string LastName) y)
        {
            return string.Equals(x.FirstName, y.FirstName, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(x.LastName, y.LastName, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode((string FirstName, string LastName) obj)
        {
            return HashCode.Combine(
                obj.FirstName?.ToLowerInvariant(),
                obj.LastName?.ToLowerInvariant());
        }
    }
}
