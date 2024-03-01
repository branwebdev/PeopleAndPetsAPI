using PeopleAndPetsAPI.Entities;

namespace PeopleAndPetsAPI.Services
{
    public interface IPeopleService : IIndividualService
    {
        public Task<IEnumerable<Individual>> GetIndividualsAsync();

        public Task<IEnumerable<Individual>> GetIndividualsBySearch(string searchTerm);
    }
}
