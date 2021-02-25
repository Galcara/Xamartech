using AutoMapper;
using BusinessLogicalLayer.Interfaces;
using Common;
using Metadata;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelViewController.Models;
using ModelViewController.Models.User;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ModelViewController.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;


        public HomeController(ILogger<HomeController> logger, IMapper mapeer)
        {
            this._mapper = mapeer;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult LoginAdm_Teacher()
        {
            return View();
        }

        [HttpPut]
        public async Task<IActionResult> PresenceStudent(StudentLoginViewModel studentLogin)
        {

            Student student = _mapper.Map<Student>(studentLogin);

            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(student.Register), Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PutAsync(Startup.UrlBase + "PresenceAPI/UpdateFrequency", content);

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
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.PrimarySid, user.UserName),
                    new Claim(ClaimTypes.UserData, user.Token)
                };
                ClaimsIdentity minhaIdentity = new ClaimsIdentity(userClaims, "Usuario");
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(new[] { minhaIdentity });


                //cria o cookie
                await HttpContext.SignInAsync(userPrincipal);
                return View(user);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AutenticateTeacher(Admin_TeacherLoginViewModel teacherLogin)
        {
            Teacher teacher = _mapper.Map<Teacher>(teacherLogin);
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(teacher), Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(Startup.UrlBase + "TeacherAPI/Login", content);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ViewBag.Errors = "Usuário e/ou senha inválidos.";
                    return View();
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
        public async Task<IActionResult> AutenticateStudent(StudentLoginViewModel studentLogin)
        {
            Student student = _mapper.Map<Student>(studentLogin);

            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(Startup.UrlBase + "StudentAPI/Login", content);
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
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.PrimarySid, user.UserName),
                    new Claim(ClaimTypes.UserData, user.Token)
                };
                ClaimsIdentity minhaIdentity = new ClaimsIdentity(userClaims, "Usuario");
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(new[] { minhaIdentity });


                //cria o cookie
                await HttpContext.SignInAsync(userPrincipal);
                return View(user);
            }

        }


    }
}
