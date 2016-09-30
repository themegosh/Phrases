using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
                var customAudioActive = false;
                if (phrase.ContainsKey("customAudio"))
                    customAudioActive = (bool)((Document)phrase["customAudio"])["active"];

                if (!customAudioActive) //only update if we dont have a custom source set
                {
                    //determin if we actually need to query watson for a new audio file
                    var referencePhrase = PhraseRepository.GetPhrase(phrase["guid"]);

                    if (!String.Equals(referencePhrase["text"], phrase["text"], StringComparison.Ordinal) //is the text different?
                        || (bool)((Document)phrase["customAudio"])["justChanged"] == true) //we're things just changed? (forced refresh)
                        shouldGetTTS = true;
                }
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
            if (File.Exists(filePath + ".wav"))
                File.Delete(filePath + ".wav");

            if (File.Exists(filePath + ".mp3"))
                File.Delete(filePath + ".mp3");
        }

        public static Document ProcessCustomAudioSource(Document phrase, MultipartFileData file)
        {
            string root = HttpContext.Current.Server.MapPath("~/App_Data/");
            var mp3FilePath = root + phrase["guid"] + ".mp3";
            
            //convert the uploaded file
            switch (file.Headers.ContentType.MediaType)
            {
                case "audio/mp3":
                case "audio/wav":
                case "audio/x-m4a":

                    ConvertToMp3(file.LocalFileName, mp3FilePath, 8);

                    //delete the temporary upload file
                    if (File.Exists(file.LocalFileName))
                        File.Delete(file.LocalFileName);
                    break;
                default:
                    throw new Exception("Not supported media.");
            }

            //add the new attributes to the phrase object
            var customAudioDocument = (Document) phrase["customAudio"];
            customAudioDocument["justChanged"] = false;
            customAudioDocument["uploadedName"] = file.Headers.ContentDisposition.FileName;

            SavePhrase(phrase, phrase["userId"]);

            return phrase;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFilePath"></param>
        /// <param name="outFilePath"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static int ConvertToMp3(string inFilePath, string outFilePath, int db)
        {
            //delete the existing phrase if it exists
            if (File.Exists(outFilePath))
                File.Delete(outFilePath);

            //ffmpeg.exe -i C:/test/goodnight.m4a -b:a 128K -af "volume=10dB" -vn C:/test/goodnight_10db.mp3        

            var args = String.Format(" -i \"{0}\" -b:a 128K -af \"volume={1}dB\" -vn \"{2}\"", inFilePath, db, outFilePath);

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = new System.Diagnostics.ProcessStartInfo();
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo.FileName = @"C:\ffmpeg\ffmpeg.exe";
            process.StartInfo.Arguments = args;
            //phrase.Add("Argruments", process.StartInfo.Arguments);
            process.Start();
            process.WaitForExit();

            return process.ExitCode;
        }

        //old
        //public static void ConvertToMp3Sox(string filenameWithoutExtention)
        //{
        //    System.Diagnostics.Process process = new System.Diagnostics.Process();
        //    process.StartInfo = new System.Diagnostics.ProcessStartInfo();
        //    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        //    process.StartInfo.FileName = @"C:\sox\sox.exe";
        //    process.StartInfo.Arguments = " -t wav -v 3.0 " + filenameWithoutExtention + ".wav -t mp3 -C 128.2 " + filenameWithoutExtention + ".mp3";
        //    //phrase.Add("Argruments", process.StartInfo.Arguments);
        //    process.Start();
        //    process.WaitForExit();
        //    //int exitCode = process.ExitCode;
        //}

    }
}