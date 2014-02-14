using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Web
{
    public static class Request
    {
        private static HttpWebRequest GetRequest(string url, int timeout)
        {
            var req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "GET";
            req.Timeout = timeout;
            return req;
        }

        public static string GetJSON(string url, int timeout)
        {
            var req = GetRequest(url, timeout);
            WebResponse response = req.GetResponse();
            return ReadResponse(response);
        }

        public static string GetJSON(string url, int timeout, DateTime lastModified)
        {
            var req = GetRequest(url, timeout);
            req.IfModifiedSince = lastModified;
            WebResponse response = req.GetResponse();
            return ReadResponse(response);
        }

        public static async Task<string> GetJSONAsync(string url, int timeout)
        {
            var req = GetRequest(url, timeout);
            WebResponse response = await req.GetResponseAsync();
            return ReadResponse(response);
        }

        public static async Task<string> GetJSONAsync(string url, int timeout, DateTime lastModified)
        {
            var req = GetRequest(url, timeout);
            req.IfModifiedSince = lastModified;
            WebResponse response = await req.GetResponseAsync();
            return ReadResponse(response);
        }

        private static string ReadResponse(WebResponse response)
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
