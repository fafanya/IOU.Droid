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

namespace share
{
    public class Client
    {
        public async void GetSynchronizeGroups()
        {
            string url = "http://192.168.0.73:2562/api/UGroupsControllerApi";
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
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var error = e;
            }
        }

        public void PostSynchronizeGroups()
        {
            string url = "http://192.168.0.73:2562/api/UGroupsControllerApi/PostSynch";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Accept = "application/x-www-form-urlencoded";
                request.Method = "POST";
                string postData = "={\"employees\":[{ \"firstName\":\"Peter\", \"lastName\":\"Jones\"}]}";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
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