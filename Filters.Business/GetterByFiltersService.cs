using Filters.Contracts;
using Filters.Domain;
using Microsoft.Extensions.Logging;

namespace Filters.Services
{
    public class GetterByFiltersService : IGetterByFiltersService
    {
        private readonly IGetterAllRepository _getterAllRepository;
        private readonly ILogger<GetterByFiltersService> _logger;

        public GetterByFiltersService(IGetterAllRepository getterAllRepository, ILogger<GetterByFiltersService> logger)
        {
            _getterAllRepository = getterAllRepository;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene una lista de personas filtrada por genero y nombre
        /// </summary>
        /// <param name="gender"></param>
        /// <param name="nameStartsWith"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Person>> GetPersonsByFiltersAsync(char? gender, string? nameStartsWith)
        {
            IEnumerable<Person> persons = new List<Person>();
            try
            {
                persons = await _getterAllRepository.GetAllPersonAsync();

                if (gender.HasValue)
                {
                    gender = char.ToUpper(gender.Value);

                    if (gender == 'M' || gender == 'F')
                    {
                        persons = persons.Where(p => p.Gender == gender);
                    }
                }

                if (!string.IsNullOrEmpty(nameStartsWith))
                {
                    persons = persons.Where(p => p.Name?.StartsWith(nameStartsWith, StringComparison.OrdinalIgnoreCase) ?? false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving filtered names.");
                throw;
            }

            return persons;
        }

    }
}
