using HueAppRichard.Helpers;
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
        string ip, username, tmpIp, tmpUsername;
        public HueHttpClient()
        {
            MainPage.RetrieveSettings(out tmpIp, out tmpUsername);
            this.ip = tmpIp;
            this.username = tmpUsername;
        }

        private ObservableCollection<HueLight> ParseJson(string jsonResponse, bool isGroup)
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
                    if (isGroup)
                    {
                        lightToAdd = jsonObject.GetNamedObject(lightId, null);
                        JsonObject lightAction = lightToAdd.GetNamedObject("action", null);
                        l = new HueLight(
                                lightId,
                                lightToAdd.GetNamedString("name", string.Empty),
                                lightAction.GetNamedBoolean("on", false),
                                Convert.ToInt32(lightAction.GetNamedNumber("sat", 255)),
                                Convert.ToInt32(lightAction.GetNamedNumber("bri", 255)),
                                Convert.ToInt32(lightAction.GetNamedNumber("hue", 4000)),
                                lightToAdd.GetNamedString("type", "Light"),
                                lightAction.GetNamedString("effect", "none") == "colorloop" ? true : false
                                );
                    }
                    else
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
                                lightState.GetNamedString("effect", "none") == "colorloop" ? true : false
                                );
                        }
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
                System.Diagnostics.Debug.WriteLine(content);

                //int port;
                //port = 8000;
                //ip = "localhost:8000";
                //username = "21ae800caaa4f2198e09b35c251be8e";

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

        public async Task<string> updateLight(HueLight hueLight, bool isGroup)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(5000);

            try
            {
                HttpClient client = new HttpClient();
                HttpStringContent content
                    = new HttpStringContent
                          ($"{{ \"on\": {hueLight.isOn.ToString().ToLower()}, \"hue\": {hueLight.hue}, \"sat\": {hueLight.saturation}, \"bri\": {hueLight.brightness}, \"effect\": \"{(hueLight.effect ? "colorloop" : "none")}\" }}",
                            Windows.Storage.Streams.UnicodeEncoding.Utf8,
                            "application/json");
                System.Diagnostics.Debug.WriteLine(content);

                //port = 8000;
                //ip = "localhost:8000";
                //username = "21ae800caaa4f2198e09b35c251be8e";
                Uri uriLampState;
                if (isGroup)
                {
                    uriLampState = new Uri($"http://{ip}/api/{username}/groups/{hueLight.id}/action");
                }
                else
                {
                    uriLampState = new Uri($"http://{ip}/api/{username}/lights/{hueLight.id}/state");
                }
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


                //int port;
                //port = 8000;
                //ip = "localhost:8000";
                //username = "21ae800caaa4f2198e09b35c251be8e";

                Uri uriAllLightInfo = new Uri($"http://{ip}/api/{username}/lights/");

                HttpResponseMessage response = await client.GetAsync(uriAllLightInfo).AsTask(cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                ObservableCollection<HueLight> returnedLights = ParseJson(jsonResponse, false);
                returnedLights.Sort<HueLight>((x, y) => x.id.CompareTo(y.id));

                return returnedLights;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<ObservableCollection<HueLight>> retrieveGroups()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(500000);

            try
            {
                HttpClient client = new HttpClient();

                //int port;
                //port = 8000;
                //ip = "localhost:8000";
                //username = "21ae800caaa4f2198e09b35c251be8e";

                Uri uriAllLightInfo = new Uri($"http://{ip}/api/{username}/groups/");

                HttpResponseMessage response = await client.GetAsync(uriAllLightInfo).AsTask(cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                ObservableCollection<HueLight> returnedGroups = ParseJson(jsonResponse, true);
                returnedGroups.Sort<HueLight>((x, y) => x.id.CompareTo(y.id));

                return returnedGroups;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
