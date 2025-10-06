using System.ComponentModel;

using McpDotNetSample.Asp.Services;

using ModelContextProtocol.Server;

namespace McpDotNetSample.Asp.Tools;

[McpServerToolType]
public static class GithubUsersIKnowTool
{
    [McpServerTool, Description("Look up the Github username of someone in your organization based on their name.")]
    public static string LookupGithubUsername(IGithubUserRepository githubUserRepository, string name)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name);

        if (githubUserRepository is null)
        {
            throw new InvalidOperationException("GitHub user repository is not configured.");
        }

        var names = name.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (names.Length == 0)
        {
            return "Please provide a name to search for.";
        }
        else if (names.Length == 1)
        {
            // Search by first name only
            var matches = githubUserRepository.FindByFirstName(names[0]).ToList();

            return matches.Count switch
            {
                0 => $"No GitHub user found with first name '{names[0]}'.",
                1 => $"GitHub username for {names[0]}: {matches[0]}",
                _ => $"Multiple users found with first name '{names[0]}': {string.Join(", ", matches)}. Please provide a full name."
            };
        }
        else
        {
            // Search by full name (first and last)
            string firstName = names[0];
            string lastName = names[^1]; // Last element

            var username = githubUserRepository.FindByFullName(firstName, lastName);

            if (username is not null)
            {
                return $"GitHub username for {firstName} {lastName}: {username}";
            }

            // If not found by full name, try searching by first name
            var firstNameMatches = githubUserRepository.FindByFirstName(firstName).ToList();

            if (firstNameMatches.Count > 0)
            {
                return $"No exact match for '{firstName} {lastName}'. Users with first name '{firstName}': {string.Join(", ", firstNameMatches)}";
            }

            // Try searching by last name as a fallback
            var lastNameMatches = githubUserRepository.FindByLastName(lastName).ToList();

            if (lastNameMatches.Count > 0)
            {
                return $"No match for first name '{firstName}'. Users with last name '{lastName}': {string.Join(", ", lastNameMatches)}";
            }

            return $"No GitHub user found matching '{name}'.";
        }
    }
}