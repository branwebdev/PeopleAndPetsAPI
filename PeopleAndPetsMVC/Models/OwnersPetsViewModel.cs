using PeopleAndPetsAPI.Entities;

namespace PeopleAndPetsMVC.Models
{
    public class OwnersPetsViewModel
    {
        public int OwnerId { get; set; }

        public string OwnerName { get; set; }

        public List<PetViewModel> Pets { get; set; }
    }
}
