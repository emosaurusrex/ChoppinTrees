using Framework.Data;
using Framework.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WoWAPI
{
    public enum ItemSlot
    {
        Head,
        Neck,
        Shoulder,
        Shirt,
        Chest,
        Waist,
        Legs,
        Feet,
        Wrist,
        Hands,
        Finger,
        Trinket,
        OneHand,
        Shield,
        Ranged,
        Cloak,
        TwoHand,
        Bag,
        Tabard,
        Robe,
        MainHand,
        OffHand,
        HeldInOffHand,
        Ammo,
        Thrown,
        RangedRight,
        Relic
    }
    [DataContract]
    public class Item
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "itemLevel")]
        public int ItemLevel { get; set; }
        [DataMember(Name = "inventoryType")]
        public ItemSlot Slot { get; set; }
        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        public static Item Pop(int itemID)
        {
            string json = Request.GetJSON(GetPopURL(itemID), Constants.Timeout);
            return JSON.Deserialize<Item>(json);
        }

        public static async Task<Item> PopAsync(int itemID)
        {
            string json = await Request.GetJSONAsync(GetPopURL(itemID), Constants.Timeout);
            return JSON.Deserialize<Item>(json);
        }

        public static List<Item> SearchByName(string name)
        {
            dynamic results = Framework.Data.JSON.Deserialize(GetSearchJSON(GetSearchURL(name)));
            return GetSearchList(results);
        }
        public static async Task<List<Item>> SearchByNameAsync(string name)
        {
            string json = await GetSearchJSONAsync(GetSearchURL(name));
            return GetSearchList(Framework.Data.JSON.Deserialize(json));
        }
        private static string GetSearchURL(string name)
        {
            return "http://www.wowhead.com/search?q=" + name.Replace(" ", "+") + "&opensearch";
        }
        
        private static string GetSearchJSON(string url)
        {
            return Request.GetJSON(url, Constants.Timeout);
        }

        private static async Task<string> GetSearchJSONAsync(string url)
        {
            string json = await Request.GetJSONAsync(url, 5000);
            return json;
        }

        private static List<Item> GetSearchList(dynamic results)
        {
            var items = new List<Item>();
            for (int i = 0; i < results[1].Count; i++)
            {
                if (results[1][i].Value.IndexOf("(Item)") != -1)
                {
                    items.Add(new Item()
                    {
                        Name = results[1][i].Value.Remove(results[1][i].Value.IndexOf(" (Item)")),
                        ID = int.Parse(results[7][i][1].Value.ToString()),
                        Icon = results[7][i][2].Value
                    });
                }
            }
            return items;
        }

        private static string GetPopURL(int itemID)
        {
            return Constants.BaseURL + "api/wow/item/" + itemID.ToString();
        }

        public string GetIconPath(int size)
        {
            if (string.IsNullOrEmpty(this.Icon))
            {
                throw new Exception("No icon was specified.");
            }
            return Constants.MediaPath + "/" + size.ToString() + "/" + this.Icon + Constants.MediaExt;
        }
        public string GetIconPath()
        {
            return GetIconPath(56);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
