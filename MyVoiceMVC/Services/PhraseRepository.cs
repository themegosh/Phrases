using MyVoiceMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyVoiceMVC.Services
{
    public static class PhraseRepository
    {
        private const string TABLE_NAME = "Phrases";

        public static Phrase GetPhrase(string guid, string userGuid)
        {
            try
            {
                //the phrase
                var sql = "SELECT * FROM dbo.Phrases WHERE guid = @guid AND userGuid = @userGuid";
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@guid", guid));
                parameters.Add(new SqlParameter("@userGuid", userGuid));
                var row = DataAccess.GetDataRow(sql, parameters);

                if (row == null)
                    return null;

                var phrase = new Phrase
                {
                    guid = guid,
                    text = row["text"].ToString(),
                    date = DateTime.Parse(row["date"].ToString()),
                    userGuid = row["userGuid"].ToString()
                };

                phrase.categories = GetPhraseCategoriesGuid(phrase);
                
                return phrase;
            }
            catch (Exception ex)
            {
                LogRepository.Log(ex.ToString());
                throw ex;
            }
        }

        public static void InsertPhrase (Phrase phrase, string userGuid)
        {
            try
            {
                var sql = "INSERT INTO dbo.Phrases (guid, text, date, userGuid, customAudioName) VALUES (@guid, @text, @date, @userGuid, @customAudioName)";
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@guid", phrase.guid));
                parameters.Add(new SqlParameter("@text", phrase.text));
                parameters.Add(new SqlParameter("@date", phrase.date));
                parameters.Add(new SqlParameter("@userGuid", userGuid));
                parameters.Add(new SqlParameter("@customAudioName", phrase.customAudio.name == null ? "" : phrase.customAudio.name));

                DataAccess.ExecSql(sql, parameters);

                SyncPhraseCategories(phrase);

            }
            catch (Exception ex)
            {
                LogRepository.Log(ex.ToString());
                throw ex;
            }
        }

        public static void UpdatePhrase (Phrase phrase, string userGuid)
        {
            try
            {
                var sql = "UPDATE dbo.Phrases SET text = @text, date = @date, customAudioName = @customAudioName WHERE guid = @guid AND userGuid = @userGuid";
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@guid", phrase.guid));
                parameters.Add(new SqlParameter("@text", phrase.text));
                parameters.Add(new SqlParameter("@date", phrase.date));
                parameters.Add(new SqlParameter("@userGuid", userGuid));
                parameters.Add(new SqlParameter("@customAudioName", phrase.customAudio.name == null ? "" : phrase.customAudio.name));

                DataAccess.ExecSql(sql, parameters);

                SyncPhraseCategories(phrase);
            }
            catch (Exception ex)
            {
                LogRepository.Log(ex.ToString());
                throw ex;
            }
        }

        public static List<Phrase> GetAllPhrases(string userGuid)
        {
            try
            {
                var phrases = new List<Phrase>();

                var sql = "SELECT * FROM dbo.Phrases WHERE userGuid = @userGuid";
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@userGuid", userGuid));
                var phraseTable = DataAccess.GetTable(sql, parameters);

                foreach (DataRow aPhrase in phraseTable.Rows)
                {
                    var phrase = new Phrase
                    {
                        guid = aPhrase["guid"].ToString(),
                        text = aPhrase["text"].ToString(),
                        date = DateTime.Parse(aPhrase["date"].ToString()),
                        userGuid = aPhrase["userGuid"].ToString()
                    };
                    
                    phrase.customAudio.hasCustomAudio = !String.IsNullOrEmpty(aPhrase["customAudioName"].ToString());
                    if (phrase.customAudio.hasCustomAudio)
                    {
                        phrase.customAudio.name = aPhrase["customAudioName"].ToString();
                    }

                    phrase.categories = GetPhraseCategoriesGuid(phrase);

                    phrases.Add(phrase);
                }
                return phrases;
            }
            catch (Exception ex)
            {
                LogRepository.Log(ex.ToString());
                throw ex;
            }
        }

        public static void DeletePhrase(Phrase phrase, string userGuid)
        {
            try
            {
                var sql = "DELETE FROM dbo.Phrases WHERE guid = @guid AND userGuid = @userGuid";
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@guid", phrase.guid));
                parameters.Add(new SqlParameter("@userGuid", userGuid));

                DataAccess.ExecSql(sql, parameters);
            }
            catch (Exception ex)
            {
                LogRepository.Log(ex.ToString());
                throw ex;
            }
            
        }

        public static void InsertCategory(Category category, string userGuid)
        {
            try
            {
                var sql = "INSERT INTO dbo.Categories (guid, name, userGuid, icon) VALUES (@guid, @name, @userGuid, @icon)";
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@guid", category.guid));
                parameters.Add(new SqlParameter("@name", category.name));
                parameters.Add(new SqlParameter("@icon", category.icon));
                parameters.Add(new SqlParameter("@userGuid", userGuid));

                DataAccess.ExecSql(sql, parameters);
            }
            catch (Exception ex)
            {
                LogRepository.Log(ex.ToString());
                throw ex;
            }
           
        }

        public static void UpdateCategory(Category category, string userGuid)
        {
            try
            {
                var sql = "UPDATE dbo.Categories SET name = @name, icon = @icon WHERE guid = @guid AND userGuid = @userGuid";
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@guid", category.guid));
                parameters.Add(new SqlParameter("@name", category.name));
                parameters.Add(new SqlParameter("@icon", category.icon));
                parameters.Add(new SqlParameter("@userGuid", userGuid));

                DataAccess.ExecSql(sql, parameters);
            }
            catch (Exception ex)
            {
                LogRepository.Log(ex.ToString());
                throw ex;
            }
        }

        public static void DeleteCategory(Category category, string userGuid)
        {
            try
            {
                var sql = "DELETE FROM dbo.Categories WHERE guid = @guid AND userGuid = @userGuid";
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@guid", category.guid));
                parameters.Add(new SqlParameter("@userGuid", userGuid));

                DataAccess.ExecSql(sql, parameters);
            }
            catch (Exception ex)
            {
                LogRepository.Log(ex.ToString());
                throw ex;
            }
        }

        public static List<Category> GetAllCategories(string userGuid)
        {
            try
            {
                var sql = "SELECT * FROM dbo.Categories WHERE userGuid = @userGuid";
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@userGuid", userGuid));

                var table = DataAccess.GetTable(sql, parameters);

                var categories = new List<Category>();
                foreach (DataRow row in table.Rows)
                {
                    var category = new Category();

                    category.guid = row["guid"].ToString();
                    category.name = row["name"].ToString();
                    category.icon = row["icon"].ToString();
                    category.userGuid = userGuid;

                    categories.Add(category);
                }
                return categories;
            }
            catch (Exception ex)
            {
                LogRepository.Log(ex.ToString());
                throw ex;
            }

        }
        
        #region "Helpers"

        private static void SyncPhraseCategories(Phrase phrase)
        {
            //delete existing category relationships
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@phraseGuid", phrase.guid));
            var sql = "DELETE FROM dbo.PhraseCategories WHERE phraseGuid = @phraseGuid";
            DataAccess.ExecSql(sql, parameters);

            foreach (var categoryGuid in phrase.categories)
            {
                //add the categories
                sql = "INSERT INTO dbo.PhraseCategories (phraseGuid, categoryGuid) VALUES (@phraseGuid, @categoryGuid)";
                parameters.Clear();
                parameters.Add(new SqlParameter("@phraseGuid", phrase.guid));
                parameters.Add(new SqlParameter("@categoryGuid", categoryGuid));
                DataAccess.ExecSql(sql, parameters);
            }
        }

        private static List<string> GetPhraseCategoriesGuid(Phrase phrase)
        {
            var categories = new List<string>();
            var sql = "SELECT categoryGuid FROM PhraseCategories WHERE phraseGuid = @phraseGuid";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@phraseGuid", phrase.guid));
            var table = DataAccess.GetTable(sql, parameters);

            foreach (DataRow dr in table.Rows)
            {
                categories.Add(dr["categoryGuid"].ToString());
            }

            return categories;
        }

        #endregion

        //public static Phrase GetPhrase(string guid)
        //{
        //    try
        //    {
        //        var client = new AmazonDynamoDBClient();
        //        Table phrasesTable = Table.LoadTable(client, TABLE_NAME);
        //        return phrasesTable.GetItem(new Primitive(guid));
        //    }
        //    catch (Exception ex)
        //    {
        //        LogRepository.Log(ex.ToString());
        //        throw ex;
        //    }
        //}

        //public static List<dynamic> GetAllPhrases(string userGuid)
        //{
        //    try
        //    {
        //        var client = new AmazonDynamoDBClient();
        //        Table phrasesTable = Table.LoadTable(client, TABLE_NAME);
        //        List<dynamic> documents = new List<dynamic>();
        //        ScanFilter scanFilter = new ScanFilter();
        //        var values = new List<AttributeValue>();
        //        values.Add(new AttributeValue(userGuid));
        //        scanFilter.AddCondition("userGuid", ScanOperator.Equal, values);

        //        var search = phrasesTable.Scan(scanFilter);

        //        do
        //        {
        //            List<Phrase> documentsSet = search.GetNextSet();
        //            foreach (var document in documentsSet)
        //            {
        //                var item = JsonConvert.DeserializeObject<dynamic>(document.ToJson()); //this is disgusting... How does one retrieve a list of formatted json from dynamodb?
        //                documents.Add(item);
        //            }
        //        } while (!search.IsDone);

        //        return documents;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogRepository.Log(ex.ToString());
        //        throw ex;
        //    }

        //}

        //public static void DeletePhrase(Phrase phrase)
        //{
        //    try
        //    {
        //        var client = new AmazonDynamoDBClient();
        //        Table phrasesTable = Table.LoadTable(client, TABLE_NAME);
        //        phrasesTable.DeleteItem(phrase);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogRepository.Log(ex.ToString());
        //        throw ex;
        //    }
        //}
    }
}