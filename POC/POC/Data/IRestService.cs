using ScanbotSDK.Xamarin.Forms;
using System.Threading.Tasks;

namespace POC.Data
{
    public interface IRestService
	{
        Task Save(IScannedPage page);
	}
}