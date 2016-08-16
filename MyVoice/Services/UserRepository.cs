using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyVoice.Services
{
    public class UserRepository
    {
        private AmazonDynamoDBClient client;

        public UserRepository()
        {
            client = new AmazonDynamoDBClient();
        }

        public void Log(string info)
        {
            try
            {
                Table LogsTable = Table.LoadTable(client, "Logs");
                var dictionary = new Dictionary<string, AttributeValue>
                {
                    { DateTime.Now.Millisecond.ToString(), new AttributeValue { N = info } }
                };
                var doc = Document.FromAttributeMap(dictionary);
                LogsTable.PutItem(doc);
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
                throw ex;
            }
        }
    }
}