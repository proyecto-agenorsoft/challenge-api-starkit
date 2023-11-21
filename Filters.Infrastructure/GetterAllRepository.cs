using Filters.Contracts;
using Filters.Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Filters.Infrastructure
{
    public class GetterAllRepository : IGetterAllRepository
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<GetterAllRepository> _logger;
        private readonly string _fileName = "names.json";
        private readonly string _dataFolder = "Data";

        public GetterAllRepository(IHostEnvironment env, ILogger<GetterAllRepository> logger)
        {
            _env = env;
            _logger = logger;
        }

        public async Task<IEnumerable<Person>> GetAllPersonAsync()
        {
            var filePath = Path.Combine(_env.ContentRootPath, _dataFolder, _fileName);

            try
            {
                if (!File.Exists(filePath))
                {
                    _logger.LogError($"File not found: {filePath}");
                    return new List<Person>();
                }

                var json = await File.ReadAllTextAsync(filePath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var container = JsonSerializer.Deserialize<ResponseContainer>(json, options);

                return container?.Response ?? new List<Person>();
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError($"JSON deserialization error: {jsonEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                throw;
            }
        }

    }
}

