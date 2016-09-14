using MyVoice.Models;
using MyVoice.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyVoice.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ViewResult Index()
        {
            //WatsonService.GetTTS("This is a simple attempt to get text to speach working!");
            ViewBag.Title = "MyVoice";

            return View();
        }
        
    }
}