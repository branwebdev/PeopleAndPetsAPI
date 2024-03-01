using Microsoft.EntityFrameworkCore;
using PeopleAndPetsAPI.Data;
using PeopleAndPetsAPI.Entities;

namespace PeopleAndPetsAPI.Services
{
    public class PetsService : IPetsService
    {
        private readonly DataContext _context;

        public PetsService(DataContext context)
        {
            _context = context;    
        }

        public async Task<Individual> AddIndividualAsync(Individual individual)
        {
            var pet = individual as Pet;
            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();

            var dbPet = await _context.Pets.FindAsync(pet.Id);

            return dbPet;
        }

        public async Task<Individual> DeleteIndividualAsync(int id)
        {
            var dbPet = await _context.Pets.FindAsync(id);

            if (dbPet is null)
            {
                return null;
            }

            _context.Pets.Remove(dbPet);
            await _context.SaveChangesAsync();

            return dbPet;
        }

        public async Task<Individual> GetIndividualAsync(int id)
        {
            return await _context.Pets.FindAsync(id);
        }

        public async Task<IEnumerable<Individual>> GetIndividualsAsync(int id)
        {
            return await _context.Pets.Where(p => p.OwnerId == id).ToListAsync();
        }

        public async Task<IEnumerable<Individual>> GetIndividualsBySearch(int id, string searchTerm)
        {
            return await _context.Pets.Where(p => p.OwnerId == id && (p.Name.ToLower().Contains(searchTerm.ToLower()) || p.Type.ToLower().Contains(searchTerm.ToLower()))).ToListAsync();
        }

        public async Task<Individual> UpdateIndividualAsync(Individual individual)
        {
            Pet pet = individual as Pet;

            var dbPet = await _context.Pets.FindAsync(pet.Id);

            if(dbPet is null) 
            {
                return null;
            }

            dbPet.Name = pet.Name;
            dbPet.Type = pet.Type;
            dbPet.OwnerId = pet.OwnerId;

            await _context.SaveChangesAsync();

            return dbPet;
        }
    }
}
