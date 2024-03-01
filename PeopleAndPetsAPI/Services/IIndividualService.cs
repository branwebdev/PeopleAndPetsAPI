using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PeopleAndPetsAPI.Entities;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PeopleAndPetsAPI.Services
{
    public interface IIndividualService
    {
        public Task<Individual> GetIndividualAsync(int id);

        public Task<Individual> AddIndividualAsync(Individual individual);

        public Task<Individual> UpdateIndividualAsync(Individual individual);

        public Task<Individual> DeleteIndividualAsync(int id);
    }
}
