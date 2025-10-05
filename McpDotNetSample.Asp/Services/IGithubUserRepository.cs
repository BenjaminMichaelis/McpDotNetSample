namespace McpDotNetSample.Asp.Services;

/// <summary>
/// Repository for looking up GitHub usernames by name.
/// </summary>
public interface IGithubUserRepository
{
    /// <summary>
    /// Finds GitHub users by first name.
    /// </summary>
    /// <param name="firstName">The first name to search for.</param>
    /// <returns>A collection of matching GitHub usernames.</returns>
    IEnumerable<string> FindByFirstName(string firstName);

    /// <summary>
    /// Finds GitHub users by last name.
    /// </summary>
    /// <param name="lastName">The last name to search for.</param>
    /// <returns>A collection of matching GitHub usernames.</returns>
    IEnumerable<string> FindByLastName(string lastName);

    /// <summary>
    /// Finds a GitHub user by full name (first and last).
    /// </summary>
    /// <param name="firstName">The first name.</param>
    /// <param name="lastName">The last name.</param>
    /// <returns>The GitHub username if found, otherwise null.</returns>
    string? FindByFullName(string firstName, string lastName);
}
