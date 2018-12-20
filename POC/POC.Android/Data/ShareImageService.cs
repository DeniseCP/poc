using Android.Graphics;
using PCLStorage;
using POC.Data;
using POC.Droid;
using System;
using System.IO;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(ShareImageService))]
namespace POC.Droid
{
    public class ShareImageService : Java.Lang.Object, IShareImage
    {
        public async Task ShareImage(string imagePath)
        {
            if (imagePath != null || imagePath.Trim() != "")
            {
                try
                {
                    var currentPath = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;

                    bool dirExists = Directory.Exists(currentPath);

                    if (dirExists)
                    {
                        var dirs = System.IO.Path.GetDirectoryName(imagePath).Split('/');
                        var imageDirectoryName = dirs[dirs.Length - 1];
                        var scannedPics = dirs[dirs.Length - 2];
                        var imageFileName = System.IO.Path.GetFileName(imagePath);

                        Android.Util.Log.Info("Image Path {0}", imageDirectoryName);

                        Android.Util.Log.Info("Image Name {0}", imageFileName);

                        var filePath = System.IO.Path.Combine(currentPath, scannedPics, imageDirectoryName, imageFileName);

                        if (File.Exists(filePath))
                        {
                            Android.Util.Log.Info("File name {0} exists", imageFileName);

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
                }
                catch (Exception ex)
                {
                    Android.Util.Log.Debug("Error {0}", ex.Message);
                }
            }



        }
    }
}