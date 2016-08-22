using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyVoice.Services
{
    public static class PhraseService
    {
        public static void DeletePhrase(Document phrase)
        {
            PhraseRepository.DeletePhrase(phrase);

            string wavPath = HttpContext.Current.Server.MapPath("~/tts/") + phrase["hash"] + ".wav";
            if (System.IO.File.Exists(wavPath))
            {
                System.IO.File.Delete(wavPath);
            }
            string mp3Path = HttpContext.Current.Server.MapPath("~/tts/") + phrase["hash"] + ".mp3";
            if (System.IO.File.Exists(mp3Path))
            {
                System.IO.File.Delete(mp3Path);
            }
        }

        public static void SavePhrase(Document phrase)
        {
            //do we need to query watson for a new audio file?
            if (!HashService.AreEqual(phrase["hash"], HashService.ToMd5(phrase["text"])))
            {
                phrase = WatsonService.GetTTS(phrase).Result;
            }
            PhraseRepository.SavePhrase(phrase);
            
        }

        public static List<Document> GetAllPhrases()
        {
            return PhraseRepository.GetAllPhrases();
        }

    }
}