using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class HomeController : AsyncController
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            DoCaptcha();
            return View();
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
      

        public ActionResult GetCaptchaAudio()
        {
            //return Json(await GetStuff(), JsonRequestBehavior.AllowGet);
            //var fileInfo = new FileInfo("captcha.wav");
            //string fileName = string.Format(@".\{0}.wav", Guid.NewGuid());
            byte[] b;
            string s = @"C:\Users\Ciaran\Documents\GitHub\TextToSpeech\mvc.wav";
            
            using (var ms = new MemoryStream())
                using (var ss = new SpeechSynthesizer())
                {                    
                    
                    ss.SetOutputToWaveStream(ms);
                    //ss.
                    //ss.SetOutputToAudioStream(ms, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 88200, 16, 1, 16000, 2, null));
                    //await Task.Run(() => ss.SetOutputToAudioStream(ms, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 88200, 16, 1, 16000, 2, null)));
                    //ss.SetOutputToWaveFile(fileName);

                    //ss.SetOutputToWaveFile(fileName);
                    var text = string.Join(",", Session["captcha"].ToString().AsEnumerable());
                    ss.Speak(text);
                    ss.SetOutputToNull();
                    ss.Dispose();
                    
                    using (var fs = new FileStream(s, FileMode.Create))
                    {
                        ms.Seek(0, SeekOrigin.Begin);
                        ms.CopyTo(fs);
                    }
                    //ms.Seek(0, SeekOrigin.Begin);
                    //b = new byte[ms.Length];
                    //ms.Read(b, 0, (int)ms.Length);
                        
                    //ms.Close();
                    //ms.Dispose();
                    //return new FileStreamResult(ms, "audio/wav ");
                }
            using (var fs = new FileStream(s, FileMode.Open, FileAccess.Read))
            {
                fs.Seek(0, SeekOrigin.Begin);
                b = new byte[fs.Length];
                fs.Read(b, 0, (int)fs.Length);
                //var r = new FileContentResult(b, ctype);
                //r.FileDownloadName = "a.wav";
                return File(b, "audio/wav", "mvc.wav");
            }
            //return File(b, "audio/wav", "d.wav");

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
                                              .Select(x => x[rnd.Next(x.Length)])
                                              .ToArray());
            return result;
        }

        public ActionResult About()
        {
            return View();
        }
    }
}