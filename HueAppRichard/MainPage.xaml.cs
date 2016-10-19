using HueAppRichard.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using HueAppRichard.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HueAppRichard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
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
            get { return this._lightsViewModel;}
        }

        private void hueListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(LightsDetailPage), e.ClickedItem);
        }

        private void showAllOn_Click(object sender, RoutedEventArgs e)
        {
            this._filteredLightsViewModel = new ObservableCollection<HueLight>();
            var filters = _lightsViewModel.Where(c => c.isOn == true);
            foreach(HueLight light in filters)
            {
                this._filteredLightsViewModel.Add(light);
            }
            this.DataContext = this._filteredLightsViewModel;
        }

        private void showAllOff_Click(object sender, RoutedEventArgs e)
        {
            this._filteredLightsViewModel = new ObservableCollection<HueLight>();
            var filters = _lightsViewModel.Where(c => c.isOn == false);
            foreach (HueLight light in filters)
            {
                this._filteredLightsViewModel.Add(light);
            }
            this.DataContext = this._filteredLightsViewModel;
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
