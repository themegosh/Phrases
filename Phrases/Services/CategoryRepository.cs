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
    public class CategoryRepository
    {
        private const string TABLE_NAME = "UserCategories";

        public static Document SaveCategory(Document category, string userId)
        {
            try
            {
                if (!category.Contains("userId"))
                    category["userId"] = userId;

                if (!category.Contains("guid"))
                    category["guid"] = Guid.NewGuid().ToString();

                var client = new AmazonDynamoDBClient();
                Table categoryTable = Table.LoadTable(client, TABLE_NAME);
                categoryTable.PutItem(category);

                return category;
            }
            catch (Exception ex)
            {
                LogRepository.Log(ex.ToString());
                throw ex;
            }
        }

        public static List<dynamic> GetAllCategories(string userId)
        {
            try
            {
                var client = new AmazonDynamoDBClient();
                Table categoryTable = Table.LoadTable(client, TABLE_NAME);
                List<dynamic> documents = new List<dynamic>();
                ScanFilter scanFilter = new ScanFilter();
                var values = new List<AttributeValue>();
                values.Add(new AttributeValue(userId));
                scanFilter.AddCondition("userId", ScanOperator.Equal, values);

                var search = categoryTable.Scan(scanFilter);

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

        public static void DeleteCategory(Document category)
        {
            try
            {
                var client = new AmazonDynamoDBClient();
                Table categoryTable = Table.LoadTable(client, TABLE_NAME);
                categoryTable.DeleteItem(category);
            }
            catch (Exception ex)
            {
                LogRepository.Log(ex.ToString());
                throw ex;
            }
        }
    }
}