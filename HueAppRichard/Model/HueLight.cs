using HueAppRichard.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueAppRichard.Model
{
    public class HueLight : INotifyPropertyChanged
    {
        public string name { get; set; }
        public string id { get; set; }
        public bool isOn { get; set; }
        public double saturation { get; set; }
        public double brightness { get; set; }
        public double hue { get; set; }
        public string type { get; set; }
        public bool effect { get; set; }
        public string rgbcolor { get; set; }

        public HueLight(string id, string name, bool isOn, int saturation, int brightness, int hue, string type, bool effect)
        {
            this.id = id;
            this.name = name;
            this.isOn = isOn;
            this.saturation = saturation;
            this.brightness = brightness;
            this.hue = hue;
            this.type = type;
            this.effect = effect;
            convertHue();
        }

        private void convertHue()
        {
            this.rgbcolor = ColorUtil.HsvToRgb(hue, saturation, brightness).ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(propertyName)));
        }
    }

    
}
