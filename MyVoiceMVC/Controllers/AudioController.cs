using MyVoiceMVC.Models;
using MyVoiceMVC.Repositories;
using MyVoiceMVC.Services;
using MyVoiceMVC.Services.TextToSpeech;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MyVoiceMVC.Controllers
{
    public class AudioController : ApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> GetAudio(string id, string userGuid)
        {
            try
            {
                var filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/") + id + ".mp3";

                if (!File.Exists(filePath))
                {
                    var phrase = await PhraseRepository.GetPhrase(id, userGuid);
                    if (phrase != null)
                        await new GoogleTextToSpeechService().GetTTS(phrase); //dev/production causes file inconsistiency. Create missing file.
                }

                var stream = new FileStream(filePath, FileMode.Open);
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(stream)
                };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg");
                result.Content.Headers.ContentRange = new ContentRangeHeaderValue(stream.Length);

                return result;
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JsonConvert.SerializeObject(ex.ToString()), Encoding.UTF8, "application/json");
                return response;
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> CustomAudioUpload()
        {
            try
            {
                string root = HttpContext.Current.Server.MapPath("~/App_Data");
                var provider = new MultipartFormDataStreamProvider(root);

                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                //parse out the phrase data
                var phrase = JsonConvert.DeserializeObject<Phrase>(provider.FormData.Get("phrase"));
                var userGuid = provider.FormData.Get("userGuid");

                // Throw failure if there isnt a file or its not properly formatted
                if (provider.FileData.Count != 1 || String.IsNullOrEmpty(provider.FileData[0].Headers.ContentDisposition.FileName))
                    throw new Exception("Upload should upload exactly one file.");

                //process the data
                phrase = await PhraseService.ProcessCustomAudioSource(
                    phrase,
                    provider.FileData[0],
                    userGuid
                );

                return Json(phrase);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.ToString());
            }
        }
    }
}
