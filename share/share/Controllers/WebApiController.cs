using System;
using System.IO;
using System.Net;
using System.Json;
using System.Collections.Generic;
using System.Reflection;

namespace share
{
    public static class WebApiController
    {
        public class Hosting
        {
            public static string m_Work = "http://192.168.0.73:2562/";
            public static string m_Home = "http://192.168.1.4:2562/";//Айпишник домашнего компа
            public static string m_Host = "http://46.101.214.70/";//Айпишник компа на хостинге Дениса

            public static string Current
            {
                get
                {
                    return Hosting.m_Home;
                }
            }
        }

        public static bool Registrate(RegisterViewModel m)
        {
            try
            {
                string url = Hosting.Current + "api/AccountApi/Register";
                JsonValue result = HttpClient.Post(url, m);
                UUser uUser = UploadObject<UUser>(result);
                LocalDBController.CreateObject(uUser);
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
                JsonValue result = HttpClient.Post(url, m);
                UUser uUser = UploadObject<UUser>(result);
                LocalDBController.CreateObject(uUser);
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
                UUser uUser = LocalDBController.LoadUserList()[0];
                if (!string.IsNullOrWhiteSpace(uUser.Id))
                {
                    LocalDBController.DeleteObject(uUser);
                }
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
                m.UUserId = LocalDBController.LoadUserList()[0].Id;

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

        public static List<UGroup> LoadGroupList()
        {
            string userId = null;
            try
            {
                userId = LocalDBController.LoadUserList()[0].Id;
            }
            catch {
                //2020327_Exception
                // Попытка номер 2
            }
            if (userId == null)
                return null;
            string url = Hosting.Current + "api/UGroupsApi/ByUser/" + userId;
            return LoadObjectList<UGroup>(url);                 
        }
        public static List<UDebt> LoadDebtList(int groupId)
        {
            string url = Hosting.Current + "api/UDebtsApi/ByGroup/" + groupId;
            return LoadObjectList<UDebt>(url);
        }
        public static List<UBill> LoadBillList(int eventId)
        {
            string url = Hosting.Current + "api/UBillsApi/ByEvent/" + eventId;
            return LoadObjectList<UBill>(url);
        }
        public static List<UEvent> LoadEventList(int groupId)
        {
            string url = Hosting.Current + "api/UEventsApi/ByGroup/" + groupId;
            return LoadObjectList<UEvent>(url);
        }
        public static List<UMember> LoadMemberList(int groupId)
        {
            string url = Hosting.Current + "api/UMembersApi/ByGroup/" + groupId;
            return LoadObjectList<UMember>(url);
        }
        public static List<UPayment> LoadPaymentList(int eventId)
        {
            string url = Hosting.Current + "api/UPaymentsApi/ByEvent/" + eventId;
            return LoadObjectList<UPayment>(url);
        }
        public static List<UTotal> LoadTotalListByGroup(int groupId)
        {
            string url = Hosting.Current + "api/UTotalsApi/ByGroup/" + groupId;
            return LoadObjectList<UTotal>(url);
        }
        public static List<UTotal> LoadTotalListByEvent(int eventId)
        {
            string url = Hosting.Current + "api/UTotalsApi/ByEvent/" + eventId;
            return LoadObjectList<UTotal>(url);
        }

        public static bool ExportGroup(int id)
        {
            try
            {
                UGroup o = LocalDBController.LoadFullGroupDetails(id);
                o.UUserId = LocalDBController.LoadUserList()[0].Id;
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
        public static bool ImportGroup(int id)
        {
            string url = Hosting.Current + "api/UGroupsApi/Import/" + id.ToString();
            try
            {
                JsonValue ugroupJSON = HttpClient.Get(url);
                UGroup ugroup = UploadObject<UGroup>(ugroupJSON);
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
                HttpClient.Put(url, o);
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
                string url = Hosting.Current + "api/" + o.Controller + "/" + o.Id;
                HttpClient.Delete(url);
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
                        return UploadObject<T>(o);
                    }
                }
            }
            catch (Exception e)
            {
                var error = e;
            }

            return null;
        }
        private static List<T> LoadObjectList<T>(string commandText) where T : UObject
        {
            List<T> items = new List<T>();
            try
            {
                JsonValue uobjectJSONList = HttpClient.Get(commandText);
                foreach (JsonValue uobjectJSON in uobjectJSONList)
                {
                    T uobject = UploadObject<T>(uobjectJSON);
                    items.Add(uobject);
                }
            }
            catch (Exception e)
            {
                var error = e;
            }

            return items;
        }

        private static string GetKey(string propertyName)
        {
            return char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
        }
        public static T UploadObject<T>(JsonValue o) where T : UObject
        {
            T result = Activator.CreateInstance(typeof(T)) as T;
            var properties = typeof(T).GetProperties();
            foreach (var p in properties)
            {
                string key = GetKey(p.Name);
                if (o.ContainsKey(key))
                {
                    var jsonValue = o[key];
                    if (jsonValue == null)
                        continue;

                    object value = null;
                    if (jsonValue.JsonType == JsonType.Number)
                    {
                        if (p.PropertyType == typeof(int))
                        {
                            value = (int)jsonValue;
                        }
                        else
                        {
                            value = (double)jsonValue;
                        }
                    }
                    else if(jsonValue.JsonType == JsonType.String)
                    {
                        value = (string)jsonValue;
                    }
                    else if(jsonValue.JsonType == JsonType.Array)
                    {
                        dynamic itemList = Activator.CreateInstance(p.PropertyType);
                        Type g = itemList.GetType().GetGenericArguments()[0];
                        foreach (JsonValue i in jsonValue)
                        {
                            MethodInfo method = typeof(WebApiController).GetMethod("UploadObject")
                             .MakeGenericMethod(new Type[] { g });
                            dynamic item = method.Invoke(null, new object[] { i });
                            itemList.Add(item);
                        }
                        value = itemList;
                    }
                    else continue;
                    p.SetValue(result, value);
                }
            }
            return result;
        }
    }
}