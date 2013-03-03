using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CaptchaToSpeech.Controllers
{
    public class HomeController :  Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            DoCaptcha();
            return View();
        }
  
        private void DoCaptcha()
        {
            var captcha = GetCaptcha();
            Session["captcha"] = captcha;
            ViewBag.Captcha = captcha;
        }
  
        private string GetCaptcha()
        {
            var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var rnd = new Random();
            var result = new string(Enumerable.Repeat(chars, 6)
                                   .Select(x => x[rnd.Next(x.Length)]).ToArray());
            return result;

        }

        public ActionResult GetCaptchaAudio()
        {
            //return Json(await GetStuff(), JsonRequestBehavior.AllowGet);
            //var fileInfo = new FileInfo("captcha.wav");
            //string fileName = string.Format(@".\{0}.wav", Guid.NewGuid());

            using (var ms = new MemoryStream())
            using (var ss = new SpeechSynthesizer())
            {
                ss.SetOutputToWaveStream(ms);
                //ss.SetOutputToAudioStream(ms, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 88200, 16, 1, 16000, 2, null));
                //await Task.Run(() => ss.SetOutputToAudioStream(ms, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 88200, 16, 1, 16000, 2, null)));
                //ss.SetOutputToWaveFile(fileName);

                //ss.SetOutputToWaveFile(fileName);
                var text = string.Join(",", Session["captcha"].ToString().AsEnumerable());
                ss.Speak(text);
                ss.SetOutputToNull();
                ss.Dispose();
                return new FileStreamResult(ms, "audio/wav ");
                return null;
            }
        }

        public ActionResult DownloadFile()
        {
            string ctype = "audio/wav";
            var filename = @"C:\Users\Ciaran\Documents\GitHub\TextToSpeech\CaptchaToSpeech\bin\yepyep.wav";

            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                fs.Seek(0, SeekOrigin.Begin);
                byte[] b = new byte[fs.Length];
                fs.Read(b, 0, (int)fs.Length);
                //var r = new FileContentResult(b, ctype);
                //r.FileDownloadName = "a.wav";
                return File(b, ctype, "c.wav");
                //return r;
                //return this.File(fs, "audio/wav", "a.wav");
                return new FileStreamResult(fs, "audio/wav");
            }            
        }

        private async Task<string> GetStuff()
        {
            Thread.Sleep(3000);
            return "yep";
        }

        [HttpPost]
        public ActionResult Index(string captcha)
        {
            if (!string.Equals(captcha, Session["captcha"].ToString(), StringComparison.OrdinalIgnoreCase))
                ModelState.AddModelError("captcha", "Incorrect Captcha");
            else
                ModelState.AddModelError("", "Correct");
            DoCaptcha();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
