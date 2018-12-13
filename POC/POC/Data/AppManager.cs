using ScanbotSDK.Xamarin.Forms;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace POC.Data
{
    public class AppManager
    {
        IRestService restService;

        public AppManager(IRestService service)
        {
            restService = service;
        }

        public Task SaveTaskAsync(string fileName, string filePath)
        {
            return restService.Save(fileName, filePath);
        }

       
    }
}
