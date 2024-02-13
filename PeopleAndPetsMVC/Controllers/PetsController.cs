using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PeopleAndPetsAPI.Entities;
using PeopleAndPetsMVC.Models;
using System;
using System.Net.Http;
using System.Text;

namespace PeopleAndPetsMVC.Controllers
{
    /// <summary>
    /// The pets controller allows you to add, delete and modify pet owners.
    /// </summary>
    public class PetsController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7265/api");

        private readonly HttpClient _httpClient;

        public PetsController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        /// <summary>
        /// This action returns the main pets page with a list of pets that belongs to a specific owner.
        /// </summary>
        public async Task<IActionResult> Index(int ownerId, string ownerName)
        {
            OwnersPetsViewModel ownersPetViewModel = new OwnersPetsViewModel
            {
                OwnerId = ownerId,
                OwnerName=ownerName,
                Pets = new List<PetViewModel>()
            };

            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/petsapi/GetPets/pets/owner/{ownerId}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();

                if (data?.Any() ?? false)
                {
                    ownersPetViewModel.Pets = JsonConvert.DeserializeObject<List<PetViewModel>>(data);
                }
            }

            return View(ownersPetViewModel);
        }

        /// <summary>
        /// This action loads the pet page for the user to edit as soon as the user clicks on the edit link within the index page.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ownerId"></param>
        /// <param name="ownerName"></param>
        /// <returns></returns>
        public async Task<IActionResult> LoadEditPetPage(int id, int ownerId, string ownerName)
        {
            PetViewModel pet = new PetViewModel();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/petsapi/GetPet/pets/{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();

                if (data?.Any() ?? false)
                {
                    pet = JsonConvert.DeserializeObject<PetViewModel>(data);
                }
            }

            ViewData["OwnerId"] = ownerId;
            ViewData["OwnerName"] = ownerName;

            return View(pet);
        }

        /// <summary>
        /// This action deletes the pet from the index page as soon as the user clicks on the delete link.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ownerId"></param>
        /// <param name="ownerName"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeletePet(int Id, int ownerId, string ownerName)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + $"/petsapi/DeletePet/pets/{Id}");

            if (response.IsSuccessStatusCode)
            {
                ViewData["DeletedSuccessfully"] = "The pet has been successfully deleted";
            }
            else
            {
                ViewData["DeletedSuccessfully"] = "The pet has not been successfully deleted";
            }

            return RedirectToAction(nameof(Index), new { ownerId = ownerId, ownerName = ownerName });
        }

        /// <summary>
        /// This action updates the backend after the user edits the details of an existing pet.
        /// </summary>
        /// <param name="pet"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditPet(PetViewModel pet)
        {
            string ownerName = Request.Form["OwnerName"];

            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(pet);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync(_httpClient.BaseAddress + $"/petsapi/UpdatePet/pets", content);

                if (response.IsSuccessStatusCode)
                {
                    ViewData["EditedSuccessfully"] = "The pet has been successfully changed";

                    string data = await response.Content.ReadAsStringAsync();

                    if (data?.Any() ?? false)
                    {
                        pet = JsonConvert.DeserializeObject<PetViewModel>(data);
                    }
                }
                else
                {
                    ViewData["EditedSuccessfully"] = "The pet has not been successfully changed";
                }
            }
            else
            {
                ViewData["EditedSuccessfully"] = "The pet has not been successfully changed";
            }

            ViewData["OwnerId"] = pet.OwnerId;
            ViewData["OwnerName"] = ownerName;

            return View("LoadEditPetPage", pet);
        }

        /// <summary>
        /// This action loads the pet page for the user to add as soon as the user clicks on the add link within the index page.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="ownerName"></param>
        /// <returns></returns>
        public async Task<IActionResult> LoadAddPetPage(int ownerId, string ownerName)
        {
            ViewData["OwnerId"] = ownerId;
            ViewData["OwnerName"] = ownerName;

            return View();
        }

        /// <summary>
        /// This action adds the new pet to the backend after the user fills in the the details.
        /// </summary>
        /// <param name="pet"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddPet(PetViewModel pet)
        {
            string ownerName = Request.Form["OwnerName"];

            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(pet);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + $"/petsapi/AddPet/pets", content);

                if (response.IsSuccessStatusCode)
                {
                    ViewData["AddedSuccessfully"] = "The pet has been successfully added";
                }
                else
                {
                    ViewData["AddedSuccessfully"] = "The pet has not been successfully added";
                }
            }
            else
            {
                ViewData["AddedSuccessfully"] = "The pet has not been successfully added";
            }

            ViewData["OwnerId"] = pet.OwnerId;
            ViewData["OwnerName"] = ownerName;

            return View("LoadAddPetPage", pet);
        }

        /// <summary>
        /// This action searches for the name/type of the pet after the user type a name or partial name within the search bar and clicks 'Search'.
        /// The pets will be filtered based on the search term. 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Search()
        {
            string searchTerm = Request.Form["searchTerm"];
            int ownerId = Convert.ToInt32(Request.Form["OwnerId"]);
            string ownerName =Request.Form["OwnerName"];

            OwnersPetsViewModel ownersPetViewModel = new OwnersPetsViewModel
            {
                OwnerId = ownerId,
                OwnerName = ownerName,
                Pets = new List<PetViewModel>()
            };

            HttpResponseMessage response;

            if (searchTerm != null && searchTerm.Any())
            {
                response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/petsapi/GetPetsBySearch/pets/owner/{ownerId}/{searchTerm}").Result;
            }
            else
            {
                response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/petsapi/GetPets/pets/owner/{ownerId}").Result;
            }

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();

                if (data?.Any() ?? false)
                {
                    ownersPetViewModel.Pets = JsonConvert.DeserializeObject<List<PetViewModel>>(data);
                }
            }

            return View("Index", ownersPetViewModel);
        }
    }
}
