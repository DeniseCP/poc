using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using PCLStorage;
using POC.Data;
using ScanbotSDK.Xamarin.Forms;
using Xamarin.Forms;
using static POC.Message;

namespace POC
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand OpenIDScanningUiCommand { get; }
        public ICommand OpenDocScanningUiCommand { get; }


        IScannedPage _selectedPage;
        public IScannedPage SelectedPage
        {
            get => _selectedPage;
            private set
            {
                _selectedPage = value;
                NotifyPropertyChanged("SelectedPage");
            }
        }

        ObservableCollection<IScannedPage> _pages = new ObservableCollection<IScannedPage>();
        public ObservableCollection<IScannedPage> Pages
        {
            get => _pages;
            private set
            {
                _pages = value;
                NotifyPropertyChanged("Pages");
            }
        }

        public MainPageViewModel()
        {
            OpenIDScanningUiCommand = new Command(async () =>
            {
                if (!CheckScanbotSDKLicense()) { return; }

                var configuration = new DocumentScannerConfiguration
                {
                    CameraPreviewMode = CameraPreviewMode.FillIn,
                    MultiPageButtonHidden = true,
                    FlashButtonHidden = true
                    // Customize colors, text resources, etc ...
                };

                try
                {
                    var result = await SBSDK.UI.LaunchDocumentScannerAsync(configuration);
                    if (result.Status == OperationResult.Ok)
                    {
                        Pages.Clear();
                        foreach (var page in result.Pages)
                            Pages.Add(page);

                        SelectedPage = Pages[0];

                        var path = Pages[0].Document.ToString().Split(':')[1];

                        // await DependencyService.Get<IShareImage>().ShareImage(path);

                        var imageName = Path.GetFileName(path);

                        var imagePath = "";

                        if (Device.RuntimePlatform == Device.iOS)
                        {
                            imagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Resources), "Application Support", "net.doo.ScanbotSDK",
                            "SBSDK_ImageStorage_Default", "PageFileStorage", "PNG", "documents", imageName);

                        } else 
                        {
                            imagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Docs", imageName);

                        }
                        await App.AppManager.SaveTaskAsync(imageName, imagePath);

                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            });

            OpenDocScanningUiCommand = new Command(async () =>
            {
                if (!CheckScanbotSDKLicense()) { return; }

                var configuration = new DocumentScannerConfiguration
                {
                    CameraPreviewMode = CameraPreviewMode.FillIn,
                    FlashButtonHidden = true,
                    MultiPageButtonHidden = true
                    // Customize colors, text resources, etc ...
                };

                var result = await SBSDK.UI.LaunchDocumentScannerAsync(configuration);
                if (result.Status == OperationResult.Ok)
                {
                    Pages.Clear();
                    foreach (var page in result.Pages)
                        Pages.Add(page);

                    SelectedPage = Pages[0];

                    var fileUri = await SBSDK.Operations.CreatePdfAsync(DocumentSources);

                    IFileSystem fileSystem = FileSystem.Current;

                    IFolder rootFolder = fileSystem.LocalStorage;

                    IFolder docsFolder = await rootFolder.CreateFolderAsync("Docs", CreationCollisionOption.OpenIfExists);

                    if (File.Exists(fileUri.AbsolutePath))
                    {
                        var fileName = Path.GetFileName(fileUri.AbsolutePath);

                        IFile scannedPage = await docsFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                        byte[] bytes = File.ReadAllBytes(fileUri.AbsolutePath);

                        if (bytes.Length > 0)
                        {
                            Console.WriteLine(true);

                            using (Stream str = await scannedPage.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
                            {
                                str.Write(bytes, 0, bytes.Length);
                            }
                        }
                        await App.AppManager.SaveTaskAsync(scannedPage.Name, scannedPage.Path);
                    }
                }
            });
        }

        IEnumerable<ImageSource> DocumentSources => Pages.Select(p => p.Document).Where(image => image != null);

        void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        bool CheckScanbotSDKLicense()
        {
            if (SBSDK.Operations.IsLicenseValid)
            {
                return true;
            }

            var msg = new AlertMessage
            {
                Title = "Info",
                Message = "Scanbot SDK trial license has expired."
            };
            MessagingCenter.Send(msg, AlertMessage.ID);
            return false;
        }

        bool CheckPageSelected()
        {
            if (SelectedPage != null)
            {
                return true;
            }

            var msg = new AlertMessage
            {
                Title = "Info",
                Message = "Selected a page first."
            };
            MessagingCenter.Send(msg, AlertMessage.ID);
            return false;
        }

        bool CheckDocumentSelected()
        {
            if (SelectedPage != null && SelectedPage.Document != null)
            {
                return true;
            }

            var msg = new AlertMessage
            {
                Title = "Info",
                Message = "Selected a page with a detected document."
            };
            MessagingCenter.Send(msg, AlertMessage.ID);
            return false;
        }
    }
}