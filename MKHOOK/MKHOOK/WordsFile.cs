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
            pathString = System.IO.Path.Combine(@"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\x86\Debug\words.txt");
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
            } else if (newWords == "Oem7" || newWords == "Oem1" || newWords == "OemQuestion")
            {    
            } else if (newWords == "Oemtilde"){
                words = words + "Ñ";
            } else if (newWords == "OemPeriod"){
                words = words + "\r\n";
            }
            else if (newWords.Length > 1)
            {
                Console.WriteLine(newWords);
            }
            else
            {
                words = words + newWords;
            }
            File.WriteAllText(pathString, words);
        }
    }
}
