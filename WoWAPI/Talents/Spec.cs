using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Framework;

namespace WoWAPI
{
    [DataContract]
    public class Spec
    {
        [DataMember(Name="icon")]
        public string Icon { get; set; }
        [DataMember(Name="name")]
        public string Name { get; set; }
        [DataMember(Name="order")]
        public int Order { get; set; }

        public string GetIconPath()
        {
            return this.GetIconPath(36);
        }
        public string GetIconPath(int size)
        {
            if (string.IsNullOrEmpty(this.Icon))
            {
                throw new Exception("No icon specified.");
            }
            return Constants.MediaPath + "/" + size.ToString() + "/" + this.Icon + ".jpg";
        }

        public static Spec Pop(SqlDataReader rs, string prefix)
        {
            var spec = new Spec();
            spec.Icon = rs.EzString(prefix + "Icon");
            spec.Name = rs.EzString(prefix + "Name");
            return spec;
        }
    }
}
