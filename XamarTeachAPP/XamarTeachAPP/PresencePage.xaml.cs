using Android;
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
    public partial class Page1 : ContentPage
    {

        public Page1(string register)
        {
            InitializeComponent();
            Presence.Clicked += BntFPButtonClicked;
            this.Register = register;
        }

        string Register;
        public static string UrlBase = "https://xamartechwebapi.conveyor.cloud/";



        public async void BntFPButtonClicked(object sender, EventArgs e)
        {

            FingerprintManagerCompat fingerprintManager = FingerprintManagerCompat.From(Android.App.Application.Context);


            if (!fingerprintManager.IsHardwareDetected)
            {

            }

            KeyguardManager keyguardManager = (KeyguardManager)Android.App.Application.Context.GetSystemService(Context.KeyguardService);

            {
                if (!keyguardManager.IsKeyguardSecure)
                {

                }
            }


            if (!fingerprintManager.HasEnrolledFingerprints)
            {

            }
            // The context is typically a reference to the current activity.
            Android.Content.PM.Permission permissionResult = ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.UseFingerprint);
            if (permissionResult == Android.Content.PM.Permission.Granted)
            {


                bool isFingerprintAvailable = await CrossFingerprint.Current.IsAvailableAsync(false);
                if (!isFingerprintAvailable)
                {
                    await DisplayAlert("Error",
                        "Biometric authentication is not available or is not configured.", "OK");
                    return;
                }

                AuthenticationRequestConfiguration conf =
                    new AuthenticationRequestConfiguration("Authentication",
                    "Authenticate access to your personal data");

                var authResult = await CrossFingerprint.Current.AuthenticateAsync(conf);

                if (authResult.Authenticated)
                {


                    //Success  
                    await DisplayAlert("Success", "Authentication succeeded", "OK");
                    using (HttpClient client = new HttpClient())
                    {


                        StringContent content = new StringContent(JsonConvert.SerializeObject(Register), Encoding.UTF8, "application/json");
                        HttpResponseMessage responseMessage = await client.PutAsync(UrlBase + "PresenceAPI", content);
                        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string responseContent = await responseMessage.Content.ReadAsStringAsync();
                            await DisplayAlert("Success", responseContent, "OK");
                        }


                    }


                }

            }

            else
            {
                // No permission. Go and ask for permissions and don't start the scanner. See

            }
        }
    }
}