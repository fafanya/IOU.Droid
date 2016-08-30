using System;
using System.IO;
using System.Net;
using System.Json;
using System.Runtime.Serialization.Json;

namespace share
{
    public class HttpClient
    {
        public static string Post(string url, object data)
        {
            string result = null;
            Stream stream = null;
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                stream = request.GetRequestStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(data.GetType());
                serializer.WriteObject(stream, data);
                stream.Close();

                response = request.GetResponse();
                stream = response.GetResponseStream();
                reader = new StreamReader(stream);
                result = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Exception e = ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
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
                WebRequest request = WebRequest.Create(new Uri(url));
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
        public static string Put(string url, object data)
        {
            string result = null;
            Stream stream = null;
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "PUT";
                request.ContentType = "application/json";
                stream = request.GetRequestStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(data.GetType());
                serializer.WriteObject(stream, data);
                stream.Close();

                response = request.GetResponse();
                stream = response.GetResponseStream();
                reader = new StreamReader(stream);
                result = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Exception e = ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (stream != null)
                    stream.Close();
                if (response != null)
                    response.Close();
            }
            return result;
        }
        public static string Delete(string url)
        {
            string result = null;
            Stream stream = null;
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                WebRequest request = WebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "DELETE";
                response = request.GetResponse();
                stream = response.GetResponseStream();
                reader = new StreamReader(stream);
                result = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Exception e = ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (stream != null)
                    stream.Close();
                if (response != null)
                    response.Close();
            }
            return result;
        }
    }
}