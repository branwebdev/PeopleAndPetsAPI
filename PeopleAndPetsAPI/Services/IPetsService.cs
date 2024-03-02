using PeopleAndPetsAPI.Entities;

namespace PeopleAndPetsAPI.Services
{
    /// <summary>
    /// This interface is created to ensure only pets service implements the following methods.
    /// </summary>
    public interface IPetsService : IIndividualService
    {
        /// <summary>
        /// Gets all the pets from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IEnumerable<Individual>> GetIndividualsAsync(int id);

        /// <summary>
        /// Gets all the pets by search term and owner Id from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public Task<IEnumerable<Individual>> GetIndividualsBySearch(int id, string searchTerm);
    }
}
