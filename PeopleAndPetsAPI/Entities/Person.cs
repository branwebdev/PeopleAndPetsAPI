using System.ComponentModel.DataAnnotations;

namespace PeopleAndPetsAPI.Entities
{
    /// <summary>
    /// Represents the schema of the people database table.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// The Id column of the people database table.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The name column of the people database table.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The gender column of the people database table.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// The age column of the people database table.
        /// </summary>
        public int Age { get; set; }
    }
}
