using Amazon.DynamoDBv2.DocumentModel;
using NAudio.Lame;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Phrases.Services
{
    public class WatsonService
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
                LogRepository.Log(ex.ToString());
                throw ex;
            }

        }

        //query watson and save the file to the ~/tts/ dir based on the hash'd "Text"
        public static Document GetTTS(Document phrase)
        {
            try
            {
                var filenameWithoutExtention = HttpContext.Current.Server.MapPath("~/tts/") + phrase["guid"];

                if (phrase["text"] != null)
                {
                    using (var handler = new HttpClientHandler { Credentials = new NetworkCredential(USERNAME, PASSWORD) })
                    using (var client = new HttpClient(handler))
                    using (var response = client.PostAsJsonAsync(URL + "synthesize?voice=en-US_AllisonVoice&accept=audio/wav", new TTSRequest() { text = phrase["text"] }))
                    using (var mediaFile = response.Result.Content.ReadAsStreamAsync())
                    using (var fileStream = new FileStream(filenameWithoutExtention + ".wav", FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                    {
                        mediaFile.Result.CopyToAsync(fileStream); //create the wav file (fallback)
                    }
                    //convert the wav to mp3
                    using (var reader = new WaveFileReader(filenameWithoutExtention + ".wav"))
                    using (var writer = new LameMP3FileWriter(filenameWithoutExtention + ".mp3", reader.WaveFormat, 128))
                    {
                        reader.CopyTo(writer); //create the mp3 as well (preferred source)
                        //phrase["url"] = "/api/tts/GetAudio?id="+ phrase["hash"] + ".mp3";
                    }
                    return phrase;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogRepository.Log(ex.ToString());
                throw ex;
            }
        }

        private class TTSRequest
        {
            public string text { get; set; } = "";
        }

    }
}