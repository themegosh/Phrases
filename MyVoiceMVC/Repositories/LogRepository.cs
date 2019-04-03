using MyVoiceMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using System.Threading.Tasks;

namespace MyVoiceMVC.Repositories
{
    public class LogRepository : BaseRepository
    {
        public static async Task Log(string text)
        {
            try
            {
                using (var conn = OpenConnection())
                {
                    await conn.ExecuteAsync("INSERT INTO dbo.Logs (date, text) VALUES (getdate(), @text)", new { text });
                }
            }
            catch (Exception)
            {
                //well, were screwed :P
                //throw ex;
            }
        }
    }
}