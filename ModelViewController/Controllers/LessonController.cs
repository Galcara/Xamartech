using AutoMapper;
using Metadata;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ModelViewController.Controllers
{
    public class LessonController : Controller
    {

        private readonly IMapper _mapper;

        public LessonController(IMapper mapper)
        {
            this._mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Startup.UrlBase + "LessonApi");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string lessonJsonResponse = await response.Content.ReadAsStringAsync();
                    List<Lesson> lesson = JsonConvert.DeserializeObject<List<Lesson>>(lessonJsonResponse);
                    return View(lesson);
                }
                ViewBag.Errors = "Dados não encontrados.";
                return View();
            }
        }
        public IActionResult Create()
        {
            return View();
        }
    }
}
