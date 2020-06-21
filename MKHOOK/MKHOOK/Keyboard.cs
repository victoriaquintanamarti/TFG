using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MKHOOK
{
    /// <summary>
    /// Clase que recoge los parámetros que se van a medir del teclado para poder determinar si se está teniendo un comportamiento anómalo.
    /// </summary>
    public class Keyboard
    {
        /// <value> Número de teclas pulsadas </value>
        private int pressedKeys;
        /// <value> Número de veces que se ha pulsado la tecla ESC </value>
        private int scapeKey;
        /// <value> Número de veces que se ha pulsado dos teclas a la vez </value>
        private int twoKeyPressed;
        /// <value> Teclas que se han pulsado. </value>
        private string words;
        /// <value> Booleano para controlar si se han pulsado dos teclas a la vez. </value>
        private bool keydown = false;
        /// <value> Tiempo en el que se inicia la medición. </value>
        private DateTime startTime;
        /// <value> Tiempo que ha transcurrido. </value>
        private TimeSpan timeElapsed;

        /// <summary>
        /// Constructor de la clase que inicializa las variables.
        /// </summary>
        public Keyboard()
        {
            pressedKeys = 0;
            scapeKey = 0;
            twoKeyPressed = 0;
            words = "";

        }
        /// <summary>
        /// Constructor de la clase que inicializa las variables.
        /// </summary>
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
