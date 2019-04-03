using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyVoiceMVC.Repositories
{
    public class BaseRepository
    {
        protected static SqlConnection OpenConnection()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }
    }
}