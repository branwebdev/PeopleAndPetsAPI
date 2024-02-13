using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeopleAndPetsAPI.Entities
{
    /// <summary>
    /// This model is used to add, update or retrieve the pet details from the client to the API and vise versa.
    /// </summary>
    public class PetViewModel
    {
        /// <summary>
        /// Unique pet Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the pet, which must only contain letters.
        /// </summary>
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must only contain letters")]
        public string Name { get; set; }

        /// <summary>
        /// Type of pet.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The owner of this pet with an Id specified.
        /// </summary>
        public int OwnerId { get; set; }
    }
}
