using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;

namespace MyVoice.Models
{
    //singleton DB pattern
    public class DBHelper
    {
        private static DBHelper instance;
        private static object syncRoot = new Object();
        private AmazonDynamoDBClient client;
        private Table phrasesTable;
        
        private DBHelper()
        {
            client = new AmazonDynamoDBClient();
            phrasesTable = Table.LoadTable(client, "Phrases");
        }
        
        public static DBHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        instance = new DBHelper();
                    }
                }
                return instance;
            }
        }

        public void SavePhrase(Document phrase)
        {
            phrasesTable.PutItem(phrase);
        }

        public List<dynamic> GetAllPhrases()
        {
            List<dynamic> documents = new List<dynamic>();
            ScanFilter scanFilter = new ScanFilter();

            var search = phrasesTable.Scan(scanFilter);

            do
            {
                List<Document> documentsSet = search.GetNextSet();
                foreach (var document in documentsSet)
                {
                    dynamic item = JsonConvert.DeserializeObject<dynamic>(document.ToJson());
                    documents.Add(item);
                }
            } while (!search.IsDone);
            
            return documents;
        }

        public void DeletePhrase(Document phrase)
        {
            phrasesTable.DeleteItem(phrase);
        }
        
    }
}