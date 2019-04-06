using MyVoiceMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVoiceMVC.Services.TextToSpeech
{
    public interface ITextToSpeech
    {
        Task<Phrase> GetTTS(Phrase phrase);
    }
}
