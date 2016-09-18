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

        [HttpGet]
        public HttpResponseMessage GetAudio(string id)
        {
            try
            {
                var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/tts/");
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                var stream = new FileStream(mappedPath + id, FileMode.Open);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg");

                //Response.AddHeader("Content-Range", "bytes 0-" + (resultStream.Length - 1).ToString() + "/" + resultStream.Length.ToString());
                result.Content.Headers.ContentRange = new ContentRangeHeaderValue(stream.Length - 1);
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
