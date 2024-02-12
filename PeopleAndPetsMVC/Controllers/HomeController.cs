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
    public class HomeController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7265/api");

        private readonly HttpClient _httpClient;

        //Test comment
        public HomeController(ILogger<HomeController> logger)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        public async Task<IActionResult> Index()
        {
            List<PersonViewModel> peopleList = new List<PersonViewModel>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/peoplepetsapi/GetPeople/peopleapi").Result;

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

        public async Task<IActionResult> LoadEditPersonPage(int id)
        {
            PersonViewModel person = new PersonViewModel();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/peoplepetsapi/GetPerson/peopleapi/{id}").Result;

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

        public async Task<IActionResult> DeletePerson(int Id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + $"/peoplepetsapi/DeletePerson/peopleapi/{Id}");

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

        public async Task<IActionResult> EditPerson(PersonViewModel person)
        {
            var json = JsonConvert.SerializeObject(person);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(_httpClient.BaseAddress + $"/peoplepetsapi/UpdatePerson/peopleapi", content);

            if (response.IsSuccessStatusCode)
            {
                ViewData["EditedSuccessfully"] = "The pet owner has been successfully changed";
            }
            else
            {
                ViewData["EditedSuccessfully"] = "The pet owner has not been successfully changed";
            }

            return View("LoadEditPersonPage", person);
        }

        public IActionResult LoadAddPersonPage() 
        { 
            return View();
        }

        public async Task<IActionResult> AddPerson(PersonViewModel person)
        {
            var json = JsonConvert.SerializeObject(person);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + $"/peoplepetsapi/AddPerson/peopleapi",content);

            if (response.IsSuccessStatusCode)
            {
                ViewData["AddedSuccessfully"] = "The pet owner has been successfully added";
            }
            else
            {
                ViewData["AddedSuccessfully"] = "The pet owner has not been successfully added";
            }

            return View("LoadAddPersonPage", new PersonViewModel());
        }

        public async Task<IActionResult> Search()
        {
            string searchTerm = Request.Form["searchTerm"];
            List<PersonViewModel> peopleList = new List<PersonViewModel>();

            HttpResponseMessage response;

            if (searchTerm != null && searchTerm.Any()) 
            {
                response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/peoplepetsapi/GetPeopleBySearch/peopleapi/{searchTerm}").Result;
            }
            else
            {
                response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/peoplepetsapi/GetPeople/peopleapi").Result;
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
