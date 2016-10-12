using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Web.Http;

namespace HueAppRichard.Model
{
    public class HueHttpClient
    {
        public HueHttpClient()
        {

        }

        private ObservableCollection<HueLight> ParseJson(string jsonResponse)
        {
            ObservableCollection<HueLight> lights = new ObservableCollection<HueLight>();
            JsonObject jsonObject;

            bool parseOk = JsonObject.TryParse(jsonResponse, out jsonObject);
            if (!parseOk)
            {
                System.Diagnostics.Debug.WriteLine(jsonResponse);
                return null;
            }

            foreach (string lightId in jsonObject.Keys)
            {
                JsonObject lightToAdd;
                HueLight l = null;

                try
                {
                    lightToAdd = jsonObject.GetNamedObject(lightId, null);
                    JsonObject lightState = lightToAdd.GetNamedObject("state", null);
                    if (lightState != null)
                    {
                        l = new HueLight(
                            lightId,
                            lightToAdd.GetNamedString("name", string.Empty),
                            lightState.GetNamedBoolean("on", false),
                            Convert.ToInt32(lightState.GetNamedNumber("sat", 255)),
                            Convert.ToInt32(lightState.GetNamedNumber("bri", 255)),
                            Convert.ToInt32(lightState.GetNamedNumber("hue", 4000)),
                            lightToAdd.GetNamedString("type", "Light"),
                            lightState.GetNamedString("effect")
                            );
                    }
                    lights.Add(l);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
            return lights;
        }

        public async Task<string> AllLightsRed()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(5000);

            try
            {
                HttpClient client = new HttpClient();
                HttpStringContent content
                    = new HttpStringContent
                          ($"{{ \"on\": true, \"hue\": 4000, \"sat\": 254, \"bri\": 254 }}",
                            Windows.Storage.Streams.UnicodeEncoding.Utf8,
                            "application/json");

                string ip, username;
                //int port;
                ip = "192.168.1.179";
                username = "1492b31c3af0d62f84eb4f438b041a7";

                Uri uriLampState = new Uri($"http://{ip}/api/{username}/groups/0/action");
                HttpResponseMessage response = await client.PutAsync(uriLampState, content).AsTask(cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine(jsonResponse);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return string.Empty;
            }

        }

        public async Task<HueLight> retrieveLightStatus(string id)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(5000);

            try
            {
                HttpClient client = new HttpClient();

                string ip, username;
                //int port;

                ip = "192.168.1.179";
                username = "1492b31c3af0d62f84eb4f438b041a7";
                //port = 8000;

                Uri uriAllLightInfo = new Uri($"http://{ip}/api/{username}/lights/4");

                HttpResponseMessage response = await client.GetAsync(uriAllLightInfo).AsTask(cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                ObservableCollection<HueLight> returnedLights = ParseJson(jsonResponse);

                HueLight correctLight = returnedLights.First();

                return correctLight;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<string> updateLight(HueLight hueLight)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(5000);

            try
            {
                HttpClient client = new HttpClient();
                HttpStringContent content
                    = new HttpStringContent
                          ($"{{ \"on\": {hueLight.isOn}, \"hue\": {hueLight.hue}, \"sat\": {hueLight.saturation}, \"bri\": {hueLight.brightness}, \"effect\": \"{hueLight.effect}\" }}",
                            Windows.Storage.Streams.UnicodeEncoding.Utf8,
                            "application/json");

                string ip, username;
                //ip = "192.168.1.179";
                //username = "1492b31c3af0d62f84eb4f438b041a7";
                //port = 8000;
                ip = "localhost:8000";
                username = "cae8e350c00fa850fa8751362a42b1f";

                Uri uriLampState = new Uri($"http://{ip}/api/{username}/lights/{hueLight.id}/state");
                HttpResponseMessage response = await client.PutAsync(uriLampState, content).AsTask(cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }
                
                string jsonResponse = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine(jsonResponse);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return string.Empty;
            }

        }

        public async Task<ObservableCollection<HueLight>> retrieveLights()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(500000);

            try
            {
                HttpClient client = new HttpClient();

                string ip, username;
                //int port;

                //ip = "192.168.1.179";
                //username = "1492b31c3af0d62f84eb4f438b041a7";
                //port = 8000;
                ip = "localhost:8000";
                username = "cae8e350c00fa850fa8751362a42b1f";

                Uri uriAllLightInfo = new Uri($"http://{ip}/api/{username}/lights/");

                HttpResponseMessage response = await client.GetAsync(uriAllLightInfo).AsTask(cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                ObservableCollection<HueLight> returnedLights = ParseJson(jsonResponse);

                return returnedLights;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
