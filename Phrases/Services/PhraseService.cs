﻿using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
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
            var shouldGetTTS = false;
            if (!phrase.Contains("userId"))
                phrase["userId"] = userId;

            if (!phrase.Contains("date"))
                phrase["date"] = DateTime.Now.ToString();

            if (!phrase.Contains("guid"))
            {
                phrase["guid"] = Guid.NewGuid().ToString();
                shouldGetTTS = true;
            }
            else
            {
                //determin if we actually need to query watson for a new audio file
                var referencePhrase = PhraseRepository.GetPhrase(phrase["guid"]);

                if (!String.Equals(referencePhrase["text"], phrase["text"], StringComparison.Ordinal)) //is the text different?
                    shouldGetTTS = true;
            }

            if (shouldGetTTS)
                phrase = WatsonService.GetTTS(phrase); //update the TTS

            PhraseRepository.SavePhrase(phrase);
            return phrase;            
        }
        
        public static List<dynamic> GetAllPhrases(string userId)
        {
            return PhraseRepository.GetAllPhrases(userId);
        }

        public static Document QuickPhrase(Document phrase)
        {
            if (!phrase.Contains("tempGuid"))
                phrase["tempGuid"] = Guid.NewGuid().ToString();
            
            phrase = WatsonService.GetTTS(phrase);

            string filePath = HttpContext.Current.Server.MapPath("~/tts/") + phrase["tempGuid"];

            //this quick phrase gets deleted after 1hr
            HttpContext.Current.Cache.Add("QuickPlay_" + phrase["tempGuid"], filePath, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(30), System.Web.Caching.CacheItemPriority.Normal, OnCachedItemRemoved);
            
            return phrase;
        }
        private static CacheItemRemovedCallback OnCachedItemRemoved = new CacheItemRemovedCallback(CachedItemRemovedCallback);
        private static void CachedItemRemovedCallback(string key, Object val, CacheItemRemovedReason reason)
        {
            var filePath = (string)val;
            //remove it
            if (System.IO.File.Exists(filePath + ".wav"))
            {
                System.IO.File.Delete(filePath + ".wav");
            }
            if (System.IO.File.Exists(filePath + ".mp3"))
            {
                System.IO.File.Delete(filePath + ".mp3");
            }
        }

    }
}