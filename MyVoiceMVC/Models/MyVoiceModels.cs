using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyVoiceMVC.Models
{
    public class Phrase
    {
        public string guid { get; set; } = String.Empty;
        public string text { get; set; } = String.Empty;
        public string userGuid { get; set; } = String.Empty;
        public IEnumerable<string> categories { get; set; } = new List<string>();
        public DateTime date { get; set; } = new DateTime();
        public string customAudioName { get; set; } = null;
        public bool forceRefresh { get; set; } = false;
        public bool temporary { get; set; } = false;

    }

    public class Category
    {
        public string guid { get; set; } = String.Empty;
        public string userGuid { get; set; } = String.Empty;
        public string name { get; set; } = String.Empty;
        public string icon { get; set; } = String.Empty;
    }    
}