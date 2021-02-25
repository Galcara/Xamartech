using AutoMapper;
using BusinessLogicalLayer;
using BusinessLogicalLayer.Interfaces;
using Common;
using Metadata;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ModelViewController.Models.ModelsAdministrator;
using ModelViewController.Models.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ModelViewController.Controllers
{
    public class AdministratorController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IHostEnvironment _appEnvironment;
      

        public AdministratorController(IMapper mapper, IHostEnvironment appEnvirnoment)
        {
            this._mapper = mapper;
            this._appEnvironment = appEnvirnoment;
          
        }

        public async Task<IActionResult> Index()
        {
           
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Startup.UrlBase + "AdminAPI");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string AdministratortJsonResponse = await response.Content.ReadAsStringAsync();
                    QueryResponse<Administrator> admins = JsonConvert.DeserializeObject<QueryResponse<Administrator>>(AdministratortJsonResponse);
                    List<AdminQueryViewModel> viewmodel = _mapper.Map<List<AdminQueryViewModel>>(admins.Data);
                    return View(viewmodel);
                }
                ViewBag.Errors = "Dados não encontrados.";
                return View();
            }
        }

        public IActionResult GenerateSchedule()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenerateSchedule(DateTime schoolBegin)
        {
            ScheduleRequest request = new ScheduleRequest()
            {
                InitialScheduleData = schoolBegin
            };

            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(Startup.UrlBase + "AdminAPI/GenerateSchedule", content);
                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                QueryResponse<Lesson> lessonsResponse = JsonConvert.DeserializeObject<QueryResponse<Lesson>>(jsonResponse);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewBag.Errors = lessonsResponse.Message;
                    return View("GenerateSchedule");
                }

                HttpResponseMessage responseClassApi = await client.GetAsync(Startup.UrlBase + "ClassApi");
                string classJsonResponse = await responseClassApi.Content.ReadAsStringAsync();
                QueryResponse<Class> classesResponse = JsonConvert.DeserializeObject<QueryResponse<Class>>(classJsonResponse);
                if (responseClassApi.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewBag.Errors = classesResponse.Message;
                    return View("GenerateSchedule");
                }

                HttpResponseMessage responseSubjectApi = await client.GetAsync(Startup.UrlBase + "SubjectApi");
                string subjectJsonResponse = await responseSubjectApi.Content.ReadAsStringAsync();
                QueryResponse<Subject> subjectsResponse = JsonConvert.DeserializeObject<QueryResponse<Subject>>(subjectJsonResponse);
                if (responseSubjectApi.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewBag.Errors = subjectsResponse.Message;
                    return View("GenerateSchedule");
                }

                HttpResponseMessage responseTeacherApi = await client.GetAsync(Startup.UrlBase + "TeacherAPI");
                string teacherJsonResponse = await responseTeacherApi.Content.ReadAsStringAsync();
                QueryResponse<Teacher> teachersResponse = JsonConvert.DeserializeObject<QueryResponse<Teacher>>(teacherJsonResponse);
                if (responseTeacherApi.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewBag.Errors = teachersResponse.Message;
                    return View("GenerateSchedule");
                }


                ViewBag.Classes = classesResponse.Data;
                ViewBag.Subjects = subjectsResponse.Data;
                ViewBag.Lessons = lessonsResponse.Data;
                ViewBag.Teachers = teachersResponse.Data;

                string data = JsonConvert.SerializeObject(lessonsResponse.Data);
                HttpContext.Session.Set("Lessons5", Encoding.UTF8.GetBytes(data));
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateLessons()
        {
            byte[] data;
            HttpContext.Session.TryGetValue("Lessons5", out data);
            string valorJson = Encoding.UTF8.GetString(data);
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(valorJson, Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(Startup.UrlBase + "AdminAPI/CreateLessons", content);
                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                Response response = JsonConvert.DeserializeObject<Response>(jsonResponse);
            }

            return RedirectToAction("Index");
        }


        public IActionResult Create()
        {
            return View();
        }

        public IActionResult loginADM()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> authenticateADM(Admin_TeacherLoginViewModel ADMLogin)
        {
            Administrator administrator = _mapper.Map<Administrator>(ADMLogin);


            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(administrator), Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(Startup.UrlBase + "AdminAPI/AdmLogin", content);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ViewBag.Errors = "Usuário e/ou senha inválidos.";
                    return View("Index");
                }

                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                User user = JsonConvert.DeserializeObject<User>(jsonResponse);

                List<Claim> userClaims = new List<Claim>()
                {
                    //define o cookie
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.UserData, user.Token),
                    new Claim(ClaimTypes.NameIdentifier, user.ID.ToString())
                };
                ClaimsIdentity minhaIdentity = new ClaimsIdentity(userClaims, "Usuario");
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(new[] { minhaIdentity });

                //cria o cookie
                await HttpContext.SignInAsync(userPrincipal);
                return View(user);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminInsertViewModel viewModel)
        {
            Administrator administrator = _mapper.Map<Administrator>(viewModel);


            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(administrator), Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(Startup.UrlBase + "AdminAPI", content);
                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                SingleResponse<int> response = JsonConvert.DeserializeObject<SingleResponse<int>>(jsonResponse);
                {
                    var arquivo = viewModel.PictureUpload;

                    string pasta = "wwwroot\\Arquivos_Usuario\\Admin";
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

                    return RedirectToAction("Index");
                }
            }
        }
    }
}
