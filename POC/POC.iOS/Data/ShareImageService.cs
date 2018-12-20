using System;
using POC.Data;
using Foundation;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using static POC.Message;
using PCLStorage;

namespace POC.iOS.Data
{
    public class ShareImageService : IShareImage
    {
        public async Task ShareImage(string imagePath)
        {
            if (imagePath != null || imagePath.Trim() != "")
            {
                try
                {
                    var currentPath = Environment.GetFolderPath(Environment.SpecialFolder.Resources);

                    bool dirExists = Directory.Exists(currentPath);

                    if (dirExists)
                    {
                        var dirs = System.IO.Path.GetDirectoryName(imagePath);

                        var imageFileName = Path.GetFileName(imagePath);

                        var filePath = System.IO.Path.Combine(dirs, imageFileName);

                        if (File.Exists(filePath))
                        {

                            IFileSystem fileSystem = FileSystem.Current;

                            IFolder rootFolder = fileSystem.LocalStorage;

                            IFolder docsFolder = await rootFolder.CreateFolderAsync("Docs", CreationCollisionOption.OpenIfExists);

                            IFile scannedPage = await docsFolder.CreateFileAsync(imageFileName, CreationCollisionOption.ReplaceExisting);

                            var file = File.OpenRead(filePath);

                            using (Stream srcStream = file)
                            {
                                Stream destStream = await scannedPage.OpenAsync(PCLStorage.FileAccess.ReadAndWrite);

                                await srcStream.CopyToAsync(destStream);
                                destStream.Dispose();

                            }
                        }

                    }

                } catch(Exception ex)
                {
                    Console.WriteLine("An Error occured {0}", ex.Message);
                }
            }
        }
    }
}
