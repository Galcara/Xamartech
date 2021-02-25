using AutoMapper;
using BusinessLogicalLayer;
using BusinessLogicalLayer.Interfaces;
using Metadata;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelViewController.Models.ModelsAdministrator;
using ModelViewController.Models.ModelsClass;
using ModelViewController.Models.ModelsStudent;
using ModelViewController.Models.ModelsSubject;
using ModelViewController.Models.ModelsTeacher;
using ModelViewController.Models.User;
using System.Text.Json.Serialization;

namespace ModelViewController
{
    public class Startup
    {
        public static string UrlBase = "http://localhost:5000/";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication("CookieAuthentication")
     .AddCookie("CookieAuthentication", config =>
     {
         config.Cookie.Name = "UserLoginCookie";
         config.LoginPath = "/Home/Index";
         config.AccessDeniedPath = "/Home/Index";
     });



            services.AddMvc().AddJsonOptions(c => c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
            services.AddSession();
            services.AddControllersWithViews();

         


            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AdminInsertViewModel, Administrator>();
                cfg.CreateMap<Administrator, AdminQueryViewModel>();
                cfg.CreateMap<Admin_TeacherLoginViewModel, Administrator>();

                cfg.CreateMap<Class, ClassQueryViewModel>();
                cfg.CreateMap<ClassInsertViewModel, Class>();

                cfg.CreateMap<SubjectInsertViewModel, Subject>();
                cfg.CreateMap<Subject, SubjectQueryViewModel>();

                cfg.CreateMap<StudentInsertViewModel, Student>();
                cfg.CreateMap<Student, StudentQueryViewModel>();
                cfg.CreateMap<StudentLoginViewModel, Student>();

                cfg.CreateMap<Teacher, TeacherQueryViewModel>();
                cfg.CreateMap<Admin_TeacherLoginViewModel, Teacher>();
                cfg.CreateMap<TeacherInsertViewModel, Teacher>().ForMember(c => c.Subjects, c => c.Ignore());


            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
