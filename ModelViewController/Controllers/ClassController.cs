using AutoMapper;
using Common;
using Metadata;
using Microsoft.AspNetCore.Mvc;
using ModelViewController.Models.ModelsClass;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ModelViewController.Controllers
{
    public class ClassController : Controller
    {

        private readonly IMapper _mapper;

        public ClassController(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Startup.UrlBase + "ClassApi");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string classJsonResponse = await response.Content.ReadAsStringAsync();
                    QueryResponse<Class> classResponse = JsonConvert.DeserializeObject<QueryResponse<Class>>(classJsonResponse);
                    return View(_mapper.Map<List<ClassQueryViewModel>>(classResponse.Data));
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
        public async Task<IActionResult> Create(ClassInsertViewModel viewModel)
        {
            Class clasS = _mapper.Map<Class>(viewModel);

            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(clasS), Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(Startup.UrlBase + "ClassApi", content);
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
