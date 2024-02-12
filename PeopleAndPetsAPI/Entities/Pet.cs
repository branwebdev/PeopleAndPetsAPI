using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeopleAndPetsAPI.Entities
{
    public class Pet
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        [ForeignKey(nameof(Person.Id))]
        public int OwnerId { get; set; }
    }
}
