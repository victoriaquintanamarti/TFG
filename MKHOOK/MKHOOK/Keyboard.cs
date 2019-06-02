using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MKHOOK
{
    public class Keyboard
    {
        private int pressedKeys;
        private int scapeKey;
        private int twoKeyPressed;
        private string words;
        private bool keydown = false;
        private DateTime startTime;
        private TimeSpan timeElapsed;

        public Keyboard()
        {
            pressedKeys = 0;
            scapeKey = 0;
            twoKeyPressed = 0;
            words = "";

        }
        public void isScapeKey(KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Back")
            {
                scapeKey++;
            }
        }
        public void keyDown()
        {
            if (keydown)
            {
                timeElapsed = DateTime.Now - startTime;
                if (timeElapsed.Milliseconds < 5)
                    twoKeyPressed++;
            }
            keydown = true;
            startTime = DateTime.Now;
        }
        public void keyUp()
        {
            keydown = false;
        }
        public int getTwoPressedKeys()
        {
            return twoKeyPressed;
        }
        public void setTwoPressedKeys(int twoKeyPressed)
        {
            this.twoKeyPressed = twoKeyPressed;
        }
        public void setPressedKeys(int pressedKeys)
        {
            this.pressedKeys = pressedKeys;
        }
        public void setScapeKey(int scapeKey)
        {
            this.scapeKey = scapeKey;
        }
        public void setWords(string words)
        {
            this.words = words;
        }
        public int getPressedKeys()
        {
            return pressedKeys;
        }
        public int getScapeKey()
        {
            return scapeKey;
        }
        public string getWords()
        {
            return words;
        }
        public void stats()
        {
            Console.WriteLine("Pressed keys: " + getPressedKeys());
            Console.WriteLine("Scape keys: " + getScapeKey());
            Console.WriteLine("Words: " + getWords());
            Console.WriteLine("Two pressed keys: " + getTwoPressedKeys());
        }
    }
}
