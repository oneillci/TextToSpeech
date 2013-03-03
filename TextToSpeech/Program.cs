using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace TextToSpeech
{
    class Program
    {
        static void Main(string[] args)
        {
            //string fileName = string.Format(@".\{0}.wav", Guid.NewGuid());
            var fileName = @".\yep.wav";

            using (var ms = new MemoryStream())
            using (SpeechSynthesizer ss = new SpeechSynthesizer())
            {
                //ss.SetOutputToWaveFile(fileName);
                ss.SetOutputToWaveStream(ms);
                ss.Speak("yep, b, u, u, 9, x, 5");
                //ss.SetOutputToNull();
                //ss.Dispose();

                using (var fs = new FileStream(@".\yepyep.wav", FileMode.Create))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.CopyTo(fs);
                }
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
