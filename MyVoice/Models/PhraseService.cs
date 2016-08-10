using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyVoice.Models
{
    public static class PhraseService
    {
        public static void DeletePhrase(Document phrase)
        {
            DBHelper.Instance.DeletePhrase(phrase);

            string fullPath = HttpContext.Current.Server.MapPath("~/tts/") + phrase["Hash"] + ".ogg";
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

        public static void SavePhrase(Document phrase)
        {
            if (WatsonService.GetTTS(phrase))
            {
                DBHelper.Instance.SavePhrase(phrase);
            }
        }
    }
}