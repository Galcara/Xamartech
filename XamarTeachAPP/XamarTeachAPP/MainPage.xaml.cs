using Android.App;
using Android.Content;
using Android.Support.V4.Content;
using Android.Support.V4.Hardware.Fingerprint;
using Newtonsoft.Json;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarTeachAPP
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public static string UrlBase = "https://xamartechwebapi.conveyor.cloud/";

        public MainPage()
        {
            InitializeComponent();
            LoginStudent.Clicked += BntLoginButtonClicked;
            lblFalhaLogin.IsVisible = false;
        }

       
        public async void BntLoginButtonClicked(object sender, EventArgs e)
        {
            lblFalhaLogin.IsVisible = false;
            var usuario = new
            {
                Register = studentRegister.Text,
                Passcode = studentPasscode.Text
            };
            using (HttpClient client = new HttpClient())

            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(UrlBase + "StudentAPI/Login", content);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseContent = await responseMessage.Content.ReadAsStringAsync();

                    await Navigation.PushAsync(new Page1(usuario.Register),true);
                    
                }
                else
                {
                    lblFalhaLogin.IsVisible = true;
                }

            }
        }
    }
}