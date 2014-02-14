using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Runtime.Serialization.Json;

using System.Web.Script.Serialization;
using System.Collections.ObjectModel;
using System.Dynamic;

namespace Framework.Data
{
    public static class JSON
    {
        public static object Deserialize(string json, Type t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(t);
            MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
            return ser.ReadObject(ms);
        }
        public static T Deserialize<T>(string json)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }
        public static string Serialize(object obj, Type t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(t);
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, obj);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }
        public static string Serialize<T>(T t)
        {
            return Serialize(t, t.GetType());
        }
        public static dynamic Deserialize(string json)
        {
            return (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(json);
        }
    }
}
