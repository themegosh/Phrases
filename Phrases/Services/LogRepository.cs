using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phrases.Services
{
    public class LogRepository
    {
        public static void Log(string info)
        {
            try
            {
                var dictionary = new Dictionary<string, string>
                {
                    { "Date", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") },
                    { "Message", info  }
                };
                var doc = Document.FromJson(JsonConvert.SerializeObject(dictionary));
                var client = new AmazonDynamoDBClient();
                Table LogsTable = Table.LoadTable(client, "Logs");
                LogsTable.PutItem(doc);
            }
            catch (Exception)
            {
                //well, were screwed :P
                //throw ex;
            }
        }
    }
}