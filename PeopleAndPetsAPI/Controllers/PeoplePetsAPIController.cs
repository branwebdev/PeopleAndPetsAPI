using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleAndPetsAPI.Data;
using PeopleAndPetsAPI.Entities;

namespace PeopleAndPetsAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PeoplePetsAPIController : ControllerBase
    {
        private readonly DataContext _context;

        public PeoplePetsAPIController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("peopleapi")]
        public async Task<ActionResult<List<Person>>> GetPeople()
        {
            var people = await _context.People.ToListAsync();

            return Ok(people);
        }

        [HttpGet("peopleapi/{searchTerm}")]
        public async Task<ActionResult<List<Person>>> GetPeopleBySearch(string searchTerm)
        {
            var people = await _context.People.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower())).ToListAsync();

            return Ok(people);
        }

        [HttpGet("peopleapi/{id}")]        
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _context.People.FindAsync(id);

            if(person is null)
            {
                return NotFound("Person not found");
            }

            return Ok(person);
        }

        [HttpPost("peopleapi")]
        public async Task<ActionResult<List<Person>>> AddPerson(Person person)
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();           

            return Ok(await _context.People.ToListAsync());
        }

        [HttpPut("peopleapi")]
        public async Task<ActionResult<List<Person>>> UpdatePerson(Person person)
        {
            var dbPerson = await _context.People.FindAsync(person.Id);

            if (dbPerson is null)
            {
                return NotFound("Person not found");
            }

            dbPerson.Name = person.Name;
            dbPerson.Gender = person.Gender;
            dbPerson.Age = person.Age;

            await _context.SaveChangesAsync();

            return Ok(person);
        }

        [HttpDelete("peopleapi/{id}")]
        public async Task<ActionResult<List<Person>>> DeletePerson(int id)
        {
            var dbPerson = await _context.People.FindAsync(id);

            if (dbPerson is null)
            {
                return NotFound("Person not found");
            }

            var dbPersonPets = await _context.Pets.Where(p => p.OwnerId == id).ToListAsync();

            _context.Pets.RemoveRange(dbPersonPets);

            _context.People.Remove(dbPerson);

            await _context.SaveChangesAsync();

            return Ok(await _context.People.ToListAsync());
        }

        [HttpGet("petsapi/owner/{id}")]
        public async Task<ActionResult<List<Pet>>> GetPets(int id)
        {
            var pets = await _context.Pets.Where(p => p.OwnerId == id).ToListAsync();

            return Ok(pets);
        }

        [HttpGet("petsapi/owner/{id}/{searchTerm}")]
        public async Task<ActionResult<List<Person>>> GetPetsBySearch(int id, string searchTerm)
        {
            var pets = await _context.Pets.Where(p => p.OwnerId == id && (p.Name.ToLower().Contains(searchTerm.ToLower()) || p.Type.ToLower().Contains(searchTerm.ToLower()))).ToListAsync();

            return Ok(pets);
        }

        [HttpGet("petsapi/{id}")]
        public async Task<ActionResult<Pet>> GetPet(int id)
        {
            var pet = await _context.Pets.FindAsync(id);

            if (pet is null)
            {
                return NotFound("Pet not found");
            }

            return Ok(pet);
        }

        [HttpPost("petsapi")]
        public async Task<ActionResult<Pet>> AddPet(Pet pet)
        {
            _context.Pets.Add(pet);
            pet = await _context.Pets.FindAsync(pet.Id);
            await _context.SaveChangesAsync();

            return Ok(pet);
        }

        [HttpPut("petsapi")]
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

        [HttpDelete("petsapi/{id}")]
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
