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
        private string m_URL = "http://46.101.214.70/";
        private string m_HomeURL = "http://192.168.1.4:2562/";

        private bool m_IsHome = false;

        public bool GetSynchronizeGroups(int groupId)
        {
            string url = (m_IsHome ? m_HomeURL: m_URL) + "api/UGroups/"+ groupId.ToString();
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "GET";

                //using (WebResponse response = await request.GetResponseAsync())
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        //JsonValue json = await Task.Run(() => JsonObject.Load(stream));
                        JsonValue value = JsonObject.Load(stream);

                        int id = value["Id"];
                        string name = value["Name"];

                        UGroup ugroup = new UGroup();
                        ugroup.Name = name;
                        ugroup.GlobalId = id;

                        if (value.ContainsKey("UEvents"))
                        {
                            ugroup.UEvents = new List<UEvent>();
                            JsonValue uevents = value["UEvents"];
                            foreach (JsonValue item in uevents)
                            {
                                UEvent uevent = new UEvent();

                                int sid = item["Id"];
                                string sname = item["Name"];
                                int sgroupid = item["UGroupId"];
                                int seventtypeid = item["UEventTypeId"];

                                uevent.Id = sid;
                                uevent.Name = sname;
                                uevent.UGroupId = sgroupid;
                                uevent.UEventTypeId = seventtypeid;

                                if (item.ContainsKey("UBills"))
                                {
                                    uevent.UBills = new List<UBill>();
                                    JsonValue sbills = item["UBills"];
                                    foreach (JsonValue sitem in sbills)
                                    {
                                        UBill ubill = new UBill();
                                        int ssid = sitem["Id"];
                                        int seventid = sitem["UEventId"];
                                        int smemberis = sitem["UMemberId"];

                                        ubill.UEventId = seventid;
                                        ubill.UMemberId = smemberis;

                                        uevent.UBills.Add(ubill);
                                    }
                                }

                                if (item.ContainsKey("UPayments"))
                                {
                                    uevent.UPayments = new List<UPayment>();
                                    JsonValue spayment = item["UPayments"];
                                    foreach (JsonValue sitem in spayment)
                                    {
                                        UPayment upayment = new UPayment();
                                        int ssid = sitem["Id"];
                                        int seventid = sitem["UEventId"];
                                        int smemberis = sitem["UMemberId"];

                                        upayment.UEventId = seventid;
                                        upayment.UMemberId = smemberis;

                                        uevent.UPayments.Add(upayment);
                                    }
                                }
                                ugroup.UEvents.Add(uevent);
                            }
                        }

                        if (value.ContainsKey("UMembers"))
                        {
                            ugroup.UMembers = new List<UMember>();
                            JsonValue umembers = value["UMembers"];
                            foreach (JsonValue item in umembers)
                            {
                                UMember umember = new UMember();

                                int sid = item["Id"];
                                string sname = item["Name"];
                                int sgroupid = item["UGroupId"];

                                umember.Id = sid;
                                umember.Name = sname;
                                umember.UGroupId = sgroupid;

                                ugroup.UMembers.Add(umember);
                            }
                        }

                        if (value.ContainsKey("UDebts"))
                        {
                            ugroup.UDebts = new List<UDebt>();
                            JsonValue udebts = value["UDebts"];
                            foreach (JsonValue item in udebts)
                            {
                                UDebt udebt = new UDebt();
                                int sid = item["Id"];
                                string sname = item["Name"];
                                double samount = item["Amount"];
                                int sgroupid = item["UGroupId"];
                                int slender = item["LenderUMemberId"];
                                int sdebtor = item["DebtorUMemberId"];

                                udebt.Name = sname;
                                udebt.Amount = samount;
                                udebt.UGroupId = sgroupid;
                                udebt.LenderUMemberId = slender;
                                udebt.DebtorUMemberId = sdebtor;

                                ugroup.UDebts.Add(udebt);
                            }
                        }
                        
                        return Controller.UploadGroup(ugroup);
                    }
                }
            }
            catch (Exception e)
            {
                var error = e;
            }
            return false;
        }

        public void PostSynchronizeGroups(int id)
        {
            string url = (m_IsHome ? m_HomeURL : m_URL) + "api/UGroups";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";

                UGroup ugroup = Controller.LoadGroupDetails(id, true);
                int currentID = ugroup.Id;
                ugroup.Id = ugroup.GlobalId ?? 0;

                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UGroup));
                serializer.WriteObject(stream, ugroup);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string postData = sr.ReadToEnd();

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

                int newID;
                if(Int32.TryParse(responseFromServer, out newID))
                {
                    ugroup.Id = currentID;
                    ugroup.GlobalId = newID;
                    Controller.UpdateObject(ugroup);
                }
            }
            catch (Exception e)
            {
                var error = e;
            }
        }
    }
}