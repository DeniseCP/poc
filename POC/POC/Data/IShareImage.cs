using System.IO;
using System.Threading.Tasks;

namespace POC.Data
{
    public interface IShareImage
    {
        Task ShareImage(string imagePath);
    }
}
