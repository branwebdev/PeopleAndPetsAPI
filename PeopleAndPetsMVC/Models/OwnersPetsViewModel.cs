using PeopleAndPetsAPI.Entities;

namespace PeopleAndPetsMVC.Models
{
    /// <summary>
    /// The model is needed as we need the owner details that is dedicated to the main pets page
    /// </summary>
    public class OwnersPetsViewModel
    {
        /// <summary>
        /// The unique Id of the pet owner.
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// The name of the pet owner.
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// The list of pets for each owner.
        /// </summary>
        public List<PetViewModel> Pets { get; set; }
    }
}
