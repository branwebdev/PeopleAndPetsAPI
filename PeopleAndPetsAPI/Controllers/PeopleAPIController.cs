using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleAndPetsAPI.Data;
using PeopleAndPetsAPI.Entities;

namespace PeopleAndPetsAPI.Controllers
{
    /// <summary>
    /// This is the people API controller that passes the pet owner's details from the client side to the database and vise versa using entity framework.
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PeopleAPIController : ControllerBase
    {
        private readonly DataContext _context;

        public PeopleAPIController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the full list of pet owners from the database and returns them to the client side.
        /// </summary>
        [HttpGet("people")]
        public async Task<ActionResult<List<Person>>> GetPeople()
        {
            var people = await _context.People.ToListAsync();

            return Ok(people);
        }

        /// <summary>
        /// Gets the list of pet owners from the database using the search term and returns them to the client side.
        /// </summary>
        /// <param name="searchTerm"></param>
        [HttpGet("people/{searchTerm}")]
        public async Task<ActionResult<List<Person>>> GetPeopleBySearch(string searchTerm)
        {
            var people = await _context.People.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower())).ToListAsync();

            return Ok(people);
        }

        /// <summary>
        /// Gets the pet owner using his/her Id from the database and returns him/her to the client side.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("people/{id}")]        
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _context.People.FindAsync(id);

            if(person is null)
            {
                return NotFound("Person not found");
            }

            return Ok(person);
        }

        /// <summary>
        /// Adds the pet owner to the database.
        /// </summary>
        /// <param name="person"></param>
        [HttpPost("people")]
        public async Task<ActionResult<List<Person>>> AddPerson(Person person)
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();           

            return Ok(await _context.People.ToListAsync());
        }

        /// <summary>
        /// Updates the details of the pet owner to the database.
        /// </summary>
        /// <param name="person"></param>
        [HttpPut("people")]
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

        /// <summary>
        /// Deletes the pet owner from the database.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("people/{id}")]
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
    }
}
