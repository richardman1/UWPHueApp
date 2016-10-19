using HueAppRichard.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using HueAppRichard.ViewModel;
using System.Collections.ObjectModel;
using Windows.Storage;
using HueAppRichard.View;
using System.Linq;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HueAppRichard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static ApplicationData APP_DATA = ApplicationData.Current;
        public static ApplicationDataContainer LOCAL_SETTINGS = APP_DATA.LocalSettings;

        private HueHttpClient httpClient = new HueHttpClient();


        private ObservableCollection<HueLight> _lightsViewModel = HueAppViewModel.getLights();
        private ObservableCollection<HueLight> _groupsViewModel = HueAppViewModel.getGroups();
        public static bool isGroup = false;
        private ObservableCollection<HueLight> _filteredLightsViewModel = new ObservableCollection<HueLight>();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void AllRedButton_Click(object sender, RoutedEventArgs e)
        {
            await httpClient.AllLightsRed();
        }

        public ObservableCollection<HueLight> LightsViewModel
        {
            get { return this._lightsViewModel; }
        }

        private void hueListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(LightsDetailPage), e.ClickedItem);
        }


        public static void SetSettings(string ip, int port, string username)
        {
            MainPage.LOCAL_SETTINGS.Values["ip"] = ip;
            MainPage.LOCAL_SETTINGS.Values["username"] = username;
        }

        public static void RetrieveSettings(out string ip, out string username)
        {
            string tmpIp = MainPage.LOCAL_SETTINGS.Values["ip"] as string;
            string tmpUsername = MainPage.LOCAL_SETTINGS.Values["username"] as string;

            if (string.IsNullOrEmpty(tmpIp))
            {
                tmpIp = "192.168.1.179";
            }
            if (string.IsNullOrEmpty(tmpUsername))
            {
                tmpUsername = "1492b31c3af0d62f84eb4f438b041a7";
            }

            ip = tmpIp;
            username = tmpUsername;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
        }

        private void showAllOn_Click(object sender, RoutedEventArgs e)
        {
            this._filteredLightsViewModel = new ObservableCollection<HueLight>();
            this.DataContext = _lightsViewModel.Where(c => c.isOn == true);
        }

        private void showAllOff_Click(object sender, RoutedEventArgs e)
        {
            this._filteredLightsViewModel = new ObservableCollection<HueLight>();
            this.DataContext = _lightsViewModel.Where(c => c.isOn == false);
        }

        private void undo_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = this._lightsViewModel;
        }

        private void showGroups_Click(object sender, RoutedEventArgs e)
        {
            if (isGroup)
            {
                isGroup = false;
                this.DataContext = this._lightsViewModel;
            }
            else
            {
                isGroup = true;
                this.DataContext = this._groupsViewModel;
            }

        }
    }
}
