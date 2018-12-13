using ScanbotSDK.Xamarin.Forms;
using System;
using System.Threading.Tasks;

namespace POC.Data
{
    public interface IRestService
	{
        Task Save(string fileName, string filePath);
	}
}