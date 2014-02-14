using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WoWAPI
{
    [DataContract]
    public class Talent
    {
        [DataMember(Name = "selected")]
        public bool Selected { get; set; }
        [DataMember(Name = "spec")]
        public Spec TalentSpec { get; set; }

        public static Talent Pop(SqlDataReader rs, bool primary)
        {
            var talent = new Talent();
            talent.Selected = primary;
            talent.TalentSpec = Spec.Pop(rs, primary ? "PrimarySpec" : "SecondarySpec");
            return talent;
        }
    }
}
