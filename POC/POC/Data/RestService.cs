using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using static POC.Message;

namespace POC.Data
{
    public class RestService : IRestService
    {
        private HttpClient client;

        public RestService()
        {
            //var authData = string.Format("{0}:{1}", Constants.Username, Constants.Password);
            //var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

            HttpClientHandler handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Automatic;
            handler.UseDefaultCredentials = true;

            client = new HttpClient(handler);
            // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

        }

        public async Task Save(string fileName, string filePath)
        {
            var uri = new Uri(string.Format(Constants.RestUrl, 21864));

            try
            {
                var content = new MultipartFormDataContent();

                content.Headers.ContentType.MediaType = "multipart/form-data";

                Stream fileToSave = System.IO.File.OpenRead(filePath);

                content.Add(new StreamContent(fileToSave), "file", fileName);

                var exists = System.IO.File.Exists(filePath);

                MessagingCenter.Send(new AlertMessage { Message = exists.ToString(), Title = "Test" }, AlertMessage.ID);


                HttpResponseMessage response = null;

                response = await client.PostAsync(uri, content);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                if (response.IsSuccessStatusCode)
                {
                    MessagingCenter.Send(new AlertMessage { Message = response.StatusCode + " " + response.ReasonPhrase, Title = "Validated" }, AlertMessage.ID);
                    Debug.WriteLine("{0} successfully saved.", response.StatusCode);

                    //System.IO.File.Delete(filePath);
                }
                else
                {
                    MessagingCenter.Send(new AlertMessage { Message = response.StatusCode + " " + response.ReasonPhrase, Title = "Not Validated" }, AlertMessage.ID);

                }

                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR {0}", ex.Message);
            }
        }
    }


}
