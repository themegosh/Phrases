using MyVoiceMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using System.Threading.Tasks;

namespace MyVoiceMVC.Repositories
{
    public class PhraseRepository : BaseRepository
    {
        public static async Task<Phrase> GetPhrase(string guid, string userGuid)
        {
            try
            {
                using (var conn = OpenConnection())
                {
                    var phrase = await conn.QueryFirstOrDefaultAsync<Phrase>("SELECT * FROM dbo.Phrases WHERE guid = @guid AND userGuid = @userGuid", new
                    {
                        guid,
                        userGuid
                    });

                    phrase.categories = await GetPhraseCategoriesGuid(phrase, conn);

                    return phrase;
                }
            }
            catch (Exception ex)
            {
                await LogRepository.Log(ex.ToString());
                throw ex;
            }
        }

        public static async Task InsertPhrase(Phrase phrase, string userGuid)
        {
            try
            {
                using (var conn = OpenConnection())
                {
                    await conn.ExecuteAsync("INSERT INTO dbo.Phrases (guid, text, date, userGuid, customAudioName) VALUES (@guid, @text, @date, @userGuid, @customAudioName)", new
                    {
                        phrase.guid,
                        phrase.text,
                        phrase.date,
                        userGuid,
                        phrase.customAudioName
                    });


                    await SyncPhraseCategories(phrase, conn);
                }
            }
            catch (Exception ex)
            {
                await LogRepository.Log(ex.ToString());
                throw ex;
            }
        }

        public static async Task UpdatePhrase(Phrase phrase, string userGuid)
        {
            try
            {
                using (var conn = OpenConnection())
                {
                    await conn.ExecuteAsync("UPDATE dbo.Phrases SET text = @text, date = @date, customAudioName = @customAudioName WHERE guid = @guid AND userGuid = @userGuid", new
                    {
                        phrase.guid,
                        phrase.text,
                        phrase.date,
                        userGuid,
                        phrase.customAudioName
                    });


                    await SyncPhraseCategories(phrase, conn);
                }
            }
            catch (Exception ex)
            {
                await LogRepository.Log(ex.ToString());
                throw ex;
            }
        }

        public static async Task<IEnumerable<Phrase>> GetAllPhrases(string userGuid)
        {
            try
            {
                using (var conn = OpenConnection())
                {
                    var phrases = await conn.QueryAsync<Phrase>("SELECT * FROM dbo.Phrases WHERE userGuid = @userGuid", new
                    {
                        userGuid
                    });

                    foreach (var phrase in phrases)
                    {
                        phrase.categories = await GetPhraseCategoriesGuid(phrase, conn);
                    }

                    return phrases;
                }
            }
            catch (Exception ex)
            {
                await LogRepository.Log(ex.ToString());
                throw ex;
            }
        }

        public static async Task DeletePhrase(Phrase phrase, string userGuid)
        {
            try
            {
                using (var conn = OpenConnection())
                {
                    await conn.ExecuteAsync("DELETE FROM dbo.Phrases WHERE guid = @guid AND userGuid = @userGuid", new
                    {
                        phrase.guid,
                        userGuid
                    });
                }
            }
            catch (Exception ex)
            {
                await LogRepository.Log(ex.ToString());
                throw ex;
            }

        }

        public static async Task InsertCategory(Category category, string userGuid)
        {
            try
            {
                using (var conn = OpenConnection())
                {
                    await conn.ExecuteAsync("INSERT INTO dbo.Categories (guid, name, userGuid, icon) VALUES (@guid, @name, @userGuid, @icon)", new
                    {
                        category.guid,
                        category.name,
                        category.icon,
                        userGuid
                    });
                }
            }
            catch (Exception ex)
            {
                await LogRepository.Log(ex.ToString());
                throw ex;
            }

        }

        public static async Task UpdateCategory(Category category, string userGuid)
        {
            try
            {
                using (var conn = OpenConnection())
                {
                    await conn.ExecuteAsync("UPDATE dbo.Categories SET name = @name, icon = @icon WHERE guid = @guid AND userGuid = @userGuid", new
                    {
                        category.guid,
                        category.name,
                        category.icon,
                        userGuid
                    });
                }
            }
            catch (Exception ex)
            {
                await LogRepository.Log(ex.ToString());
                throw ex;
            }
        }

        public static async Task DeleteCategory(Category category, string userGuid)
        {
            try
            {
                using (var conn = OpenConnection())
                {
                    await conn.ExecuteAsync("DELETE FROM dbo.Categories WHERE guid = @guid AND userGuid = @userGuid", new
                    {
                        category.guid,
                        userGuid
                    });
                }
            }
            catch (Exception ex)
            {
                await LogRepository.Log(ex.ToString());
                throw ex;
            }
        }

        public static async Task<IEnumerable<Category>> GetAllCategories(string userGuid)
        {
            try
            {
                using (var conn = OpenConnection())
                {
                    return await conn.QueryAsync<Category>("SELECT * FROM dbo.Categories WHERE userGuid = @userGuid", new
                    {
                        userGuid
                    });
                }
            }
            catch (Exception ex)
            {
                await LogRepository.Log(ex.ToString());
                throw ex;
            }

        }

        #region "Helpers"

        private static async Task SyncPhraseCategories(Phrase phrase, SqlConnection conn)
        {
            using (var trans = conn.BeginTransaction())
            {
                await conn.QueryAsync("DELETE FROM dbo.PhraseCategories WHERE phraseGuid = @phraseGuid", new
                {
                    phrase.guid
                }, trans);

                foreach (var categoryGuid in phrase.categories)
                {
                    await conn.ExecuteAsync("INSERT INTO dbo.PhraseCategories (phraseGuid, categoryGuid) VALUES (@phraseGuid, @categoryGuid)", new
                    {
                        phrase.guid,
                        categoryGuid
                    }, trans);
                }
                trans.Commit();
            }
        }

        private static async Task<IEnumerable<string>> GetPhraseCategoriesGuid(Phrase phrase, SqlConnection conn)
        {
            return await conn.QueryAsync<string>("SELECT categoryGuid FROM PhraseCategories WHERE phraseGuid = @guid", new
            {
                phrase.guid
            });
        }

        #endregion
    }
}