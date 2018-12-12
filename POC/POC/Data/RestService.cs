using ModernHttpClient;
using ScanbotSDK.Xamarin.Forms;
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

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (o, cert, chain, errors) => true;

            client = new HttpClient(new NativeMessageHandler(true, true));
            // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
        }

        public async Task Save(IScannedPage page)
        {
            var uri = new Uri(string.Format(Constants.RestUrl, 21864));

            try
            {
                var content = new MultipartFormDataContent();

                var path = page.Document.ToString().Split(':')[1].Trim();

                FileStream file = new FileStream(path, FileMode.Open);

                content.Add(new StreamContent(file), "file", file.Name);

                HttpResponseMessage response = null;

                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    MessagingCenter.Send(new AlertMessage { Message = "Ok", Title = "Validated" }, AlertMessage.ID);
                    Debug.WriteLine("{0} successfully saved.", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR {0}", ex.Message);
                MessagingCenter.Send(new AlertMessage { Message = "400" + ex.Message, Title = "Not Validated" }, AlertMessage.ID);
            }
        }
    }


}
