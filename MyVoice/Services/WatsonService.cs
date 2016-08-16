using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyVoice.Services
{
    public static class WatsonService
    {
        private const string USERNAME = "a80450f8-88b3-4a4d-9b25-15d8adf2bc7f";
        private const string PASSWORD = "VGOobHWj5Nev";
        private const string URL = "https://stream.watsonplatform.net/text-to-speech/api/v1/";

        //not used, yet
        public static Task<string> GetVoices()
        {
            try
            {
                using (var handler = new HttpClientHandler { Credentials = new NetworkCredential(USERNAME, PASSWORD) })
                using (var client = new HttpClient(handler))
                using (var response = client.GetAsync(URL + "voices"))
                {
                    return response.Result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                new LogRepository().Log(ex.ToString());
                throw ex;
            }
            
        }

        //query watson and save the file to the ~/tts/ dir based on the hash'd "Text"
        public static bool GetTTS(Document phrase)
        {
            try
            {
                if (phrase["Text"] != null)
                {
                    using (var handler = new HttpClientHandler { Credentials = new NetworkCredential(USERNAME, PASSWORD) })
                    using (var client = new HttpClient(handler))
                    using (var response = client.PostAsJsonAsync(URL + "synthesize?voice=en-US_AllisonVoice", new TTSRequest() { text = phrase["Text"] }))
                    using (var mediaFile = response.Result.Content.ReadAsStreamAsync())
                    using (var fileStream = new FileStream(HttpContext.Current.Server.MapPath("~/tts/") + phrase["Hash"] + ".ogg", FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                    {
                        mediaFile.Result.CopyToAsync(fileStream);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                new LogRepository().Log(ex.ToString());
                throw ex;
            }
        }
        
        public class TTSRequest
        {
            public string text { get; set; } = "";
        }

        
    }
}