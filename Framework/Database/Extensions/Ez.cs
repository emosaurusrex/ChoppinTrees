using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Framework
{
    public static class Ez
    {
        public static int EzGetOrdinal(this SqlDataReader rs, string name)
        {
            for (var i = 0; i < rs.FieldCount; i++)
            {
                if (rs.GetName(i).ToLower() == name.ToLower())
                {
                    return i;
                }
            }
            return -1;
        }

        public static object EzValue(this SqlDataReader rs, string name)
        {
            int ord = rs.EzGetOrdinal(name);
            if (rs.IsDBNull(ord))
            {
                return null;
            }
            else
            {
                return rs.GetValue(ord);
            }
        }

        public static string EzString(this SqlDataReader rs, string name)
        {
            return rs.EzString(name, string.Empty);
        }
        public static string EzString(this SqlDataReader rs, string name, string ifnull)
        {
            int ord = rs.EzGetOrdinal(name);
            return rs.IsDBNull(ord) ? ifnull : rs.GetString(ord);
        }
        public static int EzInt(this SqlDataReader rs, string name)
        {
            int ord = rs.EzGetOrdinal(name);
            return rs.IsDBNull(ord) ? 0 : rs.GetInt32(rs.EzGetOrdinal(name));
        }
        public static long EzLong(this SqlDataReader rs, string name)
        {
            int ord = rs.EzGetOrdinal(name);
            return rs.IsDBNull(ord) ? 0 : rs.GetInt64(ord);
        }
        public static DateTime EzDate(this SqlDataReader rs, string name)
        {
            return rs.GetDateTime(rs.EzGetOrdinal(name));
        }
        public static bool EzBool(this SqlDataReader rs, string name)
        {
            return rs.GetBoolean(rs.EzGetOrdinal(name));
        }
        public static double EzDouble(this SqlDataReader rs, string name)
        {
            return rs.GetDouble(rs.EzGetOrdinal(name));
        }
        public static decimal EzDecimal(this SqlDataReader rs, string name)
        {
            int ord = rs.EzGetOrdinal(name);
            if (rs.IsDBNull(ord))
            {
                return 0m;
                //throw new Exception("Data is null for column: " + name);
            }
            return rs.GetDecimal(rs.EzGetOrdinal(name));
        }
        public static bool EzNull(this SqlDataReader rs, string name)
        {
            int ord = rs.EzGetOrdinal(name);
            return rs.IsDBNull(ord);
        }
    }
}
