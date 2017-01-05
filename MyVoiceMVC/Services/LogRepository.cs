using MyVoiceMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyVoiceMVC.Services
{
    public class LogRepository
    {
        public static void Log(string text)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@date", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt")));
                parameters.Add(new SqlParameter("@text", text));
                
                DataAccess.ExecSql("INSERT INTO dbo.Logs (date, text) VALUES (@date, @text)", parameters);
            }
            catch (Exception)
            {
                //well, were screwed :P
                //throw ex;
            }
        }
    }
}