using Filters.Domain;

namespace Filters.Contracts
{
    public interface IGetterAllRepository
    {
        Task<IEnumerable<Person>> GetAllPersonAsync();
    }
}
