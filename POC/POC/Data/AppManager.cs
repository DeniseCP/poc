using ScanbotSDK.Xamarin.Forms;
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

        public Task SaveTaskAsync(IScannedPage page)
        {
            return restService.Save(page);
        }

       
    }
}
