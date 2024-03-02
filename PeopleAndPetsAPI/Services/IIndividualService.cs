using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PeopleAndPetsAPI.Entities;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PeopleAndPetsAPI.Services
{
    /// <summary>
    /// This interface will be used by the people and pets service so they can implement the following methods.
    /// </summary>
    public interface IIndividualService
    {
        /// <summary>
        /// Gets an entity from the database depending on the service.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Individual> GetIndividualAsync(int id);

        /// <summary>
        /// Add an individual to the database depending on the service.
        /// </summary>
        /// <param name="individual"></param>
        /// <returns></returns>
        public Task<Individual> AddIndividualAsync(Individual individual);

        /// <summary>
        /// Updates the individual to the database depending on the service.
        /// </summary>
        /// <param name="individual"></param>
        /// <returns></returns>
        public Task<Individual> UpdateIndividualAsync(Individual individual);

        /// <summary>
        /// Deletes the individual from the database depending on the service.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Individual> DeleteIndividualAsync(int id);
    }
}
