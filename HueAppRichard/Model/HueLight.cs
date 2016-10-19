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
        public int saturation { get; set; }
        public int brightness { get; set; }
        public int hue { get; set; }
        public string type { get; set; }
        public bool effect { get; set; }

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
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(propertyName)));
        }
    }

    
}
