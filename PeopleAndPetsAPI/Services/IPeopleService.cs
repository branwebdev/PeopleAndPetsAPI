using PeopleAndPetsAPI.Entities;

namespace PeopleAndPetsAPI.Services
{
    /// <summary>
    /// This interface is created to ensure only people service implements the following methods.
    /// </summary>
    public interface IPeopleService : IIndividualService
    {
        /// <summary>
        /// Gets all the people from the database.
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Individual>> GetIndividualsAsync();

        /// <summary>
        /// Gets all the people by search term from the database.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public Task<IEnumerable<Individual>> GetIndividualsBySearch(string searchTerm);
    }
}
