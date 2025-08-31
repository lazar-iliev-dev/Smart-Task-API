using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class MlSearchService
    {
        private readonly HttpClient _http;
        private readonly ILogger<MlSearchService> _logger;

        public MlSearchService(HttpClient http, ILogger<MlSearchService> logger)
        {
            _http = http;
            _http.BaseAddress = new Uri("http://ml-service:5000/");
            _logger = logger;
        }

        // POST: send query to ML microservice
        public async Task<object?> GetSmartSearchAsync(string query)
        {
            _logger.LogInformation("Sending search query '{Query}' to ML service", query);

            var response = await _http.PostAsJsonAsync("search", new { query });
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<object>();
        }

        // GET: find similar tasks by ID
        public async Task<object?> GetSimilarAsync(string taskId)
        {
            _logger.LogInformation("Requesting similar tasks for ID {TaskId}", taskId);

            return await _http.GetFromJsonAsync<object>($"similar/{taskId}");
        }
    }
}
