using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Settings
{
    public class Setting : System.Attribute
    {
        public string DefaultValue;
        public string SQLName;
        public Type UnderlyingType = typeof(string);
        public bool JSON;
        public Setting(string SQLName, string DefaultValue)
        {
            this.DefaultValue = DefaultValue;
            this.SQLName = SQLName;
        }
    }

    private static class Helper
    {
        private static Setting GetAttribute(Type t)
        {
            return (Setting)Attribute.GetCustomAttribute(t, typeof(Setting));
        }
        public static SqlDataReader GetSettingsRecordset()
        {
            using (SqlConnection con = Database.Connect())
            {
                return Database.Query(con, "Site.GetSettings");
            }
        }

        public static string GetSettingValue(SqlDataReader rs, Type t)
        {
            var attribute = GetAttribute(t);
            int ordinal;
            if ((ordinal = rs.EzGetOrdinal(attribute.SQLName)) != -1)
            {
                return rs.GetString(ordinal);
            }
            return attribute.DefaultValue;
        }

        public static void UpdateSetting(Type t, string value)
        {
            var attribute = GetAttribute(t);
            using (SqlConnection con = Database.Connect())
            {
                Database.Procedure(con, "Site.UpdateSetting", new List<SqlParameter>()
                {
                    new SqlParameter("@settingName", attribute.SQLName),
                    new SqlParameter("@settingValue", value)
                });
            }
        }
    }

    public static class Settings
    {
        private SqlDataReader settingsData;
        private static string _GuildTitle;
        [Setting("GuildTitle", "[Settings.GuildTitle]")]
        public static string GuildTitle
        {
            get
            {
                if (_GuildTitle == null)
                {
                    Refresh();
                }
                return _GuildTitle;
            }
            set
            {
                _GuildTitle = value;
            }
        }
        public static void Refresh()
        {
            using (SqlConnection con = Database.Connect())
            {
                SqlDataReader rs = Database.Query(con, "Site.GetSettings");
                if(!rs.Read())
                {
                    throw new Exception("Could not load settings.");
                }
                foreach (var pi in GetSettings())
                {
                    var attr = GetAttribute(pi);
                    if (attr.JSON)
                    {
                        pi.SetValue(null, Data.JSON.Deserialize(rs.EzString(attr.SQLName), attr.UnderlyingType));
                    }
                    else
                    {
                        pi.SetValue(null, Convert.ChangeType(rs.EzValue(attr.SQLName), attr.UnderlyingType));
                    }
                }
            }
        }
        public static void Save()
        {
            using (SqlConnection con = Database.Connect())
            {
                foreach (var pi in GetSettings())
                {
                    Database.Procedure(con, "Site.UpdateSetting", new List<SqlParameter>()
                    {
                        new SqlParameter("@settingName", GetAttribute(pi).SQLName),
                        new SqlParameter("@settingValue", pi.GetValue(null, null).ToString())
                    });
                }
            }
        }
        private static PropertyInfo[] GetSettings()
        {
            return typeof(Settings).GetProperties(BindingFlags.Static).Where(p => p.IsDefined(typeof(Setting), false)).ToArray();
        }
        private static Setting GetAttribute(PropertyInfo pi)
        {
            return (Setting)pi.GetCustomAttribute(typeof(Setting));
        }
    }
}
