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
                var filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/tts/") + id + ".mp3";

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
        public HttpResponseMessage CustomAudioUpload()
        {
            try
            {
                var results = "";
                string root = HttpContext.Current.Server.MapPath("~/App_Data");
                var provider = new MultipartFormDataStreamProvider(root);

                // Read the form data.
                Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    results += file.Headers.ContentDisposition.FileName;
                    results += "Server file path: " + file.LocalFileName;
                }

                return jsonResponse(results, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return jsonResponse(ex.ToString(), HttpStatusCode.InternalServerError);
            }
        }


        private HttpResponseMessage jsonResponse(string obj, HttpStatusCode status)
        {
            if (status == HttpStatusCode.InternalServerError)
            {
                obj = JsonConvert.SerializeObject(obj);
            }

            HttpResponseMessage response = Request.CreateResponse(status);
            response.Content = new StringContent(obj, Encoding.UTF8, "application/json");
            return response;
        }
    }
}
