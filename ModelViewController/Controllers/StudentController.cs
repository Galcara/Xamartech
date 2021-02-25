using AutoMapper;
using Common;
using Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using ModelViewController.Models.ModelsStudent;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ModelViewController.Controllers
{
    public class StudentController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IHostEnvironment _appEnvironment;

        public StudentController(IMapper mapper, IHostEnvironment appEnvirnoment)
        {
            this._mapper = mapper;
            this._appEnvironment = appEnvirnoment;
        }

        public async Task<IActionResult> Index()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Startup.UrlBase + "StudentAPI");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string studentJsonResponse = await response.Content.ReadAsStringAsync();
                    QueryResponse<Student> students = JsonConvert.DeserializeObject<QueryResponse<Student>>(studentJsonResponse);
                    List<StudentQueryViewModel> viewmodel = _mapper.Map<List<StudentQueryViewModel>>(students.Data);
                    return View(viewmodel);
                }
                ViewBag.Errors = "Dados não encontrados.";
                return View();
            }
        }

        public async Task<IActionResult> Create()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync(Startup.UrlBase + "ClassAPI");
                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                QueryResponse<Class> response = JsonConvert.DeserializeObject<QueryResponse<Class>>(jsonResponse);
                if (!response.Success)
                {
                    return RedirectToAction("Index", "Class");
                }
                ViewBag.Class = response.Data.Select(r => new SelectListItem { Value = r.ID.ToString(), Text = r.ClassName }).ToList();
                return View();
            }


        }

        [HttpPost]
        public async Task<IActionResult> Create(StudentInsertViewModel viewModel)
        {
            if (viewModel.PictureUpload == null || viewModel.PictureUpload.Length == 0)
            {
                ViewBag.Errors = "Foto não enviada.";
                return await Create();
            }

            Student student = _mapper.Map<Student>(viewModel);

            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(Startup.UrlBase + "StudentAPI", content);
                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                SingleResponse<int> response = JsonConvert.DeserializeObject<SingleResponse<int>>(jsonResponse);
                {
                    var arquivo = viewModel.PictureUpload;

                    string pasta = "wwwroot\\Arquivos_Usuario\\Student";
                    // Define um nome para o arquivo enviado incluindo o sufixo obtido de milesegundos
                    string nomeArquivo = response.Data.ToString();
                    //verifica qual o tipo de arquivo : jpg, gif, png, pdf ou tmp
                    if (arquivo.FileName.Contains(".jpg"))
                        nomeArquivo += ".jpg";
                    else if (arquivo.FileName.Contains(".gif"))
                        nomeArquivo += ".gif";
                    else if (arquivo.FileName.Contains(".png"))
                        nomeArquivo += ".png";
                    else if (arquivo.FileName.Contains(".pdf"))
                        nomeArquivo += ".pdf";
                    else
                        nomeArquivo += ".tmp";


                    //< obtém o caminho físico da pasta wwwroot >
                    string caminho_WebRoot = _appEnvironment.ContentRootPath;
                    string fileFullPath = caminho_WebRoot + "\\" + pasta + "\\" + nomeArquivo;
                    using (FileStream stream = new FileStream(fileFullPath, FileMode.Create))
                    {
                        await arquivo.CopyToAsync(stream);
                    }
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
}
