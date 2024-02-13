using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PeopleAndPetsAPI.Entities;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;

namespace PeopleAndPetsMVC.Controllers
{
    /// <summary>
    /// The home controller allows you to add, delete and modify pet owners.
    /// </summary>
    public class HomeController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7265/api");

        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        /// <summary>
        /// This action returns the home page with a list of pet owners.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            List<PersonViewModel> peopleList = new List<PersonViewModel>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/peopleapi/GetPeople/people").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();

                if(data?.Any() ?? false) 
                {
                    peopleList = JsonConvert.DeserializeObject<List<PersonViewModel>>(data);
                }                                
            }

            return View(peopleList);
        }

        /// <summary>
        /// This action loads the pet owner page for the user to edit as soon as the user clicks on the edit link within the index page.
        /// </summary>
        /// <param name="id"></param>
        public async Task<IActionResult> LoadEditPersonPage(int id)
        {
            PersonViewModel person = new PersonViewModel();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/peopleapi/GetPerson/people/{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();

                if (data?.Any() ?? false)
                {
                    person = JsonConvert.DeserializeObject<PersonViewModel>(data);
                }
            }

            return View(person);
        }

        /// <summary>
        /// This action deletes the pet owner from the index page as soon as the user clicks on the delete link.
        /// </summary>
        /// <param name="Id"></param>
        public async Task<IActionResult> DeletePerson(int Id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + $"/peopleapi/DeletePerson/people/{Id}");

            if (response.IsSuccessStatusCode)
            {
                ViewData["DeletedSuccessfully"] = "The pet owner has been successfully deleted";
            }
            else
            {
                ViewData["DeletedSuccessfully"] = "The pet owner has not been successfully deleted";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// This action updates the backend after the user edits the details of an existing pet owner.
        /// </summary>
        /// <param name="person"></param>
        public async Task<IActionResult> EditPerson(PersonViewModel person)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(person);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync(_httpClient.BaseAddress + $"/peopleapi/UpdatePerson/people", content);

                if (response.IsSuccessStatusCode)
                {
                    ViewData["EditedSuccessfully"] = "The pet owner has been successfully changed";
                }
                else
                {
                    ViewData["EditedSuccessfully"] = "The pet owner has not been successfully changed";
                }
            }
            else
            {
                ViewData["EditedSuccessfully"] = "The pet owner has not been successfully changed";
            }

            return View("LoadEditPersonPage", person);
        }

        /// <summary>
        /// This action loads the pet owner page for the user to add as soon as the user clicks on the add link within the index page.
        /// </summary>
        public IActionResult LoadAddPersonPage() 
        { 
            return View();
        }

        /// <summary>
        /// This action adds the new pet owner to the backend after the user fills in the the details.
        /// </summary>
        /// <param name="person"></param>
        public async Task<IActionResult> AddPerson(PersonViewModel person)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(person);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + $"/peopleapi/AddPerson/people", content);

                if (response.IsSuccessStatusCode)
                {
                    ViewData["AddedSuccessfully"] = "The pet owner has been successfully added";
                }
                else
                {
                    ViewData["AddedSuccessfully"] = "The pet owner has not been successfully added";
                }
            }
            else
            {
                ViewData["AddedSuccessfully"] = "The pet owner has not been successfully added";
            }


            return View("LoadAddPersonPage", new PersonViewModel());
        }

        /// <summary>
        /// This action searches for the name of the pet owner after the user type a name or partial name within the search bar and clicks 'Search'.
        /// The pet owners will be filtered based on the search term.
        /// </summary>
        public async Task<IActionResult> Search()
        {
            string searchTerm = Request.Form["searchTerm"];
            List<PersonViewModel> peopleList = new List<PersonViewModel>();

            HttpResponseMessage response;

            if (searchTerm != null && searchTerm.Any()) 
            {
                response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/peopleapi/GetPeopleBySearch/people/{searchTerm}").Result;
            }
            else
            {
                response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/peopleapi/GetPeople/people").Result;
            }

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();

                if (data?.Any() ?? false)
                {
                    peopleList = JsonConvert.DeserializeObject<List<PersonViewModel>>(data);
                }
            }

            return View("Index", peopleList);
        }
    }
}
