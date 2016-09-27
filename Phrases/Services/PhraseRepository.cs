using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phrases.Services
{
    public class PhraseRepository
    {
        private const string TABLE_NAME = "UserPhrases";

        public static void SavePhrase(Document phrase)
        {
            try
            {
                var client = new AmazonDynamoDBClient();
                Table phrasesTable = Table.LoadTable(client, TABLE_NAME);
                phrasesTable.PutItem(phrase);
            }
            catch (Exception ex)
            {
                LogRepository.Log(ex.ToString());
                throw ex;
            }
        }

        public static Document GetPhrase(string guid)
        {
            try
            {
                var client = new AmazonDynamoDBClient();
                Table phrasesTable = Table.LoadTable(client, TABLE_NAME);
                return phrasesTable.GetItem(new Primitive(guid));
            }
            catch (Exception ex)
            {
                LogRepository.Log(ex.ToString());
                throw ex;
            }
        }

        public static List<dynamic> GetAllPhrases(string userId)
        {
            try
            {
                var client = new AmazonDynamoDBClient();
                Table phrasesTable = Table.LoadTable(client, TABLE_NAME);
                List<dynamic> documents = new List<dynamic>();
                ScanFilter scanFilter = new ScanFilter();
                var values = new List<AttributeValue>();
                values.Add(new AttributeValue(userId));
                scanFilter.AddCondition("userId", ScanOperator.Equal, values);

                var search = phrasesTable.Scan(scanFilter);

                do
                {
                    List<Document> documentsSet = search.GetNextSet();
                    foreach (var document in documentsSet)
                    {
                        var item = JsonConvert.DeserializeObject<dynamic>(document.ToJson()); //this is disgusting... How does one retrieve a list of formatted json from dynamodb?
                        documents.Add(item);
                    }
                } while (!search.IsDone);

                return documents;
            }
            catch (Exception ex)
            {
                LogRepository.Log(ex.ToString());
                throw ex;
            }

        }

        public static void DeletePhrase(Document phrase)
        {
            try
            {
                var client = new AmazonDynamoDBClient();
                Table phrasesTable = Table.LoadTable(client, TABLE_NAME);
                phrasesTable.DeleteItem(phrase);
            }
            catch (Exception ex)
            {
                LogRepository.Log(ex.ToString());
                throw ex;
            }
        }
    }
}