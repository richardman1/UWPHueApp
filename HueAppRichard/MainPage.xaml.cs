using HueAppRichard.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using Windows.Data.Json;
using HueAppRichard.ViewModel;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HueAppRichard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private HueHttpClient httpClient;

        private ObservableCollection<HueLight> _lightsViewModel = HueAppViewModel.getLights();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void RetrieveButton_Click(object sender, RoutedEventArgs e)
        {
            await httpClient.retrieveLights();
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
    }
}
