//Напиши что нибудь тут:
//Написал
//commit for testbranch1
using System;
using System.IO;
using System.Net;
using System.Json;
using System.Runtime.Serialization.Json;

namespace share
{
    public static class HttpClient
    {
        public static JsonValue Post(string url, object data)
        {
            JsonValue result = null;
            Stream stream = null;
            WebResponse response = null;
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.ContentType = "application/json";

                request.Method = "POST";
                stream = request.GetRequestStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(data.GetType());
                serializer.WriteObject(stream, data);
                stream.Close();

                response = request.GetResponse();
                stream = response.GetResponseStream();
                result = JsonValue.Load(stream);
            }
            catch (Exception ex)
            {
                Exception e = ex;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (response != null)
                    response.Close();
            }
            return result;
        }
        public static JsonValue Get(string url)
        {
            JsonValue result = null;
            Stream stream = null;
            WebResponse response = null;
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.ContentType = "application/json";

                request.Method = "GET";

                response = request.GetResponse();
                stream = response.GetResponseStream();
                result = JsonValue.Load(stream);
            }
            catch (Exception ex)
            {
                Exception e = ex;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (response != null)
                    response.Close();
            }
            return result;
        }
        public static JsonValue Put(string url, object data)
        {
            string result = null;
            Stream stream = null;
            WebResponse response = null;
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.ContentType = "application/json";

                request.Method = "PUT";
                stream = request.GetRequestStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(data.GetType());
                serializer.WriteObject(stream, data);
                stream.Close();

                response = request.GetResponse();
                stream = response.GetResponseStream();
                result = JsonValue.Load(stream);
            }
            catch (Exception ex)
            {
                Exception e = ex;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (response != null)
                    response.Close();
            }
            return result;
        }
        public static JsonValue Delete(string url)
        {
            string result = null;
            Stream stream = null;
            WebResponse response = null;
            try
            {
                WebRequest request = WebRequest.Create(new Uri(url));
                request.ContentType = "application/json";

                request.Method = "DELETE";

                response = request.GetResponse();
                stream = response.GetResponseStream();
                result = JsonValue.Load(stream);
            }
            catch (Exception ex)
            {
                Exception e = ex;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (response != null)
                    response.Close();
            }
            return result;
        }
    }
}
