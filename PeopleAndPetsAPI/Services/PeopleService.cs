using Microsoft.EntityFrameworkCore;
using PeopleAndPetsAPI.Data;
using PeopleAndPetsAPI.Entities;
using System;

namespace PeopleAndPetsAPI.Services
{
    public class PeopleService : IPeopleService
    {
        private readonly DataContext _context;

        public PeopleService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Individual>> AddIndividualAsync(Individual individual)
        {
            _context.People.Add(individual as Person);
            await _context.SaveChangesAsync();

            return await _context.People.ToListAsync();
        }

        public async Task<IEnumerable<Individual>> DeleteIndividualAsync(int id)
        {
            var dbPerson = await _context.People.FindAsync(id);

            if (dbPerson is null)
            {
                return null;
            }

            var dbPersonPets = await _context.Pets.Where(p => p.OwnerId == id).ToListAsync();
            _context.Pets.RemoveRange(dbPersonPets);
            _context.People.Remove(dbPerson);
            await _context.SaveChangesAsync();

            return await _context.People.ToListAsync();
        }

        public async Task<Individual> GetIndividualAsync(int id)
        {
            var person = await _context.People.FindAsync(id);

            return person;
        }

        public async Task<IEnumerable<Individual>> GetIndividualsAsync()
        {
            var people = await _context.People.ToListAsync();

            return people;
        }

        public async Task<IEnumerable<Individual>> GetIndividualsBySearch(string searchTerm)
        {
            var people = await _context.People.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower())).ToListAsync();

            return people;
        }

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
