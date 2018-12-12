using Xamarin.Forms;
using static POC.Message;

namespace POC
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<AlertMessage>(this, AlertMessage.ID, (msg) =>
            {
                DisplayAlert(msg.Title, msg.Message, "OK");
            });
        }
    }
}
