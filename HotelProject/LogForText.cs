using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelProject
{
    public class LogForText : ILog
    {
        string Text { get; set; }
        public LogForText(string text)
        {
            Text = text;
        }
        public void Log(string text)
        {
            using (StreamWriter sw = new StreamWriter(text))
            {

                sw.WriteLine(text);

            }
        }
    }
}

