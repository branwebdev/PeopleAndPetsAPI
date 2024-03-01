using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleAndPetsAPI.Data;
using PeopleAndPetsAPI.Entities;
using PeopleAndPetsAPI.Services;

namespace PeopleAndPetsAPI.Controllers
{
    /// <summary>
    /// This is the people API controller that passes the pet owner's details from the client side to the database and vise versa using entity framework.
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PeopleAPIController : ControllerBase
    {
        private readonly IPeopleService _peopleService;

        public PeopleAPIController(IPeopleService peopleService)
        {
            _peopleService = peopleService;
        }

        /// <summary>
        /// Gets the full list of pet owners from the database and returns them to the client side.
        /// </summary>
        [HttpGet("people")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            return Ok(await _peopleService.GetIndividualsAsync());
        }

        /// <summary>
        /// Gets the list of pet owners from the database using the search term and returns them to the client side.
        /// </summary>
        /// <param name="searchTerm"></param>
        [HttpGet("people/{searchTerm}")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeopleBySearch(string searchTerm)
        {
            return Ok(await _peopleService.GetIndividualsBySearch(searchTerm));
        }

        /// <summary>
        /// Gets the pet owner using his/her Id from the database and returns him/her to the client side.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("people/{id}")]        
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var extractedPerson = await _peopleService.GetIndividualAsync(id);

            if(extractedPerson is null)
            {
                return NotFound("Person not found");
            }

            return Ok(extractedPerson);
        }

        /// <summary>
        /// Adds the pet owner to the database.
        /// </summary>
        /// <param name="person"></param>
        [HttpPost("people")]
        public async Task<ActionResult> AddPerson(Person person)
        {
            var extractedPerson = await _peopleService.AddIndividualAsync(person);

            if(extractedPerson is null)
            {
                return NotFound("Person not added");
            }

            return Ok();
        }

        /// <summary>
        /// Updates the details of the pet owner to the database.
        /// </summary>
        /// <param name="person"></param>
        [HttpPut("people")]
        public async Task<ActionResult> UpdatePerson(Person person)
        {
            var extractedPerson = await _peopleService.UpdateIndividualAsync(person);

            if (extractedPerson is null)
            {
                return NotFound("Person not found");
            }

            return Ok();
        }

        /// <summary>
        /// Deletes the pet owner from the database.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("people/{id}")]
        public async Task<ActionResult> DeletePerson(int id)
        {
            var extractedPerson = await _peopleService.DeleteIndividualAsync(id);

            if (extractedPerson is null)
            {
                return NotFound("Person not found");
            }

            return Ok();
        }        
    }
}