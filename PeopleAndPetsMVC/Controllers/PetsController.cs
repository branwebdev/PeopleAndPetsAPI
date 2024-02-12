using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PeopleAndPetsAPI.Entities;
using PeopleAndPetsMVC.Models;
using System;
using System.Net.Http;
using System.Text;

namespace PeopleAndPetsMVC.Controllers
{
    public class PetsController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7265/api");

        private readonly HttpClient _httpClient;

        public PetsController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        public async Task<IActionResult> Index(int ownerId, string ownerName)
        {
            OwnersPetsViewModel ownersPetViewModel = new OwnersPetsViewModel
            {
                OwnerId = ownerId,
                OwnerName=ownerName,
                Pets = new List<PetViewModel>()
            };

            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/peoplepetsapi/GetPets/petsapi/owner/{ownerId}").Result;

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

        public async Task<IActionResult> LoadEditPetPage(int id, int ownerId, string ownerName)
        {
            PetViewModel pet = new PetViewModel();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/peoplepetsapi/GetPet/petsapi/{id}").Result;

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

        public async Task<IActionResult> DeletePet(int Id, int ownerId, string ownerName)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + $"/peoplepetsapi/DeletePet/petsapi/{Id}");

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

        public async Task<IActionResult> EditPet(PetViewModel pet)
        {
            string ownerName = Request.Form["OwnerName"];

            var json = JsonConvert.SerializeObject(pet);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(_httpClient.BaseAddress + $"/peoplepetsapi/UpdatePet/petsapi", content);

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

            ViewData["OwnerId"] = pet.OwnerId;
            ViewData["OwnerName"] = ownerName;

            return View("LoadEditPetPage", pet);
        }

        public async Task<IActionResult> LoadAddPetPage(int ownerId, string ownerName)
        {
            ViewData["OwnerId"] = ownerId;
            ViewData["OwnerName"] = ownerName;

            return View();
        }

        public async Task<IActionResult> AddPet(PetViewModel pet)
        {
            string ownerName = Request.Form["OwnerName"];

            var json = JsonConvert.SerializeObject(pet);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + $"/peoplepetsapi/AddPet/petsapi", content);

            if (response.IsSuccessStatusCode)
            {
                ViewData["AddedSuccessfully"] = "The pet has been successfully added";
            }
            else
            {
                ViewData["AddedSuccessfully"] = "The pet has not been successfully added";
            }

            ViewData["OwnerId"] = pet.OwnerId;
            ViewData["OwnerName"] = ownerName;

            return View("LoadAddPetPage", pet);
        }

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
                response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/peoplepetsapi/GetPetsBySearch/petsapi/owner/{ownerId}/{searchTerm}").Result;
            }
            else
            {
                response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/peoplepetsapi/GetPets/petsapi/owner/{ownerId}").Result;
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
