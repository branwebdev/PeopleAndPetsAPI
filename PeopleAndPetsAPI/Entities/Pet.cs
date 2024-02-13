using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeopleAndPetsAPI.Entities
{
    /// <summary>
    /// Represents the schema of the pets database table.
    /// </summary>
    public class Pet
    {
        /// <summary>
        /// The Id column of the pets database table.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The name column of the pets database table.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type column of the pets database table.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The owner Id column of the pets database table.
        /// </summary>
        [ForeignKey(nameof(Person.Id))]
        public int OwnerId { get; set; }
    }
}
