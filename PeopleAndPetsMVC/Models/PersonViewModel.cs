using System.ComponentModel.DataAnnotations;

namespace PeopleAndPetsAPI.Entities
{
    /// <summary>
    /// This model is used to add, update or retrieve the pet owner details from the client to the API and vise versa.
    /// </summary>
    public class PersonViewModel
    {
        /// <summary>
        /// Unique pet owner Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the pet owner, which must only contain letters.
        /// </summary>
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must only contain letters")]
        public string Name { get; set; }

        /// <summary>
        /// The gender of the pet owner
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// The pet owner's age
        /// </summary>
        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100")]
        public int Age { get; set; }
    }
}
