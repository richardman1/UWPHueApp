using HueAppRichard.Model;
using HueAppRichard.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HueAppRichard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LightsDetailPage : Page
    {
        private HueLight hueLight;

        public LightsDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.hueLight = e.Parameter as HueLight;
            this.DataContext = this.hueLight;
        }

        private async void isOn_Toggled(object sender, RoutedEventArgs e)
        {
            this.hueLight.isOn = isOn.IsOn;
            await HueAppViewModel.updateLight(this.hueLight);
        }

        private async void colorloop_Toggled(object sender, RoutedEventArgs e)
        {
            this.hueLight.effect = colorloop.IsOn ? "colorloop" : "none";
            await HueAppViewModel.updateLight(this.hueLight);
        }

        private async void hueSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            this.hueLight.hue = Convert.ToInt32(hueSlider.Value);
            this.hueLight.effect = "none";
            await HueAppViewModel.updateLight(hueLight);
        }

        private async void saturationSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            this.hueLight.saturation = Convert.ToInt32(saturationSlider.Value);
            this.hueLight.effect = "none";
            await HueAppViewModel.updateLight(hueLight);
        }

        private async void brightnessSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            this.hueLight.brightness = Convert.ToInt32(brightnessSlider.Value);
            this.hueLight.effect = "none";
            await HueAppViewModel.updateLight(hueLight);
        }
    }
}
