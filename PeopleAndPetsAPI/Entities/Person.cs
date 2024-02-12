using System.ComponentModel.DataAnnotations;

namespace PeopleAndPetsAPI.Entities
{
    public class Person
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }
    }
}
