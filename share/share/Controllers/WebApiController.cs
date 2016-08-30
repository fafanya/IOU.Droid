using System;
using System.IO;
using System.Net;
using System.Text;
using System.Json;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;

namespace share
{
    public class WebApiController
    {
        public class Hosting
        {
            public static string m_Work = "http://192.168.0.73:2562/";
            public static string m_Home = "http://192.168.1.4:2562/";
            public static string m_Host = "http://46.101.214.70/";

            public static string Current
            {
                get
                {
                    return Hosting.m_Host;
                }
            }
        }

        public static bool Registrate(RegisterViewModel m)
        {
            try
            {
                string url = Hosting.Current + "api/AccountApi/Register";
                string result = HttpClient.Post(url, m);

                UUser uUser = Deserialize<UUser>(result);
                LocalDBController.CreateUser(uUser.id, uUser.email);
                return true;
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
                string url = Hosting.Current + "api/AccountApi/Login";
                string result = HttpClient.Post(url, m);

                UUser uUser = Deserialize<UUser>(result);
                LocalDBController.CreateUser(uUser.id, uUser.email);
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
                LocalDBController.Logout();
            }
            catch
            {

            }
        }

        public static bool LoginInGroup(LoginGroupViewModel m)
        {
            try
            {
                string url = Hosting.Current + "api/UGroupsApi/Login";
                string result = HttpClient.Post(url, m);

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
                m.UUserId = LocalDBController.GetCurrentUserId();

                string url = Hosting.Current + "api/UMembersApi/Select";
                string result = HttpClient.Post(url, m);

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
            string url = Hosting.Current + "api/UMembersApi/ByGroup/" + groupId;
            try
            {
                JsonValue umemberJSONList = HttpClient.Get(url);
                foreach (JsonValue umemberJSON in umemberJSONList)
                {
                    UMember umember = UploadMember(umemberJSON);
                    items.Add(umember);
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
            string url = Hosting.Current + "api/UEventsApi/ByGroup/" + groupId;
            try
            {
                JsonValue ueventJSONList = HttpClient.Get(url);
                foreach (JsonValue ueventJSON in ueventJSONList)
                {
                    UEvent uevent = UploadEvent(ueventJSON);
                    items.Add(uevent);
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
            string url = Hosting.Current + "api/UDebtsApi/ByGroup/" + groupId;
            try
            {
                JsonValue udebtJSONList = HttpClient.Get(url);
                foreach (JsonValue udebtJSON in udebtJSONList)
                {
                    UDebt udebtJ = UploadDebt(udebtJSON);
                    items.Add(udebtJ);
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
            string url = Hosting.Current + "api/UPaymentsApi/ByEvent/" + eventId;
            try
            {
                JsonValue upaymentJSONList = HttpClient.Get(url);
                foreach (JsonValue upaymentJSON in upaymentJSONList)
                {
                    UPayment upayment = UploadPayment(upaymentJSON);
                    items.Add(upayment);
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
            string url = Hosting.Current + "api/UBillsApi/ByEvent/" + eventId;
            try
            {
                JsonValue ubillJSONList = HttpClient.Get(url);
                foreach (JsonValue ubillJSON in ubillJSONList)
                {
                    UBill ubill = UploadBill(ubillJSON);
                    items.Add(ubill);
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
                string userId = LocalDBController.GetCurrentUserId();
                if (userId == null)
                    return items;

                string url = Hosting.Current + "api/UGroupsApi/ByUser/" + userId;
                JsonValue ugroupJSONList = HttpClient.Get(url);
                foreach (JsonValue ugroupJSON in ugroupJSONList)
                {
                    UGroup ugroup = UploadGroup(ugroupJSON);
                    items.Add(ugroup);
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
            string url = Hosting.Current + "api/UTotalsApi/ByGroup/" + groupId;
            try
            {
                JsonValue utotalJSONList = HttpClient.Get(url);
                foreach (JsonValue utotalJSON in utotalJSONList)
                {
                    UTotal utotal = UploadTotal(utotalJSON);
                    items.Add(utotal);
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
            string url = Hosting.Current + "api/UTotalsApi/ByEvent/" + eventId;
            try
            {
                JsonValue utotalJSONList = HttpClient.Get(url);
                foreach (JsonValue utotalJSON in utotalJSONList)
                {
                    UTotal utotal = UploadTotal(utotalJSON);
                    items.Add(utotal);
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
                UGroup o = LocalDBController.LoadFullGroupDetails(id);
                o.UUserId = LocalDBController.GetCurrentUserId();
                if (string.IsNullOrWhiteSpace(o.UUserId))
                    return false;

                string url = Hosting.Current + "api/UGroupsApi/Export";
                HttpClient.Post(url, o);
            }
            catch (Exception e)
            {
                var error = e;
            }
            return true;
        }
        public bool ImportGroup(int id)
        {
            string url = Hosting.Current + "api/UGroupsApi/Import/" + id.ToString();
            try
            {
                JsonValue ugroupJSON = HttpClient.Get(url);
                UGroup ugroup = UploadGroup(ugroupJSON);
                LocalDBController.UploadGroup(ugroup);
                return true;
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
                string url = Hosting.Current + "api/" + o.Controller;
                HttpClient.Post(url, o);
            }
            catch (Exception ex)
            {
                var error = ex;
            }
        }
        public static void UpdateObject<T>(T o) where T : UObject
        {
            try
            {
                string url = Hosting.Current + "api/" + o.Controller + "/" + o.Id;
                string result = HttpClient.Put(url, o);
            }
            catch (Exception e)
            {
                var error = e;
            }
        }
        public static void DeleteObject<T>(T o) where T : UObject
        {
            try
            {
                string url = Hosting.Current + "api/" + o.Controller + "/" + o.Id.ToString();
                string result = HttpClient.Delete(url);
            }
            catch (Exception er)
            {
                var erros = er;
            }
        }
        public static T LoadObjectDetails<T>(int id) where T : UObject
        {
            T item = Activator.CreateInstance(typeof(T)) as T;

            string url = Hosting.Current + "api/" + item.Controller + "/" + id.ToString();
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
            T obj = default(T);
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                obj = (T)serializer.ReadObject(ms);
            }
            catch(Exception ex)
            {
                Exception e = ex;
            }
            finally
            {
                if (ms != null)
                    ms.Close();
            }
            return obj;
        }
    }
}