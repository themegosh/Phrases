using MyVoiceMVC.Models;
using MyVoiceMVC.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace MyVoiceMVC.Services
{
    public class PhraseService
    {
        public static async Task DeletePhrase(Phrase phrase, string userGuid)
        {
            await PhraseRepository.DeletePhrase(phrase, userGuid);

            string wavPath = HttpContext.Current.Server.MapPath("~/tts/") + phrase.guid + ".wav";
            if (File.Exists(wavPath))
            {
                File.Delete(wavPath);
            }
            string mp3Path = HttpContext.Current.Server.MapPath("~/tts/") + phrase.guid + ".mp3";
            if (File.Exists(mp3Path))
            {
                File.Delete(mp3Path);
            }
        }

        public static async Task<Phrase> SavePhrase(Phrase phrase, string userGuid)
        {
            var shouldGetTTS = false;
            var newPhrase = String.IsNullOrEmpty(phrase.guid);

            if (phrase.userGuid != null)
                phrase.userGuid = userGuid;

            if (phrase.date != null)
                phrase.date = DateTime.Now;

            if (newPhrase)
            {
                phrase.guid = Guid.NewGuid().ToString();
                shouldGetTTS = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(phrase.customAudioName)) //only update if we dont have a custom source set
                {
                    //determin if we actually need to query watson for a new audio file
                    var referencePhrase = await PhraseRepository.GetPhrase(phrase.guid, userGuid);

                    if (!String.Equals(referencePhrase.text, phrase.text, StringComparison.Ordinal) //is the text different?
                        || phrase.forceRefresh == true) //we're things just changed? (forced refresh)
                        shouldGetTTS = true;
                }
            }

            if (shouldGetTTS)
                phrase = await WatsonService.GetTTS(phrase); //update the TTS

            if (newPhrase)
                await PhraseRepository.InsertPhrase(phrase, userGuid);
            else
                await PhraseRepository.UpdatePhrase(phrase, userGuid);

            phrase.forceRefresh = false; //reset

            return phrase;
        }

        public static async Task<IEnumerable<Phrase>> GetAllPhrases(string userId)
        {
            return await PhraseRepository.GetAllPhrases(userId);
        }

        public static async Task<Phrase> QuickPhrase(Phrase phrase)
        {
            phrase.temporary = true;

            phrase.guid = Guid.NewGuid().ToString(); //each "quick phrase" is temporary and will be deleted on its own. thus needs its own guid

            phrase = await WatsonService.GetTTS(phrase);

            string filePath = HttpContext.Current.Server.MapPath("~/tts/") + phrase.guid;

            //this quick phrase gets deleted after 1hr
            HttpContext.Current.Cache.Add("QuickPlay_" + phrase.guid, filePath, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(30), CacheItemPriority.Normal, OnCachedItemRemoved);

            return phrase;
        }
        private static readonly CacheItemRemovedCallback OnCachedItemRemoved = new CacheItemRemovedCallback(CachedItemRemovedCallback);
        private static void CachedItemRemovedCallback(string key, Object val, CacheItemRemovedReason reason)
        {
            var filePath = (string)val;

            //remove it
            if (File.Exists(filePath + ".wav"))
                File.Delete(filePath + ".wav");

            if (File.Exists(filePath + ".mp3"))
                File.Delete(filePath + ".mp3");
        }

        public static async Task<Phrase> ProcessCustomAudioSource(Phrase phrase, MultipartFileData file, string userGuid)
        {
            string root = HttpContext.Current.Server.MapPath("~/App_Data/");
            var mp3FilePath = root + phrase.guid + ".mp3";

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
            phrase.forceRefresh = false;
            phrase.customAudioName = file.Headers.ContentDisposition.FileName;

            await SavePhrase(phrase, userGuid);

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

            System.Diagnostics.Process process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = @"C:\ffmpeg\ffmpeg.exe",
                    Arguments = args
                }
            };
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