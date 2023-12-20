using Newtonsoft.Json;
using KentekenApp.Models;
using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using System.Xml;

namespace KentekenApp
{
    public partial class Info : ContentPage
    {
        public string KentekenText { get; set; }

        public Info(string kentekenText)
            :base()
        {
            KentekenText = kentekenText;
            InitializeComponent();
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            // Makes string uppercase and removes - incase people put it in
            string KentekenUsable = KentekenText.Replace("-", "");
            KentekenUsable = KentekenUsable.ToUpper();
            await CallApiAsync(KentekenUsable);
        }

        /// <summary>
        /// CallApiSync
        /// Calls function to call the api
        /// Returns vechicleinfo with a kenteken that is specified
        /// </summary>
        /// <returns></returns>
        private async Task CallApiAsync(string kenteken)
        {
            // Makes api url link
          
            string apiUrl = "https://opendata.rdw.nl/resource/qyrd-w56j.json?kenteken=" + kenteken;

            // Call the API and get the response
            string apiResponse = await GetApiResponse(apiUrl);

            if (apiResponse != null)
            {
                // Deserialize the JSON response into a list of ApiValues
                List<ApiValues> vehicleInfoList = JsonConvert.DeserializeObject<List<ApiValues>>(apiResponse);

                // Import the kenteken
                string targetKenteken = kenteken;

                // Find the ApiValues object with the specified kenteken
                ApiValues vehicleInfo = vehicleInfoList.FirstOrDefault(info => info.kenteken == targetKenteken);

                // Image api? Geen idee hoe lang dit blijft werken
                string apiUrlCar = "https://www.carimagery.com/api.asmx/GetImageUrl?searchTerm=" + vehicleInfo.handelsbenaming;

                // Call the API and get the response
               string apiResponseCar = await GetApiResponse(apiUrlCar);


                if (vehicleInfo.handelsbenaming != null)
                {
                    // Removes first 86 characters removes "" and adds http so the link is correct
                    apiResponseCar = apiResponseCar.Substring(86);
                    apiResponseCar = apiResponseCar.Replace("</string>", "");
                    apiResponseCar = apiResponseCar.Replace("\"", "");
                    apiResponseCar = "https://" + apiResponseCar;

                    // Calls the function loads the image sets the image
                    LoadImage(apiResponseCar);
                } else
                {
                        // Sends error and send back to homepage
                        await DisplayAlert("Verkeerde Kenteken", "Invalide Kenteken Plaat", "OK");
                        await Navigation.PushAsync(new MainPage());
                }

                if (vehicleInfo != null)
                {
                   // Update Values
                    Kenteken.Text = vehicleInfo.kenteken;
                    Voertuigsoort.Text = vehicleInfo.voertuigsoort;
                    Merk.Text = vehicleInfo.merk;
                    Handelsbenaming.Text = vehicleInfo.handelsbenaming;
                    Prijs.Text = vehicleInfo.catalogusprijs;
                    massa.Text = vehicleInfo.massa_ledig_voertuig + " KG";
                   

                    // Fixes dates do dutch time zone
                    DatumNL.Text = DateTime.ParseExact(vehicleInfo.datum_eerste_tenaamstelling_in_nederland, "yyyyMMdd", null).ToString("dd-MM-yyyy");
                    EersteToelating.Text = DateTime.ParseExact(vehicleInfo.datum_eerste_toelating, "yyyyMMdd", null).ToString("dd-MM-yyyy");
                    APK.Text = DateTime.ParseExact(vehicleInfo.vervaldatum_apk, "yyyyMMdd", null).ToString("dd-MM-yyyy");

                    // Special values
                    Verzekering.Text = vehicleInfo.wam_verzekerd;
                    Cilinder.Text = vehicleInfo.cilinderinhoud + " CC";
                }
                else
                {
                    // Sends error and send back to homepage
                    await DisplayAlert("Verkeerde Kenteken", "Invalide Kenteken Plaat", "OK");
                    await Navigation.PushAsync(new MainPage());
                }
            }

            /// <summary>
            /// LoadImage
            /// Downloads image 
            /// After that changes it to JPEG and sets the source of the xaml
            /// </summary>
            /// <returns></returns>

            async void LoadImage(string imglink)
            {
                Uri urilink = new Uri(imglink);

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        // Download the image as a byte array
                        byte[] imageData = await client.GetByteArrayAsync(urilink);

                        // Convert JFIF to JPEG
                        byte[] jpegData = imageData;

                        // Create a Maui.Graphics.Image from the JPEG data
                        StreamImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(jpegData)) as StreamImageSource;

                        // Set the Image source
                        AutoFoto.Source = imageSource;
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., network errors, invalid image format, etc.)
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }


            /// <summary>Gets the API response.</summary>
            /// <param name="apiUrl">The API URL.</param>
            /// <returns>
            ///   <br />
            /// </returns>
            async Task<string> GetApiResponse(string apiUrl)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Make a GET request to the API
                        HttpResponseMessage response = await client.GetAsync(apiUrl);

                        // Check if the request was successful
                        if (response.IsSuccessStatusCode)
                        {
                            // Read and return the response content as a string
                            return await response.Content.ReadAsStringAsync();
                        }
                        else
                        {
                            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                            return null;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        return null;
                    }
                }
            }
        }
    } }
