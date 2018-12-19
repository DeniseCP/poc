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

                            var originalImage = BitmapFactory.DecodeFile(filePath);

                            using (var ms = new MemoryStream())
                            {
                                originalImage.Compress(Bitmap.CompressFormat.Png, 90, ms);

                                byte[] bytes = ms.ToArray();

                                Android.Util.Log.Info("File to bytes size is {0}", bytes.Length.ToString());

                                if (bytes.Length > 0)
                                {
                                    Console.WriteLine(true);

                                    using (Stream str = await scannedPage.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
                                    {
                                        str.Write(bytes, 0, bytes.Length);
                                    }

                                    Android.Util.Log.Info("Scanned file to save is {0}", "true");
                                }
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