using PCLStorage;
using ScanbotSDK.Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using static POC.Message;
using System;
using System.Diagnostics;
using System.IO;

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
                    // Customize colors, text resources, etc ...
                };

                try{
                    var result = await SBSDK.UI.LaunchDocumentScannerAsync(configuration);
                    if (result.Status == OperationResult.Ok)
                    {
                        Pages.Clear();
                        foreach (var page in result.Pages)
                            Pages.Add(page);

                        SelectedPage = Pages[0];

                        IFileSystem fileSystem = FileSystem.Current;

                        IFolder rootFolder = fileSystem.LocalStorage;

                        IFolder docsFolder = await rootFolder.CreateFolderAsync("Docs", CreationCollisionOption.OpenIfExists);

                        IFile scannedPage = await docsFolder.CreateFileAsync(SelectedPage.Document.Id.ToString()+".png", CreationCollisionOption.ReplaceExisting);

                        var path = Pages[0].Document.ToString().Split(':')[1];

                        using (System.IO.FileStream stream = (System.IO.FileStream)await scannedPage.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
                        {
                            stream.Write(System.IO.File.ReadAllBytes(path), 0, 100);
                        }


                      await App.AppManager.SaveTaskAsync(scannedPage.Name, scannedPage.Path);

                    }
                } catch (Exception ex) {
                    Console.Write(ex.Message);
                }
            });

            OpenDocScanningUiCommand = new Command(async () =>
            {
                if (!CheckScanbotSDKLicense()) { return; }

                var configuration = new DocumentScannerConfiguration
                {
                    CameraPreviewMode = CameraPreviewMode.FillIn,
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

                    IFile scannedPage = await docsFolder.CreateFileAsync("scannedPage.pdf", CreationCollisionOption.ReplaceExisting);

                    //await App.AppManager.SavePDFTaskAsync(fileUri);

                    await App.AppManager.SaveTaskAsync(scannedPage.Name, scannedPage.Path);

                    //await scannedPage.DeleteAsync();
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