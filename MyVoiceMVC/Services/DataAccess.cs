using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyVoiceMVC.Services
{
    public class DataAccess
    {
        private const int SQL_COMMAND_TIMEOUT = 180;

        public static string GetString(string sql, List<SqlParameter> parameters)
        {
            string resultString = null;
            object result = 0;

            using (SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = SQL_COMMAND_TIMEOUT;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters.ToArray());
                try
                {
                    cn.Open();
                    result = cmd.ExecuteScalar();
                    if ((result != null))
                    {
                        if (result.GetType().Name != "DBNull")
                        {
                            resultString = result.ToString();
                        }
                        else
                        {
                            resultString = "";
                        }
                    }
                    else
                    {
                        resultString = "";
                    }
                    cn.Close();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return resultString;
        }

        public static int GetInt(string sql, List<SqlParameter> parameters)
        {
            int resultInt = 0;
            object result = 0;

            using (SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = SQL_COMMAND_TIMEOUT;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters.ToArray());
                try
                {
                    cn.Open();
                    result = cmd.ExecuteScalar();
                    if ((result != null))
                    {
                        if (result.GetType().Name != "DBNull")
                        {
                            resultInt = Convert.ToInt32(result);
                        }
                        else
                        {
                            resultInt = 0;
                        }
                    }
                    else
                    {
                        resultInt = 0;
                    }
                    cn.Close();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return resultInt;
        }

        public static double GetDouble(string sql, List<SqlParameter> parameters)
        {
            double resultDouble = 0;
            object result = 0;

            using (SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = SQL_COMMAND_TIMEOUT;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters.ToArray());
                try
                {
                    cn.Open();
                    result = cmd.ExecuteScalar();
                    if ((result != null))
                    {
                        if (result.GetType().Name != "DBNull")
                        {
                            resultDouble = Convert.ToDouble(result);
                        }
                        else
                        {
                            resultDouble = 0;
                        }
                    }
                    else
                    {
                        resultDouble = 0;
                    }
                    cn.Close();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return resultDouble;
        }

        public static DateTime GetDateTime(string sql, List<SqlParameter> parameters)
        {
            DateTime resultDate = new DateTime();
            object result = 0;

            using (SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = SQL_COMMAND_TIMEOUT;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters.ToArray());
                try
                {
                    cn.Open();
                    result = cmd.ExecuteScalar();
                    if ((result != null))
                    {
                        if (result.GetType().Name != "DBNull")
                        {
                            resultDate = Convert.ToDateTime(result);
                        }
                        else
                        {
                            resultDate = new DateTime();
                        }
                    }
                    else
                    {
                        resultDate = new DateTime();
                    }
                    cn.Close();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return resultDate;
        }

        public static DataRow GetDataRow(string sql, List<SqlParameter> parameters)
        {
            DataRow dataRow = null;
            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = null;

            using (SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = SQL_COMMAND_TIMEOUT;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters.ToArray());

                try
                {
                    cn.Open();
                    dataAdapter = new SqlDataAdapter(cmd);
                    dataAdapter.Fill(dataTable);
                    if (dataTable.Rows.Count > 0)
                    {
                        dataRow = dataTable.Rows[0];
                    }
                    cn.Close();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return dataRow;
        }

        public static DataTable GetTable(string sql, List<SqlParameter> parameters)
        {
            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = null;
            using (SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = SQL_COMMAND_TIMEOUT;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters.ToArray());

                try
                {
                    cn.Open();
                    dataAdapter = new SqlDataAdapter(cmd);
                    dataAdapter.Fill(dataTable);
                    cn.Close();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return dataTable;
        }

        public static int ExecSql(string sql, List<SqlParameter> parameters)
        {
            int affectedRows = -1;
            using (SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = SQL_COMMAND_TIMEOUT;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters.ToArray());

                try
                {
                    cn.Open();
                    affectedRows = cmd.ExecuteNonQuery();
                    cn.Close();
                }
                catch (Exception e)
                {
                    affectedRows = -1;
                    throw e;
                }
            }
            return affectedRows;
        }

        public static int ExecSqlInsert(string sql, List<SqlParameter> parameters)
        {
            int insertedId = -1;
            using (SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = SQL_COMMAND_TIMEOUT;
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters.ToArray());

                    cn.Open();
                    insertedId = Convert.ToInt32(cmd.ExecuteScalar());
                    cn.Close();
                }
                catch (Exception e)
                {
                    insertedId = -1;
                    throw e;
                }
            }
            return insertedId;
        }
    }
}