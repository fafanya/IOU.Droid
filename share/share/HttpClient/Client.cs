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
        private bool m_IsHome = !false;
        private string m_HostURL = "http://46.101.214.70/";
        private string m_HomeURL = "http://192.168.1.4:2562/";
        private string m_WorkURL = "http://192.168.0.73:2562/";

        public bool ImportGroup(int id)
        {
            string url = (m_IsHome ? m_HomeURL: m_HostURL) + "api/UGroups/"+ id.ToString();
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "GET";
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        JsonValue g = JsonValue.Load(stream);

                        UGroup ugroup = new UGroup();
                        ugroup.Id = g["Id"];
                        ugroup.Name = g["Name"];

                        if (g.ContainsKey("UEvents"))
                        {
                            ugroup.UEvents = new List<UEvent>();
                            foreach (JsonValue e in g["UEvents"])
                            {
                                UEvent uevent = new UEvent();
                                uevent.Id = e["Id"];
                                uevent.Name = e["Name"];
                                uevent.UGroupId = e["UGroupId"];
                                uevent.UEventTypeId = e["UEventTypeId"];

                                if (e.ContainsKey("UBills"))
                                {
                                    uevent.UBills = new List<UBill>();
                                    foreach (JsonValue b in e["UBills"])
                                    {
                                        UBill ubill = new UBill();
                                        ubill.Id = b["Id"];
                                        ubill.UEventId = b["UEventId"];
                                        ubill.UMemberId = b["UMemberId"];
                                        uevent.UBills.Add(ubill);
                                    }
                                }
                                if (e.ContainsKey("UPayments"))
                                {
                                    uevent.UPayments = new List<UPayment>();
                                    foreach (JsonValue p in e["UPayments"])
                                    {
                                        UPayment upayment = new UPayment();
                                        upayment.Id = p["Id"];
                                        upayment.UEventId = p["UEventId"];
                                        upayment.UMemberId = p["UMemberId"];
                                        uevent.UPayments.Add(upayment);
                                    }
                                }
                                ugroup.UEvents.Add(uevent);
                            }
                        }

                        if (g.ContainsKey("UMembers"))
                        {
                            ugroup.UMembers = new List<UMember>();
                            foreach (JsonValue m in g["UMembers"])
                            {
                                UMember umember = new UMember();
                                umember.Id = m["Id"];
                                umember.Name = m["Name"];
                                umember.UGroupId = m["UGroupId"];
                                ugroup.UMembers.Add(umember);
                            }
                        }

                        if (g.ContainsKey("UDebts"))
                        {
                            ugroup.UDebts = new List<UDebt>();
                            foreach (JsonValue d in g["UDebts"])
                            {
                                UDebt udebt = new UDebt();
                                udebt.Id = d["Id"];
                                udebt.Name = d["Name"];
                                udebt.Amount = d["Amount"];
                                udebt.UGroupId = d["UGroupId"];
                                udebt.LenderUMemberId = d["LenderUMemberId"];
                                udebt.DebtorUMemberId = d["DebtorUMemberId"];
                                ugroup.UDebts.Add(udebt);
                            }
                        }
                        return Controller.UploadNewGroup(ugroup);
                    }
                }
            }
            catch (Exception e)
            {
                var error = e;
            }
            return false;
        }

        public void UpdateObject<T>(T m_Bill) where T : UObject
        {
            throw new NotImplementedException();
        }

        public T LoadObject<T>(int billGlobalId) where T : UObject
        {
            throw new NotImplementedException();
        }

        internal List<UEvent> LoadEventList(int m_GroupId)
        {
            throw new NotImplementedException();
        }

        internal List<UMember> LoadMemberList(int m_GlobalId)
        {
            throw new NotImplementedException();
        }

        internal List<UPayment> LoadPaymentList(int m_GlobalId)
        {
            throw new NotImplementedException();
        }

        internal List<UBill> LoadBillList(int m_GlobalId)
        {
            throw new NotImplementedException();
        }

        internal List<UDebt> LoadDebtList(int m_GroupId)
        {
            throw new NotImplementedException();
        }

        public bool ExportGroup(int localId)
        {
            try
            {
                UGroup ugroup = Controller.LoadFullGroupDetails(localId);

                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UGroup));
                serializer.WriteObject(stream, ugroup);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string postData = sr.ReadToEnd();

                string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/UGroups";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                
                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string result = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();

                int id;
                if(int.TryParse(result, out id))
                {
                    ugroup.Id = id;
                    Controller.UpdateObject(ugroup);
                    return true;
                }
            }
            catch (Exception e)
            {
                var error = e;
            }
            return false;
        }

        public bool PostGroup(UGroup g)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UGroup));
                serializer.WriteObject(stream, g);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string postData = sr.ReadToEnd();

                string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/UGroups";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string result = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();

                int id;
                if (int.TryParse(result, out id))
                {
                    g.Id = id;
                    Controller.UpdateObject(g);
                    return true;
                }
            }
            catch (Exception e)
            {
                var error = e;
            }
            return false;
        }
        public bool PostEvent(UEvent e)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UEvent));
                serializer.WriteObject(stream, e);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string postData = sr.ReadToEnd();

                string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/UEvents";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string result = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();

                int id;
                if (int.TryParse(result, out id))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                var error = ex;
            }
            return false;
        }
        public bool PostMember(UMember m)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UMember));
                serializer.WriteObject(stream, m);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string postData = sr.ReadToEnd();

                string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/UMembers";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string result = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();

                int id;
                if (int.TryParse(result, out id))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                var error = e;
            }
            return false;
        }
        public bool PostDebt(UDebt d)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UDebt));
                serializer.WriteObject(stream, d);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string postData = sr.ReadToEnd();

                string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/UDebts";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string result = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();

                int id;
                if (int.TryParse(result, out id))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                var error = e;
            }
            return false;
        }
        public bool PostBill(UBill b)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UBill));
                serializer.WriteObject(stream, b);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string postData = sr.ReadToEnd();

                string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/UBills";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string result = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();

                int id;
                if (int.TryParse(result, out id))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                var error = e;
            }
            return false;
        }
        public bool PostPayment(UPayment p)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UPayment));
                serializer.WriteObject(stream, p);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string postData = sr.ReadToEnd();

                string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/UPayments";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string result = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();

                int id;
                if (int.TryParse(result, out id))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                var error = e;
            }
            return false;
        }
    }
}