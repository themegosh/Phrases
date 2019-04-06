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
using Google.Cloud.TextToSpeech.V1;

namespace MyVoiceMVC.Services.TextToSpeech
{
    public class GoogleTextToSpeechService : ITextToSpeech
    {
        //query watson and save the file to the ~/App_Data/ dir based on the hash'd "Text"
        public async Task<Phrase> GetTTS(Phrase phrase)
        {
            try
            {
                var ttsClient = TextToSpeechClient.Create();
                
                // Perform the Text-to-Speech request, passing the text input
                // with the selected voice parameters and audio file type
                var response = await ttsClient.SynthesizeSpeechAsync(new SynthesizeSpeechRequest
                {
                    Input = new SynthesisInput
                    {
                        Text = phrase.text
                    },
                    Voice = new VoiceSelectionParams
                    {
                        LanguageCode = "en-US", // Build the voice request, select the language code ("en-US")
                        SsmlGender = SsmlVoiceGender.Female // and the SSML voice gender ("neutral")
                    },
                    AudioConfig = new AudioConfig
                    {
                        AudioEncoding = AudioEncoding.Mp3
                    }
                });

                var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/"), phrase.guid + ".mp3");

                // Write the binary AudioContent of the response to an MP3 file.
                using (Stream output = File.Create(filePath))
                {
                    response.AudioContent.WriteTo(output);
                    Console.WriteLine($"Audio content written to file '{output}'");
                }

                return phrase;
            }
            catch (Exception ex)
            {
                await LogRepository.Log(ex.ToString());
                throw ex;
            }
        }
    }
}