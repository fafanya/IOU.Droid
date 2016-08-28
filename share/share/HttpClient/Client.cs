using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.IO;
using System.Json;
using System.Runtime.Serialization.Json;

namespace share
{
    public class Client
    {
        private static bool m_IsHome = false;
        private static string m_HostURL = "http://46.101.214.70/";
        private static string m_HomeURL = "http://192.168.1.4:2562/";//"http://192.168.0.73:2562/";

        public static bool Registrate(RegisterViewModel m)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RegisterViewModel));
                serializer.WriteObject(stream, m);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string postData = sr.ReadToEnd();

                string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/AccountApi/Register";
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
                    Server.CreateUser(null, null);
                    return true;
                }
            }
            catch (Exception e)
            {
                var error = e;
            }
            return false;
        }

        public static bool Login(LoginViewModel m)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(LoginViewModel));
                serializer.WriteObject(stream, m);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string postData = sr.ReadToEnd();

                string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/AccountApi/Login";
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


                UUser uUser = Deserialize<UUser>(result);

                Server.CreateUser(uUser.id, uUser.email);
                return true;
            }
            catch (Exception e)
            {
                var error = e;
            }
            return false;
        }
        public static void Logout()
        {
            try
            {
                Server.Logout();
            }
            catch
            {

            }
        }

        public static bool LoginInGroup(LoginGroupViewModel m)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(LoginGroupViewModel));
                serializer.WriteObject(stream, m);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string postData = sr.ReadToEnd();

                string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/UGroupsApi/Login";
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

                bool isSuccess;
                if (bool.TryParse(result, out isSuccess))
                {
                    return isSuccess;
                }

                return false;
            }
            catch (Exception e)
            {
                var error = e;
            }
            return false;
        }
        public static bool SelectMember(int memberId)
        {
            try
            {
                SelectViewModel m = new SelectViewModel();
                m.UMemberId = memberId;
                m.UUserId = Server.GetCurrentUserId();

                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SelectViewModel));
                serializer.WriteObject(stream, m);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string postData = sr.ReadToEnd();

                string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/UMembersApi/Select";
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

                bool isSuccess;
                if (bool.TryParse(result, out isSuccess))
                {
                    return isSuccess;
                }

                return false;
            }
            catch (Exception e)
            {
                var error = e;
            }
            return false;
        }

        public static List<UMember> LoadMemberList(int groupId)
        {
            List<UMember> items = new List<UMember>();
            string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/UMembersApi/ByGroup/" + groupId;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "GET";
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        JsonValue o = JsonValue.Load(stream);
                        foreach (JsonValue g in o)
                        {
                            UMember m = UploadMember(g);
                            items.Add(m);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var error = e;
            }
            return items;
        }
        public static List<UEvent> LoadEventList(int groupId)
        {
            List<UEvent> items = new List<UEvent>();
            string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/UEventsApi/ByGroup/" + groupId;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "GET";
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        JsonValue o = JsonValue.Load(stream);
                        foreach (JsonValue g in o)
                        {
                            UEvent e = UploadEvent(g);
                            items.Add(e);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var error = e;
            }
            return items;
        }
        public static List<UDebt> LoadDebtList(int groupId)
        {
            List<UDebt> items = new List<UDebt>();
            string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/UDebtsApi/ByGroup/" + groupId;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "GET";
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        JsonValue o = JsonValue.Load(stream);
                        foreach (JsonValue g in o)
                        {
                            UDebt d = UploadDebt(g);
                            items.Add(d);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var error = e;
            }
            return items;
        }
        public static List<UPayment> LoadPaymentList(int eventId)
        {
            List<UPayment> items = new List<UPayment>();
            string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/UPaymentsApi/ByEvent/" + eventId;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "GET";
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        JsonValue o = JsonValue.Load(stream);
                        foreach (JsonValue g in o)
                        {
                            UPayment p = UploadPayment(g);
                            items.Add(p);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var error = e;
            }
            return items;
        }
        public static List<UBill> LoadBillList(int eventId)
        {
            List<UBill> items = new List<UBill>();
            string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/UBillsApi/ByEvent/" + eventId;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "GET";
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        JsonValue o = JsonValue.Load(stream);
                        foreach (JsonValue g in o)
                        {
                            UBill b = UploadBill(g);
                            items.Add(b);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var error = e;
            }
            return items;
        }
        public static List<UGroup> LoadGroupList()
        {
            List<UGroup> items = new List<UGroup>();
            try
            {
                string userId = Server.GetCurrentUserId();
                if (userId == null)
                    return items;

                string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/UGroupsApi/ByUser/" + userId;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "GET";
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        JsonValue o = JsonValue.Load(stream);
                        foreach(JsonValue g in o)
                        {
                            UGroup ng = UploadGroup(g);
                            items.Add(ng);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var error = e;
            }

            return items;
        }
        public static List<UTotal> LoadTotalListByGroup(int groupId)
        {
            List<UTotal> items = new List<UTotal>();
            string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/UTotalsApi/ByGroup/" + groupId;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "GET";
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        JsonValue o = JsonValue.Load(stream);
                        foreach (JsonValue g in o)
                        {
                            UTotal t = UploadTotal(g);
                            items.Add(t);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var error = e;
            }
            return items;
        }
        public static List<UTotal> LoadTotalListByEvent(int eventId)
        {
            List<UTotal> items = new List<UTotal>();
            string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/UTotalsApi/ByEvent/" + eventId;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "GET";
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        JsonValue o = JsonValue.Load(stream);
                        foreach (JsonValue g in o)
                        {
                            UTotal t = UploadTotal(g);
                            items.Add(t);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var error = e;
            }
            return items;
        }

        public bool ExportGroup(int id)
        {
            try
            {
                UGroup ugroup = Server.LoadFullGroupDetails(id);
                ugroup.UUserId = Server.GetCurrentUserId();
                if (string.IsNullOrWhiteSpace(ugroup.UUserId))
                    return false;

                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UGroup));
                serializer.WriteObject(stream, ugroup);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string postData = sr.ReadToEnd();

                string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/UGroupsApi/Export";
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
            }
            catch (Exception e)
            {
                var error = e;
            }
            return true;
        }
        public bool ImportGroup(int id)
        {
            string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/UGroupsApi/Import/" + id.ToString();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "GET";
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        JsonValue o = JsonValue.Load(stream);
                        UGroup g = UploadGroup(o);
                        Server.UploadGroup(g);
                        return true;
                    }
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
        public static T LoadObjectDetails<T>(int id) where T : UObject
        {
            T item = Activator.CreateInstance(typeof(T)) as T;

            string url = (m_IsHome ? m_HomeURL : m_HostURL) + "api/" + item.Controller + "/" + id.ToString();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
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
                        if (typeof(T) == typeof(UEvent))
                        {
                            return UploadEvent(o) as T;
                        }
                        if (typeof(T) == typeof(UMember))
                        {
                            return UploadMember(o) as T;
                        }
                        if (typeof(T) == typeof(UDebt))
                        {
                            return UploadDebt(o) as T;
                        }
                        if (typeof(T) == typeof(UPayment))
                        {
                            return UploadPayment(o) as T;
                        }
                        if (typeof(T) == typeof(UBill))
                        {
                            return UploadBill(o) as T;
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

        private static UGroup UploadGroup(JsonValue o)
        {
            UGroup ugroup = new UGroup();
            ugroup.Id = o["id"];
            ugroup.Name = o["name"];
            ugroup.Password = o["password"];
            ugroup.UUserId = o["uUserId"];

            if (o.ContainsKey("uEvents") && o["uEvents"] != null)
            {
                ugroup.UEvents = new List<UEvent>();
                foreach (JsonValue e in o["uEvents"])
                {
                    ugroup.UEvents.Add(UploadEvent(e));
                }
            }
            if (o.ContainsKey("uMembers") && o["uMembers"] != null)
            {
                ugroup.UMembers = new List<UMember>();
                foreach (JsonValue m in o["uMembers"])
                {
                    ugroup.UMembers.Add(UploadMember(m));
                }
            }
            if (o.ContainsKey("uDebts") && o["uDebts"] != null)
            {
                ugroup.UDebts = new List<UDebt>();
                foreach (JsonValue d in o["uDebts"])
                {
                    ugroup.UDebts.Add(UploadDebt(d));
                }
            }
            return ugroup;
        }
        private static UEvent UploadEvent(JsonValue o)
        {
            UEvent uevent = new UEvent();
            uevent.Id = o["id"];
            uevent.Name = o["name"];
            uevent.UGroupId = o["uGroupId"];
            uevent.UEventTypeId = o["uEventTypeId"];
            uevent.ReadOnlyFields["EventTypeName"] = o["eventTypeName"];

            if (o.ContainsKey("uBills") && o["uBills"] != null)
            {
                uevent.UBills = new List<UBill>();
                foreach (JsonValue b in o["uBills"])
                {
                    uevent.UBills.Add(UploadBill(b));
                }
            }
            if (o.ContainsKey("uPayments") && o["uPayments"] != null)
            {
                uevent.UPayments = new List<UPayment>();
                foreach (JsonValue p in o["uPayments"])
                {
                    
                    uevent.UPayments.Add(UploadPayment(p));
                }
            }
            return uevent;
        }
        private static UMember UploadMember(JsonValue o)
        {
            UMember umember = new UMember();
            umember.Id = o["id"];
            umember.Name = o["name"];
            umember.UGroupId = o["uGroupId"];
            return umember;
        }
        private static UDebt UploadDebt(JsonValue o)
        {
            UDebt udebt = new UDebt();
            udebt.Id = o["id"];
            udebt.Name = o["name"];
            udebt.Amount = o["amount"];
            udebt.UGroupId = o["uGroupId"];
            udebt.LenderId = o["lenderId"];
            udebt.DebtorId = o["debtorId"];

            udebt.ReadOnlyFields["DebtorName"] = o["debtorName"];
            udebt.ReadOnlyFields["LenderName"] = o["lenderName"];
            return udebt;
        }
        private static UBill UploadBill(JsonValue o)
        {
            UBill ubill = new UBill();
            ubill.Id = o["id"];
            ubill.UEventId = o["uEventId"];
            ubill.UMemberId = o["uMemberId"];
            ubill.Amount = o["amount"];
            ubill.ReadOnlyFields["Name"] = o["memberName"];
            return ubill;
        }
        private static UPayment UploadPayment(JsonValue o)
        {
            UPayment upayment = new UPayment();
            upayment.Id = o["id"];
            upayment.UEventId = o["uEventId"];
            upayment.UMemberId = o["uMemberId"];
            upayment.Amount = o["amount"];
            upayment.ReadOnlyFields["Name"] = o["memberName"];
            return upayment;
        }
        private static UTotal UploadTotal(JsonValue o)
        {
            UTotal utotal = new UTotal();
            utotal.Amount = o["amount"];
            utotal.DebtorName = o["debtorName"];
            utotal.LenderName = o["lenderName"];
            return utotal;
        }

        private static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            return obj;
        }
    }
}