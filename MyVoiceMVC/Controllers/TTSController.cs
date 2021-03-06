﻿using Microsoft.AspNet.Identity;
using MyVoiceMVC.Models;
using MyVoiceMVC.Repositories;
using MyVoiceMVC.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyVoiceMVC.Controllers
{
    [Authorize]
    public class TTSController : Controller
    {
        //public async Task<ActionResult> Index()
        //{
        //    var userId = User.Identity.GetUserId();

        //    //LogRepository.Log("Hello world!");

        //    return Json(new { stuff = userId }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public async Task<ActionResult> SavePhrase(Phrase phrase)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                phrase = await PhraseService.SavePhrase(phrase, userId);

                return Json(phrase, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { error = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeletePhrase(Phrase phrase)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                await PhraseService.DeletePhrase(phrase, userId);

                return Json(phrase, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { error = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> QuickPhrase(Phrase phrase)
        {
            try
            {
                phrase = await PhraseService.QuickPhrase(phrase);

                return Json(phrase, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { error = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public async Task<ActionResult> GetUserData()
        {
            try
            {
                var userId = User.Identity.GetUserId();

                var userData = new
                {
                    Categories = await PhraseRepository.GetAllCategories(userId),
                    Phrases = await PhraseService.GetAllPhrases(userId),
                    User = new
                    {
                        userGuid = userId,
                        userName = User.Identity.Name
                    }

                };
                return Json(userData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { error = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        public async Task<ActionResult> SaveCategory(Category category)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                if (String.IsNullOrEmpty(category.guid))
                {
                    category.guid = Guid.NewGuid().ToString();
                    await PhraseRepository.InsertCategory(category, userId);
                }
                else
                {
                    await PhraseRepository.UpdateCategory(category, userId);
                }

                return Json(category, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { error = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteCategory(Category category)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                await PhraseRepository.DeleteCategory(category, userId);

                return Json(category, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { error = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> LogThing()
        {
            try
            {
                var error = Request.Form["errorMsg"] + " line: " + Request.Form["lineNumber"] + " url: " + Request.Form["url"];
                await LogRepository.Log(error);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { error = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //public async Task<HttpResponseMessage> CustomAudioUpload()
        //{
        //    try
        //    {
        //        string root = HttpContext.Current.Server.MapPath("~/App_Data");
        //        var provider = new MultipartFormDataStreamProvider(root);

        //        // Read the form data.
        //        await Request.Content.ReadAsMultipartAsync(provider);

        //        //parse out the phrase data
        //        var phrase = Document.FromJson(provider.FormData.Get("phrase"));

        //        // Throw failure if there isnt a file or its not properly formatted
        //        if (provider.FileData.Count != 1 || String.IsNullOrEmpty(provider.FileData[0].Headers.ContentDisposition.FileName))
        //            throw new Exception("Upload should upload exactly one file.");

        //        //process the data
        //        phrase = await PhraseService.ProcessCustomAudioSource(
        //            phrase,
        //            provider.FileData[0]
        //        );

        //        return jsonResponse(phrase.ToJson(), HttpStatusCode.OK);
        //    }
        //    catch (Exception ex)
        //    {
        //        return jsonResponse(ex.ToString(), HttpStatusCode.InternalServerError);
        //    }
        //}



    }
}