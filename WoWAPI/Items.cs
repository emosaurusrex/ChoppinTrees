using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WoWAPI
{
    [DataContract]
    public class Items
    {
        [DataMember]
        public int AverageItemLevel { get; set; }
        [DataMember]
        public int EquippedItemLevel { get; set; }
    }
}
