using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleAndPetsAPI.Data;
using PeopleAndPetsAPI.Entities;
using PeopleAndPetsAPI.Services;

namespace PeopleAndPetsAPI.Controllers
{
    /// <summary>
    /// This is the pets API controller that passes the pet's details from the client side to the database and vise versa using entity framework.
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PetsAPIController : ControllerBase
    {
        private readonly IPetsService _individualService;

        public PetsAPIController(IPetsService petsServices)
        {
            _individualService = petsServices;
        }

        /// <summary>
        /// Gets the full list of pets from the database and returns them to the client side.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("pets/owner/{id}")]
        public async Task<ActionResult<List<Pet>>> GetPets(int id)
        {
            return Ok(await _individualService.GetIndividualsAsync(id));
        }

        /// <summary>
        /// Gets the list of pets from the database using the search term and owner id and returns them to the client side.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="searchTerm"></param>
        [HttpGet("pets/owner/{id}/{searchTerm}")]
        public async Task<ActionResult<List<Person>>> GetPetsBySearch(int id, string searchTerm)
        {
            return Ok(await _individualService.GetIndividualsBySearch(id, searchTerm));
        }

        /// <summary>
        /// Gets the pet using its Id from the database and returns it to the client side.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("pets/{id}")]
        public async Task<ActionResult<Pet>> GetPet(int id)
        {
            var extractedPet = await _individualService.GetIndividualAsync(id);

            if (extractedPet is null)
            {
                return NotFound("Pet not found");
            }

            return Ok(extractedPet);
        }

        /// <summary>
        /// Adds the pet to the database.
        /// </summary>
        /// <param name="pet"></param>
        [HttpPost("pets")]
        public async Task<ActionResult<IEnumerable<Pet>>> AddPet(Pet pet)
        {
            return Ok(await _individualService.AddIndividualAsync(pet));
        }

        /// <summary>
        /// Updates the details of the pet to the database.
        /// </summary>
        /// <param name="pet"></param>
        [HttpPut("pets")]
        public async Task<ActionResult<Pet>> UpdatePet(Pet pet)
        {
            var extractedPet = await _individualService.UpdateIndividualAsync(pet);

            if (extractedPet is null)
            {
                return NotFound("Pet not found");
            }

            return Ok(extractedPet);
        }

        /// <summary>
        /// Deletes the pet from the database.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("pets/{id}")]
        public async Task<ActionResult<IEnumerable<Pet>>> DeletePet(int id)
        {
            var extractedPets = await _individualService.DeleteIndividualAsync(id);

            if (extractedPets is null)
            {
                return NotFound("Pet not found");
            }

            return Ok(extractedPets);
        }
    }
}
