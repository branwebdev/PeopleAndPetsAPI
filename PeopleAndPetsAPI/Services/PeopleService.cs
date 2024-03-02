using Microsoft.EntityFrameworkCore;
using PeopleAndPetsAPI.Data;
using PeopleAndPetsAPI.Entities;
using System;

namespace PeopleAndPetsAPI.Services
{
    /// <summary>
    /// This is the People Service that interacts with the people database table.
    /// </summary>
    public class PeopleService : IPeopleService
    {
        private readonly DataContext _context;

        public PeopleService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds the people into the database
        /// </summary>
        /// <param name="individual"></param>
        /// <returns></returns>
        public async Task<Individual> AddIndividualAsync(Individual individual)
        {
            var person = individual as Person;
            _context.People.Add(person);
            await _context.SaveChangesAsync();

            var dbPerson = await _context.People.FindAsync(person.Id);

            return dbPerson;
        }

        /// <summary>
        /// Deletes the person from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Individual> DeleteIndividualAsync(int id)
        {
            var dbPerson = await _context.People.FindAsync(id);

            if (dbPerson is null)
            {
                return null;
            }

            var dbPersonPets = await _context.Pets.Where(p => p.OwnerId == id).ToListAsync();

            if (dbPersonPets != null && dbPersonPets.Any())
            {
                _context.Pets.RemoveRange(dbPersonPets);
            }

            _context.People.Remove(dbPerson);
            await _context.SaveChangesAsync();

            return dbPerson;
        }

        /// <summary>
        /// Gets the person from the database using the person's Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Individual> GetIndividualAsync(int id)
        {
            var person = await _context.People.FindAsync(id);

            return person;
        }

        /// <summary>
        /// Gets all the people from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Individual>> GetIndividualsAsync()
        {
            var people = await _context.People.ToListAsync();

            return people;
        }

        /// <summary>
        /// Gets all the people from the database using the search term.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Individual>> GetIndividualsBySearch(string searchTerm)
        {
            var people = await _context.People.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower())).ToListAsync();

            return people;
        }

        /// <summary>
        /// Updates the person to the database.
        /// </summary>
        /// <param name="individual"></param>
        /// <returns></returns>
        public async Task<Individual> UpdateIndividualAsync(Individual individual)
        {
            Person person = individual as Person;

            var dbPerson = await _context.People.FindAsync(person.Id);

            if(dbPerson is null)
            {
                return null;
            }

            dbPerson.Name = person.Name;
            dbPerson.Gender = person.Gender;
            dbPerson.Age = person.Age;

            await _context.SaveChangesAsync();

            return dbPerson;
        }
    }
}
