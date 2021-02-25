using AutoMapper;
using Common;
using Metadata;
using Microsoft.AspNetCore.Mvc;
using ModelViewController.Models.ModelsSubject;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ModelViewController.Controllers
{
    public class SubjectController : Controller
    {

        private readonly IMapper _mapper;

        public SubjectController(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Startup.UrlBase + "SubjectApi");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string subjectJsonResponse = await response.Content.ReadAsStringAsync();
                    QueryResponse<Subject> subjectResponse = JsonConvert.DeserializeObject<QueryResponse<Subject>>(subjectJsonResponse);
                    return View(_mapper.Map<List<SubjectQueryViewModel>>(subjectResponse.Data));
                }
                ViewBag.Errors = "Dados não encontrados.";
                return View();
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SubjectInsertViewModel viewModel)
        {
            Subject subject = _mapper.Map<Subject>(viewModel);

            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(subject), Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(Startup.UrlBase + "SubjectApi", content);
                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                Response response = JsonConvert.DeserializeObject<Response>(jsonResponse);
                if (response.Success)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Erros = response.Message;
                return View();
            }
        }
    }
}
