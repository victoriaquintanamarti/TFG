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
        private string words = "";
        private int specialCharacters = 0;

        public WordsFile(Events events)
        {
            this.events = events;
            string fileName = "words-" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".json";
            pathString = System.IO.Path.Combine(@"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\Debug", fileName);
            using (System.IO.FileStream fs = System.IO.File.Create(pathString))
            { }
        }
        public void write(string newWords)
        {
            if (newWords == "Space")
            {
                words = words + " ";
            } else if (newWords == "Back")
            {
                if (words.Length != 0)
                    words = words.Remove(words.Length - 1, 1);
            } else if (newWords == "Return")
            {
                words = words + "\r\n";
            }
            else if (newWords.Length > 1)
            {
                specialCharacters++;
            }
            else
            {
                words = words + newWords;
            }
            File.WriteAllText(pathString, words);
        }
    }
}
