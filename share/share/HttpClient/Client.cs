using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.Net;
using System.IO;
using System.Json;
using System.Runtime.Serialization.Json;

namespace share
{
    public class Client
    {
        public async void GetSynchronizeGroups()
        {
            string url = "http://192.168.1.4:2562/api/UGroupsControllerApi";
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "GET";

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        JsonValue json = await Task.Run(() => JsonObject.Load(stream));
                        foreach(var item in json)
                        {
                            JsonValue value = item as JsonValue;

                            int id = value["Id"];
                            string name = value["Name"];

                            object uevents = value["UEvents"];
                            object umembers = value["UMembers"];
                            object udebts = value["UDebts"];

                            UGroup ugroup = new UGroup();
                            ugroup.Name = name;
                            Controller.CreateGroup(ugroup);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var error = e;
            }
        }

        public void PostSynchronizeGroups(int id)
        {
            //string url = "http://192.168.1.4:2562/api/UGroupsControllerApi/PostSynch";
            string url = "http://192.168.1.4:2562/api/UGroupsControllerApi";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";

                UGroup ugroup = Controller.LoadGroupDetails(id);

                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UGroup));
                serializer.WriteObject(stream, ugroup);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string postData = sr.ReadToEnd();

                //string postData = "{\"Id\": 1, \"Name\": \"Elephants\", \"UDebts\": null, \"UEvents\": null, \"UMembers\": null}";

                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();

                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                Console.WriteLine(responseFromServer);
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (Exception e)
            {
                var error = e;
            }
        }
    }
}