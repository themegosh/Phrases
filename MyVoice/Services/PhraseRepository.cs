using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyVoice.Services
{
    public class PhraseRepository
    {
        private AmazonDynamoDBClient client;

        public PhraseRepository()
        {
            client = new AmazonDynamoDBClient();
        }

        public void SavePhrase(Document phrase)
        {
            try
            {
                Table phrasesTable = Table.LoadTable(client, "Phrases");
                phrasesTable.PutItem(phrase);
            }
            catch (Exception ex)
            {
                new LogRepository().Log(ex.ToString());
                throw ex;
            }
        }

        public List<dynamic> GetAllPhrases()
        {
            try
            {
                Table phrasesTable = Table.LoadTable(client, "Phrases");
                List<dynamic> documents = new List<dynamic>();
                ScanFilter scanFilter = new ScanFilter();

                var search = phrasesTable.Scan(scanFilter);

                do
                {
                    List<Document> documentsSet = search.GetNextSet();
                    foreach (var document in documentsSet)
                    {
                        dynamic item = JsonConvert.DeserializeObject<dynamic>(document.ToJson()); //this is disgusting... How does one retrieve a list of formatted json from dynamodb?
                        documents.Add(item);
                    }
                } while (!search.IsDone);

                return documents;
            }
            catch (Exception ex)
            {
                new LogRepository().Log(ex.ToString());
                throw ex;
            }

        }

        public void DeletePhrase(Document phrase)
        {
            try
            {
                Table phrasesTable = Table.LoadTable(client, "Phrases");
                phrasesTable.DeleteItem(phrase);
            }
            catch (Exception ex)
            {
                new LogRepository().Log(ex.ToString());
                throw ex;
            }
        }
    }
}