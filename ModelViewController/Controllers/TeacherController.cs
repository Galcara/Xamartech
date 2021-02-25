using AutoMapper;
using Common;
using Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using ModelViewController.Models.ModelLesson;
using ModelViewController.Models.ModelsPresence;
using ModelViewController.Models.ModelsTeacher;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ModelViewController.Controllers
{
    public class TeacherController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IHostEnvironment _appEnvironment;

        public TeacherController(IMapper mapper, IHostEnvironment appEnvironment)
        {
            this._mapper = mapper;
            this._appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Presences(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Startup.UrlBase + "TeacherAPI/Presences?id=" + id);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string teacherJsonResponse = await response.Content.ReadAsStringAsync();
                    QueryResponse<Presence> presences = JsonConvert.DeserializeObject<QueryResponse<Presence>>(teacherJsonResponse);
                    if (presences.Data.Count == 0)
                    {
                        return RedirectToAction("Index");
                    }
                    List<PresenceViewModel> presencas = new List<PresenceViewModel>();
                    presences.Data.ForEach(c => presencas.Add(
                        new PresenceViewModel()
                        {
                            Nome = c.Student.StudentName,
                            Presença = c.Attendance
                        }));
                    return View(presencas);
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Lessons(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Startup.UrlBase + "TeacherAPI/Lessons?id=" + id);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string teacherJsonResponse = await response.Content.ReadAsStringAsync();
                    QueryResponse<Lesson> presences = JsonConvert.DeserializeObject<QueryResponse<Lesson>>(teacherJsonResponse);
                    List<LessonVIewModel> aulas = new List<LessonVIewModel>();
                    foreach (var item in presences.Data)
                    {
                        aulas.Add(new LessonVIewModel()
                        {
                            ID = item.ID,
                            Horario = item.date.ToString("dd/MM/yyyy - HH:mm"),
                            Materia = item.Subject.SubjectName,
                            Turma = item.Class.ClassName
                        });
                    }
                    return View(aulas);
                }
                return RedirectToAction("Index");
            }
        }


        public async Task<IActionResult> Index()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Startup.UrlBase + "TeacherAPI");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string teacherJsonResponse = await response.Content.ReadAsStringAsync();
                    QueryResponse<Teacher> teachers = JsonConvert.DeserializeObject<QueryResponse<Teacher>>(teacherJsonResponse);
                    List<TeacherQueryViewModel> viewmodel = _mapper.Map<List<TeacherQueryViewModel>>(teachers.Data);

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

                HttpResponseMessage responseMessage = await client.GetAsync(Startup.UrlBase + "SubjectAPI");
                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                QueryResponse<Subject> response = JsonConvert.DeserializeObject<QueryResponse<Subject>>(jsonResponse);
                if (!response.Success)
                {
                    return RedirectToAction("Index", "Subject");
                }
                ViewBag.Subjects = response.Data.Select(r => new SelectListItem { Value = r.ID.ToString(), Text = r.SubjectName }).ToList();
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeacherInsertViewModel viewModel)
        {
            if (viewModel.PictureUpload == null || viewModel.PictureUpload.Length == 0)
            {
                ViewBag.Errors = "Foto não enviada.";
                return await Create();
            }

            Teacher teacher = _mapper.Map<Teacher>(viewModel);
            viewModel.Subjects.ForEach(c => teacher.Subjects.Add(new Subject() { ID = c }));

            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(teacher), Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(Startup.UrlBase + "TeacherAPI", content);
                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                SingleResponse<int> response = JsonConvert.DeserializeObject<SingleResponse<int>>(jsonResponse);
                if (response.Success)
                {
                    var arquivo = viewModel.PictureUpload;

                    string pasta = "wwwroot\\Arquivos_Usuario\\Teacher";
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
                ViewBag.Erros = response.Message;
                return View();
            }
        }
    }
}
