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

namespace share
{
    public class Client
    {
        public static void Connect()
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync("http://192.168.1.4:2562/api/UGroupsControllerApi");
            var lol = response.Result;
            var y = 1 + 1;
        }
    }
}