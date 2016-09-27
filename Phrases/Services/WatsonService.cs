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
                var filenameWithoutExtention = HttpContext.Current.Server.MapPath("~/tts/");

                //quickPhrase processing
                if (phrase.Contains("tempGuid"))
                    filenameWithoutExtention += phrase["tempGuid"];
                else
                    filenameWithoutExtention += phrase["guid"];
                
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

                    ConvertToMp3(filenameWithoutExtention);
                    
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

        private static void ConvertToMp3(string filenameWithoutExtention)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = new System.Diagnostics.ProcessStartInfo();
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo.FileName = @"C:\sox\sox.exe";
            process.StartInfo.Arguments = " -t wav -v 3.0 " + filenameWithoutExtention + ".wav -t mp3 -C 128.2 " + filenameWithoutExtention + ".mp3";
            //phrase.Add("Argruments", process.StartInfo.Arguments);
            process.Start();
            process.WaitForExit();
            //int exitCode = process.ExitCode;
        }

        private class TTSRequest
        {
            public string text { get; set; } = "";
        }

    }
}