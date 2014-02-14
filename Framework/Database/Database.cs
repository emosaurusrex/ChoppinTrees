using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace Framework
{
    public static class Database
    {
        /// <summary>
        /// Connects to a specified database.
        /// </summary>
        /// <param name="defaultDB">The database to connect to.</param>
        /// <returns>Returns an open SqlConnection.</returns>
        public static SqlConnection Connect(string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            return con;
        }
        public static SqlConnection Connect()
        {
            return Connect(Constants.DBConnection);
        }
        /// <summary>
        /// Performs a non-query; insert, update, etc.
        /// </summary>
        /// <param name="con">An open SqlConnection.</param>
        /// <param name="query">A query or name of the stored procedure to execute.</param>
        public static void Procedure(SqlConnection con, string query)
        {
            if (con.State != ConnectionState.Open)
            {
                throw new Exception("SqlConnection object passed to nonQuery is closed.");
            }
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// Performs a non-query; insert, update, etc.
        /// </summary>
        /// <param name="con">An open SqlConnection.</param>
        /// <param name="storedProc">The name of the stored procedure to execute.</param>
        /// <param name="args">A list of SqlParameters to run with the query.</param>
        public static void Procedure(SqlConnection con, string storedProc, List<SqlParameter> args)
        {
            Procedure(con, storedProc, args.ToArray());
        }
        /// <summary>
        /// Performs a non-query; insert, update, etc.
        /// </summary>
        /// <param name="con">An open SqlConnection.</param>
        /// <param name="storedProc">The name of the stored procedure to execute.</param>
        /// <param name="args">An array of SqlParameters to run with the query.</param>
        public static void Procedure(SqlConnection con, string storedProc, SqlParameter[] args)
        {
            if (con.State != ConnectionState.Open)
            {
                throw new Exception("SqlConnection object passed to nonQuery is closed.");
            }
            SqlCommand cmd = new SqlCommand(storedProc, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(args);
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// Performs a non-query; insert, update, etc.
        /// </summary>
        /// <param name="con">An open SqlConnection.</param>
        /// <param name="storedProc">The name of the stored procedure to execute.</param>
        /// <param name="arg">A single SqlParameter to run with the query.</param>
        public static void Procedure(SqlConnection con, string storedProc, SqlParameter arg)
        {
            if (con.State != ConnectionState.Open)
            {
                throw new Exception("SqlConnection object passed to nonQuery is closed.");
            }
            SqlCommand cmd = new SqlCommand(storedProc, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(arg);
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// Performs a query or stored procedure.
        /// </summary>
        /// <param name="conn">An open SqlConnection.</param>
        /// <param name="query">A query or stored procedure to execute.</param>
        /// <returns>Returns a SqlDataReader.</returns>
        public static SqlDataReader Query(SqlConnection conn, string query)
        {
            if (conn.State != ConnectionState.Open)
            {
                throw new Exception("SqlConnection object passed to recordSet is closed.");
            }
            SqlCommand cmd = new SqlCommand(query, conn);
            return cmd.ExecuteReader();
        }
        /// <summary>
        /// Performs a stored procedure.
        /// </summary>
        /// <param name="conn">An open SqlConnection.</param>
        /// <param name="query">The stored procedure to execute.</param>
        /// <param name="args">An array of SqlParameters to run with the procedure.</param>
        /// <returns>Returns a SqlDataReader.</returns>
        public static SqlDataReader Query(SqlConnection conn, string storedProc, SqlParameter[] args)
        {
            if (conn.State != ConnectionState.Open)
            {
                throw new Exception("SqlConnection object passed to recordSet is closed.");
            }
            SqlCommand cmd = new SqlCommand(storedProc, conn);
            cmd.Parameters.AddRange(args);
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd.ExecuteReader();
        }
        /// <summary>
        /// Performs a stored procedure.
        /// </summary>
        /// <param name="conn">An open SqlConnection.</param>
        /// <param name="query">The stored procedure to execute.</param>
        /// <param name="args">A list of SqlParameter to run with the procedure.</param>
        /// <returns>Returns a SqlDataReader.</returns>
        public static SqlDataReader Query(SqlConnection conn, string storedProc, List<SqlParameter> args)
        {
            if (conn.State != ConnectionState.Open)
            {
                throw new Exception("SqlConnection object passed to recordSet is closed.");
            }
            SqlCommand cmd = new SqlCommand(storedProc, conn);
            foreach (SqlParameter s in args)
            {
                cmd.Parameters.Add(s);
            }
            cmd.CommandType = CommandType.StoredProcedure;

            return cmd.ExecuteReader();
        }
        /// <summary>
        /// Executes a stored procedure.
        /// </summary>
        /// <param name="conn">An open SqlConnection.</param>
        /// <param name="query">The stored procedure to execute.</param>
        /// <param name="args">A single SqlParameter to run with the procedure.</param>
        /// <returns>Returns a SqlDataReader.</returns>
        public static SqlDataReader Query(SqlConnection conn, string storedProc, SqlParameter arg)
        {
            SqlParameter[] argArr = new SqlParameter[1];
            argArr[0] = arg;
            return Query(conn, storedProc, argArr);
        }
    }

}
