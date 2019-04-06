using MyVoiceMVC.Models;
using MyVoiceMVC.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace MyVoiceMVC.Services.TextToSpeech
{
    public class WatsonService : ITextToSpeech
    {
        private static readonly string USERNAME = WebConfigurationManager.AppSettings["IBM_USERNAME"];
        private static readonly string PASSWORD = WebConfigurationManager.AppSettings["IBM_PASSWORD"];
        private const string URL = "https://stream.watsonplatform.net/text-to-speech/api/v1/";

        //not used, yet
        public async Task<string> GetVoices()
        {
            try
            {
                using (var handler = new HttpClientHandler { Credentials = new NetworkCredential(USERNAME, PASSWORD) })
                using (var client = new HttpClient(handler))
                using (var response = client.GetAsync(URL + "voices"))
                {
                    return await response.Result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                await LogRepository.Log(ex.ToString());
                throw ex;
            }

        }

        //query watson and save the file to the ~/App_Data/ dir based on the hash'd "Text"
        public async Task<Phrase> GetTTS(Phrase phrase)
        {
            try
            {
                var filepathWithoutExtention = HttpContext.Current.Server.MapPath("~/App_Data/");

                //quickPhrase processing
                //if (phrase.Contains("tempGuid"))
                //    filepathWithoutExtention += phrase["tempGuid"];
                //else
                filepathWithoutExtention += phrase.guid;
                var ttsRequest = new TTSRequest() { text = phrase.text };

                if (phrase.text != null)
                {
                    using (var handler = new HttpClientHandler { Credentials = new NetworkCredential(USERNAME, PASSWORD) })
                    using (var client = new HttpClient(handler))
                    using (var response = client.PostAsync(URL + "synthesize?voice=en-US_AllisonVoice&accept=audio/wav", new StringContent(JsonConvert.SerializeObject(ttsRequest), Encoding.UTF8, "application/json")))
                    using (var mediaFile = response.Result.Content.ReadAsStreamAsync())
                    using (var fileStream = new FileStream(filepathWithoutExtention + ".wav", FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                    {
                        await mediaFile.Result.CopyToAsync(fileStream); //create the wav file (fallback)
                    }

                    PhraseService.ConvertToMp3(filepathWithoutExtention + ".wav", filepathWithoutExtention + ".mp3", 8);

                    //delete the wav file
                    if (File.Exists(filepathWithoutExtention + ".wav"))
                        File.Delete(filepathWithoutExtention + ".wav");

                    return phrase;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                await LogRepository.Log(ex.ToString());
                throw ex;
            }
        }



        private class TTSRequest
        {
            public string text { get; set; } = "";
        }

    }
}