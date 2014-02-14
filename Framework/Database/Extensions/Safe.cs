using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Framework
{
    public static class Safe
    {
        public static string SafeGetString(this SqlDataReader rs, int index)
        {
            if (index == -1 || rs.IsDBNull(index))
            {
                return String.Empty;
            }
            else
            {
                return rs.GetString(index);
            }
        }
        public static string SafeGetString(this SqlDataReader rs, string name)
        {
            return rs.SafeGetString(rs.SafeGetOrdinal(name));
        }
        public static DateTime? SafeGetDate(this SqlDataReader rs, int index)
        {
            if (index == -1 || rs.IsDBNull(index))
            {
                return null;
            }
            else
            {
                return rs.GetDateTime(index);
            }
        }
        public static DateTime? SafeGetDate(this SqlDataReader rs, string name)
        {
            return rs.SafeGetDate(rs.SafeGetOrdinal(name));
        }
        public static int? SafeGetInt(this SqlDataReader rs, int index)
        {
            if (index == -1 || rs.IsDBNull(index) || rs.GetValue(index).ToString().Trim() == string.Empty)
            {
                return null;
            }
            else
            {
                return int.Parse(rs.GetValue(index).ToString());
            }
        }
        public static int? SafeGetInt(this SqlDataReader rs, string name)
        {
            return rs.SafeGetInt(rs.SafeGetOrdinal(name));
        }
        public static int SafeGetOrdinal(this SqlDataReader rs, string name)
        {
            // we don't know if the column exists, loop through every field until we find it
            for (var i = 0; i < rs.FieldCount; i++)
            {
                if (rs.GetName(i).ToLower() == name.ToLower())
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
