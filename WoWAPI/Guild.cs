using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WoWAPI
{
    [DataContract]
    public class Guild
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "realm")]
        public string Realm { get; set; }

        public string GetArmoryLink()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new Exception("Missing required info for guild armory link.");
            }
            return Constants.BaseURL + Constants.ArmoryPath + "guild/" + this.Realm + "/" + this.Name;
        }
    }
}
