using Filters.Domain;

namespace Filters.Contracts
{
    public interface IGetterByFiltersService
    {
        Task<IEnumerable<Person>> GetPersonsByFiltersAsync(char? gender, string? nameStartsWith);
    }
}
