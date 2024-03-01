using PeopleAndPetsAPI.Entities;

namespace PeopleAndPetsAPI.Services
{
    public interface IPetsService : IIndividualService
    {
        public Task<IEnumerable<Individual>> GetIndividualsAsync(int id);

        public Task<IEnumerable<Individual>> GetIndividualsBySearch(int id, string searchTerm);
    }
}
