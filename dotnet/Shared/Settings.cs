using Microsoft.Extensions.Configuration;

public static class Settings
{
    public static (string apiKey, string? orgId, string? searchServiceName, string? searchServiceAdminKey, string? searchIndexName)
        LoadSettings(string configFile = "config/settings.json")
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory());

        // Add the JSON file if it exists
        if (File.Exists(configFile))
        {
            builder.AddJsonFile(configFile, optional: true, reloadOnChange: true);
        }

        // Add user secrets
        builder.AddUserSecrets<Program>(optional: true);

        // Build the configuration
        var configuration = builder.Build();

        try
        {
            // Retrieve required fields
            string? apiKey = configuration["apiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new Exception("The 'apiKey' must be provided.");
            }

            // orgId is optional
            string? orgId = configuration["orgId"];

            // Retrieve other required fields
            string? searchServiceName = configuration["ARXIV_SEARCH_SERVICE_NAME"];

            string? searchServiceAdminKey = configuration["ARXIV_SEARCH_ADMIN_KEY"];

            string? searchIndexName = configuration["ARXIV_SEARCH_INDEX_NAME"];

            return (apiKey, orgId, searchServiceName, searchServiceAdminKey, searchIndexName);
        }
        catch (Exception e)
        {
            Console.WriteLine("Something went wrong: " + e.Message);
            return ("", "", "", "", "");
        }
    }
}
