using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using Phrases.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Phrases.Models;
using System.Web.Services;
using System.Web;
using System.Threading.Tasks;

namespace Phrases.Controllers
{
    [Authorize]
    public class TTSController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SavePhrase([FromBody] object value)
        {
            try
            {
                Document phrase = Document.FromJson(value.ToString());
                var userId = User.Identity.GetUserId();
                phrase = PhraseService.SavePhrase(phrase, userId);

                return jsonResponse(phrase.ToJson(), HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return jsonResponse(ex.ToString(), HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeletePhrase([FromBody] object value)
        {
            try
            {
                Document phrase = Document.FromJson(value.ToString());
                PhraseService.DeletePhrase(phrase);

                return jsonResponse(phrase.ToJson(), HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return jsonResponse(ex.ToString(), HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public HttpResponseMessage QuickPhrase([FromBody] object value)
        {
            try
            {
                Document phrase = Document.FromJson(value.ToString());
                phrase = PhraseService.QuickPhrase(phrase);

                return jsonResponse(phrase.ToJson(), HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return jsonResponse(ex.ToString(), HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAudio(string id)
        {
            try
            {
                var filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/") + id + ".mp3";

                if (!File.Exists(filePath))
                {
                    var phrase = PhraseRepository.GetPhrase(id);
                    if (phrase != null)
                        WatsonService.GetTTS(phrase); //dev/production causes file inconsistiency. Create missing file.
                }

                var stream = new FileStream(filePath, FileMode.Open);
                var result = new HttpResponseMessage(HttpStatusCode.OK);

                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg");
                result.Content.Headers.ContentRange = new ContentRangeHeaderValue(stream.Length);
                
                return result;
            }
            catch (Exception ex)
            {
                return jsonResponse(ex.ToString(), HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUserData()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var userData = new UserData();
                userData.Categories = CategoryRepository.GetAllCategories(userId);
                userData.Phrases = PhraseService.GetAllPhrases(userId);
                userData.User = userId;

                return jsonResponse(JsonConvert.SerializeObject(userData), HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return jsonResponse(ex.ToString(), HttpStatusCode.InternalServerError);
            }
        }



        [HttpPost]
        public HttpResponseMessage SaveCategory([FromBody] object value)
        {
            try
            {
                Document category = Document.FromJson(value.ToString());
                var userId = User.Identity.GetUserId();
                category = CategoryRepository.SaveCategory(category, userId);

                return jsonResponse(category.ToJson(), HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return jsonResponse(ex.ToString(), HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteCategory([FromBody] object value)
        {
            try
            {
                Document category = Document.FromJson(value.ToString());
                CategoryRepository.DeleteCategory(category);

                return jsonResponse(category.ToJson(), HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return jsonResponse(ex.ToString(), HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> CustomAudioUpload()
        {
            try
            {
                string root = HttpContext.Current.Server.MapPath("~/App_Data");
                var provider = new MultipartFormDataStreamProvider(root);
                
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                //parse out the phrase data
                var phrase = Document.FromJson(provider.FormData.Get("phrase"));

                // Throw failure if there isnt a file or its not properly formatted
                if (provider.FileData.Count != 1 || String.IsNullOrEmpty(provider.FileData[0].Headers.ContentDisposition.FileName))
                    throw new Exception("Upload should contain exactly one file.");

                //process the data
                phrase = PhraseService.ProcessCustomAudioSource(
                    phrase,
                    provider.FileData[0]
                );

                return jsonResponse(phrase.ToJson(), HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return jsonResponse(ex.ToString(), HttpStatusCode.InternalServerError);
            }
        }
        
        private HttpResponseMessage jsonResponse(object obj, HttpStatusCode status)
        {
            HttpResponseMessage response = Request.CreateResponse(status);
            response.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            return response;
        }
    }
}
