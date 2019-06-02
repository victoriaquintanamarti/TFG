using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKHOOK
{
    public class WordsFile
    {
        private string pathString;
        private Events events;

        public WordsFile(Events events)
        {
            this.events = events;
            string fileName = "words-" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".json";
            pathString = System.IO.Path.Combine(@"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\Debug", fileName);
            using (System.IO.FileStream fs = System.IO.File.Create(pathString))
            { }
        }
        public void write()
        {
            File.WriteAllText(pathString, events.getWords());
        }
    }
}
