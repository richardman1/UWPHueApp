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
        private int id;

        public int Id
        {
            get { return this.id; }
            set { id = value; NotifyPropertyChanged("Id"); }
        }

        public int saturation { get; set; }
        private string name;

        public string Name
        {
            get { return this.name; }
            set { name = value;  NotifyPropertyChanged("Name"); }
        }

        public int hue { get; set; }
        public int brightness { get; set; }
        private bool isOn;

        public bool IsOn
        {
            get { return this.isOn; }
            set { isOn = value; NotifyPropertyChanged("IsOn"); }
        }


        public HueLight(int id, string name, bool isOn, int saturation, int brightness, int hue)
        {
            this.Id = id;
            this.Name = name;
            this.IsOn = isOn;
            this.saturation = saturation;
            this.brightness = brightness;
            this.hue = hue;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(propertyName)));
        }
    }

    
}
