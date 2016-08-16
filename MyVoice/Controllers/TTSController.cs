using MyVoice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DocumentModel;
using System.Text;
using MyVoice.Services;

namespace MyVoice.Controllers
{
    public class TTSController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetAllPhrases()
        {
            try
            {
                return jsonResponse(JsonConvert.SerializeObject(new PhraseRepository().GetAllPhrases()), HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return jsonResponse(ex.ToString(), HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public HttpResponseMessage SavePhrase([FromBody] object value)
        {
            try
            {
                Document phrase = Document.FromJson(value.ToString());

                PhraseService.SavePhrase(phrase);
                
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
