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

        public bool Registrate(RegisterViewModel m)
        {
            throw new NotImplementedException();
        }

        public void UpdateObject<T>(T o) where T : UObject
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(stream, o);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string postData = sr.ReadToEnd();

                string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/" + o.Controller + "/" + o.Id;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "PUT";
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
            }
            catch (Exception e)
            {
                var error = e;
            }
        }

        public T LoadObject<T>(int billGlobalId) where T : UObject
        {
            throw new NotImplementedException();
        }

        internal List<UMember> LoadMemberList(int m_GlobalId)
        {
            throw new NotImplementedException();
        }

        internal List<UEvent> LoadEventList(int m_GroupId)
        {
            throw new NotImplementedException();
        }

        internal List<UDebt> LoadDebtList(int m_GroupId)
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

        public static void CreateObject<T>(T o) where T : UObject
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(stream, o);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string postData = sr.ReadToEnd();

                string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/" + o.Controller;
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
                }
            }
            catch (Exception e)
            {
                var error = e;
            }
        }
        public static void UpdateObject<T>(T o) where T : UObject
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(stream, o);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string postData = sr.ReadToEnd();

                string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/" + o.Controller + "/" + o.Id;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "PUT";
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
            }
            catch (Exception e)
            {
                var error = e;
            }
        }
        public static void DeleteObject<T>(T o) where T : UObject
        {
            string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/" + o.Controller + "/" + o.Id.ToString();
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "DELETE";
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string result = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (Exception er)
            {
                var erros = er;
            }
        }
        public static T UploadObject<T>(int id) where T : UObject
        {
            T item = Activator.CreateInstance(typeof(T)) as T;

            string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/" + item.Controller + "/" + id.ToString();
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "GET";
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        JsonValue o = JsonValue.Load(stream);
                        if (typeof(T) == typeof(UGroup))
                        {
                            return UploadGroup(o) as T;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var error = e;
            }

            return null;
        }
        public static UGroup UploadGroup(JsonValue o)
        {
            UGroup g = new UGroup();
            UGroup ugroup = new UGroup();
            ugroup.Id = o["Id"];
            ugroup.Name = o["Name"];

            if (o.ContainsKey("UEvents"))
            {
                ugroup.UEvents = new List<UEvent>();
                foreach (JsonValue e in o["UEvents"])
                {
                    ugroup.UEvents.Add(UploadEvent(o));
                }
            }
            if (o.ContainsKey("UMembers"))
            {
                ugroup.UMembers = new List<UMember>();
                foreach (JsonValue m in o["UMembers"])
                {

                    ugroup.UMembers.Add(UploadMember(m));
                }
            }
            if (o.ContainsKey("UDebts"))
            {
                ugroup.UDebts = new List<UDebt>();
                foreach (JsonValue d in o["UDebts"])
                {
                    
                    ugroup.UDebts.Add(UploadDebt(d));
                }
            }
            return g;
        }
        public static UEvent UploadEvent(JsonValue o)
        {
            UEvent uevent = new UEvent();
            uevent.Id = o["Id"];
            uevent.Name = o["Name"];
            uevent.UGroupId = o["UGroupId"];
            uevent.UEventTypeId = o["UEventTypeId"];

            if (o.ContainsKey("UBills"))
            {
                uevent.UBills = new List<UBill>();
                foreach (JsonValue b in o["UBills"])
                {
                    
                    uevent.UBills.Add(UploadBill(b));
                }
            }
            if (o.ContainsKey("UPayments"))
            {
                uevent.UPayments = new List<UPayment>();
                foreach (JsonValue p in o["UPayments"])
                {
                    
                    uevent.UPayments.Add(UploadPayment(p));
                }
            }
            return uevent;
        }
        public static UMember UploadMember(JsonValue o)
        {
            UMember umember = new UMember();
            umember.Id = o["Id"];
            umember.Name = o["Name"];
            umember.UGroupId = o["UGroupId"];
            return umember;
        }
        public static UDebt UploadDebt(JsonValue o)
        {
            UDebt udebt = new UDebt();
            udebt.Id = o["Id"];
            udebt.Name = o["Name"];
            udebt.Amount = o["Amount"];
            udebt.UGroupId = o["UGroupId"];
            udebt.LenderUMemberId = o["LenderUMemberId"];
            udebt.DebtorUMemberId = o["DebtorUMemberId"];
            return udebt;
        }
        public static UBill UploadBill(JsonValue o)
        {
            UBill ubill = new UBill();
            ubill.Id = o["Id"];
            ubill.UEventId = o["UEventId"];
            ubill.UMemberId = o["UMemberId"];
            return ubill;
        }
        public static UPayment UploadPayment(JsonValue o)
        {
            UPayment upayment = new UPayment();
            upayment.Id = o["Id"];
            upayment.UEventId = o["UEventId"];
            upayment.UMemberId = o["UMemberId"];
            return upayment;
        }
    }
}