using System.Text.Json;

public class MovieStartupService : IHostedService
{
    private readonly ILogger<MovieStartupService> _logger;

    public MovieStartupService(ILogger<MovieStartupService> logger)
    {
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        string url = "https://jsonmock.hackerrank.com/api/moviesdata/search/?Title=dark";

        using HttpClient client = new HttpClient();

        try
        {
            HttpResponseMessage response = await client.GetAsync(url, cancellationToken);

            string json = await response.Content.ReadAsStringAsync(cancellationToken);

            ApiResponse? result = JsonSerializer.Deserialize<ApiResponse>(json);

            if (result?.data == null)
            {
                _logger.LogWarning("No movie data returned from API.");
                return;
            }

            List<string> titles = result.data
                .Select(movie => movie.Title ?? string.Empty)
                .OrderBy(title => title)
                .ToList();

            _logger.LogInformation("Sorted Titles:");
            foreach (var title in titles)
            {
                _logger.LogInformation("{Title}", title);
            }

            _logger.LogInformation("Original Titles:");
            foreach (var movie in result.data)
            {
                _logger.LogInformation("{Title} ORIGINAL TITLE", movie.Title);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching movies: {Message}", ex.Message);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

public class ApiResponse
{
    public List<Movie>? data { get; set; }
}

public class Movie
{
    public string? Title { get; set; }
}
