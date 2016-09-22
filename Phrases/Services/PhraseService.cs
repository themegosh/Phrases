using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Phrases.Services
{
    public class PhraseService
    {
        public static void DeletePhrase(Document phrase)
        {
            PhraseRepository.DeletePhrase(phrase);

            string wavPath = HttpContext.Current.Server.MapPath("~/tts/") + phrase["guid"] + ".wav";
            if (System.IO.File.Exists(wavPath))
            {
                System.IO.File.Delete(wavPath);
            }
            string mp3Path = HttpContext.Current.Server.MapPath("~/tts/") + phrase["guid"] + ".mp3";
            if (System.IO.File.Exists(mp3Path))
            {
                System.IO.File.Delete(mp3Path);
            }
        }

        public static Document SavePhrase(Document phrase, string userId)
        {
            if (!phrase.Contains("guid"))
                phrase["guid"] = Guid.NewGuid().ToString();
            
            if (!phrase.Contains("userId"))
                phrase["userId"] = userId;

            //TODO determin if we actually need to query watson for a new audio file
            phrase = WatsonService.GetTTS(phrase);
            PhraseRepository.SavePhrase(phrase);
            return phrase;
        }
        
        public static List<dynamic> GetAllPhrases(string userId)
        {
            return PhraseRepository.GetAllPhrases(userId);
        }

        public static Document QuickPhrase(Document phrase)
        {
            if (!phrase.Contains("guid"))
                phrase["guid"] = Guid.NewGuid().ToString();
            
            phrase = WatsonService.GetTTS(phrase);
            //if (HttpContext.Current.Session["QuickPhrase"] != null)
            //{
            //    DeletePhrase((Document)HttpContext.Current.Session["QuickPhrase"]);
            //}
            //HttpContext.Current.Session["QuickPhrase"] = phrase;

            return phrase;
        }

    }
}