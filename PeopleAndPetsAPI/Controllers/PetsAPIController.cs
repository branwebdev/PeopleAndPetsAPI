using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleAndPetsAPI.Data;
using PeopleAndPetsAPI.Entities;

namespace PeopleAndPetsAPI.Controllers
{
    /// <summary>
    /// This is the pets API controller that passes the pet's details from the client side to the database and vise versa using entity framework.
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PetsAPIController : ControllerBase
    {
        private readonly DataContext _context;

        public PetsAPIController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the full list of pets from the database and returns them to the client side.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("pets/owner/{id}")]
        public async Task<ActionResult<List<Pet>>> GetPets(int id)
        {
            var pets = await _context.Pets.Where(p => p.OwnerId == id).ToListAsync();

            return Ok(pets);
        }

        /// <summary>
        /// Gets the list of pets from the database using the search term and owner id and returns them to the client side.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="searchTerm"></param>
        [HttpGet("pets/owner/{id}/{searchTerm}")]
        public async Task<ActionResult<List<Person>>> GetPetsBySearch(int id, string searchTerm)
        {
            var pets = await _context.Pets.Where(p => p.OwnerId == id && (p.Name.ToLower().Contains(searchTerm.ToLower()) || p.Type.ToLower().Contains(searchTerm.ToLower()))).ToListAsync();

            return Ok(pets);
        }

        /// <summary>
        /// Gets the pet using its Id from the database and returns it to the client side.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("pets/{id}")]
        public async Task<ActionResult<Pet>> GetPet(int id)
        {
            var pet = await _context.Pets.FindAsync(id);

            if (pet is null)
            {
                return NotFound("Pet not found");
            }

            return Ok(pet);
        }

        /// <summary>
        /// Adds the pet to the database.
        /// </summary>
        /// <param name="pet"></param>
        [HttpPost("pets")]
        public async Task<ActionResult<Pet>> AddPet(Pet pet)
        {
            _context.Pets.Add(pet);
            pet = await _context.Pets.FindAsync(pet.Id);
            await _context.SaveChangesAsync();

            return Ok(pet);
        }

        /// <summary>
        /// Updates the details of the pet to the database.
        /// </summary>
        /// <param name="pet"></param>
        [HttpPut("pets")]
        public async Task<ActionResult<List<Pet>>> UpdatePet(Pet pet)
        {
            var dbPet = await _context.Pets.FindAsync(pet.Id);

            if (dbPet is null)
            {
                return NotFound("Pet not found");
            }

            dbPet.Name = pet.Name;
            dbPet.Type = pet.Type;
            dbPet.OwnerId = pet.OwnerId;

            await _context.SaveChangesAsync();

            return Ok(pet);
        }

        /// <summary>
        /// Deletes the pet from the database.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("pets/{id}")]
        public async Task<ActionResult<List<Pet>>> DeletePet(int id)
        {
            var dbPet = await _context.Pets.FindAsync(id);

            if (dbPet is null)
            {
                return NotFound("Pet not found");
            }

            _context.Pets.Remove(dbPet);
            await _context.SaveChangesAsync();

            return Ok(await _context.Pets.ToListAsync());
        }
    }
}
