using HueAppRichard.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueAppRichard.ViewModel
{
     class HueAppViewModel
    {
        private static ObservableCollection<HueLight> huelights = new ObservableCollection<HueLight>();

        private static HueHttpClient httpClient = new HueHttpClient();

        public ObservableCollection<HueLight> HueLights
        {
            get { return huelights; }
        }

        public static ObservableCollection<HueLight> getLights()
        {
            if (huelights.Count == 0)
            {
                Task.Run(() => AddLights()).Wait();
            }
            return huelights;
        }

        public static async Task AddLights()
        {
            ObservableCollection<HueLight> lights = await httpClient.retrieveLights();
            foreach (HueLight l in lights)
            {
                huelights.Add(l);
            }
        }
    }
}
