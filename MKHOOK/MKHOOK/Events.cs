using System;
using System.ComponentModel;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using System.Timers;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MKHOOK
{
    public class Events : Form
    {

        private IKeyboardMouseEvents m_Events;
        private Keyboard keyboard;
        private Mouse mouse;
        private System.Timers.Timer timer;
        private DateTime startTime;
        private TimeSpan timeElapsed;
        private string intervalToStr;
        private double timeSample = 100;
        private string time = "10";
        private double timeMouse = 10;
        private string outputJSON;
        private string pathString = "";
        private JsonClass jsonObject;
        private bool firstTime = true;
        private bool noActivity = false;
        private string mouseSample = "10";
        private System.Timers.Timer mtimer;
        private WordsFile wordsFile;
        private int a = 0;
        private bool pressedKey = false;
        string workingDirectory = Environment.CurrentDirectory;
        public Events()
        {
            keyboard = new Keyboard();
            mouse = new Mouse();
            wordsFile = new WordsFile(this);

            ShowInTaskbar = false;
            Opacity = 0;

            SubscribeGlobal();
            FormClosing += Main_Closing;

            timer = new System.Timers.Timer(1000);
            timer.Enabled = true;
            timer.Elapsed += new ElapsedEventHandler(timerElapsed);
            timer.Start();
            mtimer = new System.Timers.Timer(timeSample);
            mtimer.Enabled = true;
            mtimer.Elapsed += new ElapsedEventHandler(mtimerElapsed);
            mtimer.Start();
            startTime = DateTime.Now;
            Console.WriteLine("Python Starting2");
        }
        private void mtimerElapsed(object sender, System.EventArgs e)
        {
            mouse.euclideanDistance();
            mouse.setPreviousPositionMouse(mouse.getX(), mouse.getY());
            a++;
        }
            private void timerElapsed(object sender, System.EventArgs e)
        {
            timeElapsed = DateTime.Now - startTime;
            intervalToStr = timeElapsed.ToString();

            string output = $"{(int)timeElapsed.TotalMinutes}:{timeElapsed.Seconds:00}";
            Console.WriteLine(output);
            Console.WriteLine(a);
            if ((System.Convert.ToInt32($"{timeElapsed.Seconds:00}") % System.Convert.ToInt32(time) == 0) && (System.Convert.ToInt32($"{timeElapsed.Seconds:00}") != 0))
            {
                if (firstTime)
                {
                    firstTime = false;
                    string fileName = "infoActivity-" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".json";
                    pathString = System.IO.Path.Combine(workingDirectory + "/infoActivity", fileName);
                    using (System.IO.FileStream fs = System.IO.File.Create(pathString))
                    { }
                    Console.WriteLine("Path to my file: {0}\n", pathString);
                    jsonObject = new JsonClass()
                    {
                        TimeActivityPerSecond = time,
                        MouseSamplePerSecond = mouseSample

                    };

                    jsonObject.Activity = new List<ActivityStats>();
                }
                else
                {
                    noActivity = checkJson();
                }
                ActivityStats activity = new ActivityStats();
                activity.Time = new TimeStats()
                {
                    TimeElapsed = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),

                };
                activity.Keyboard = new KeyboardStats()
                {
                    PressedKeys = keyboard.getPressedKeys(),
                    BackSpaceKey = keyboard.getbackSpaceKey(),
                    TwoPressedKeys = keyboard.getTwoPressedKeys()

                };
                activity.Mouse = new MouseStats()
                {
                    MouseClicks = mouse.getClicks(),
                    EuclideanDistance = mouse.getSumDistances(),
                    MouseWheel = mouse.getMouseWheel()
                };
                jsonObject.Activity.Add(activity);
                outputJSON = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                if (!noActivity)
                {
                    File.WriteAllText(pathString, outputJSON);
                    Console.WriteLine("Escrito JSON");

                }

                //RESET

                keyboard.setPressedKeys(0);
                keyboard.setbackSpaceKey(0);
                keyboard.setTwoPressedKeys(0);
                mouse.setClicks(0);
                mouse.setSumDistances(0);
                mouse.setChangeMouseWheel(0);

            }
        }

        public string getWords()
        {
            return keyboard.getWords();
        }
        public void setTime(string time)
        {
            Console.WriteLine("Cambiado time");
            this.time = time;
            firstTime = true;
        }

        public void setTimeMouse(string time)
        {
            mouseSample = time;
            Console.WriteLine("Cambiado timeMouse");
            this.timeMouse = Convert.ToDouble(time);
            timeSample = 1000 / timeMouse;
            mtimer.Stop();
            mtimer.Interval = timeSample;
            mtimer.Start();
            firstTime = true;
        }

        public Keyboard getKeyboard()
        {
            return keyboard;
        }
        public Mouse getMouse()
        {
            return mouse;
        }
        public bool checkJson()
        {
            if ((keyboard.getPressedKeys() == 0) &&
                (keyboard.getbackSpaceKey() == 0) &&
                (keyboard.getTwoPressedKeys() == 0) &&
                (mouse.getClicks() == 0) &&
                (mouse.getSumDistances() == 0) &&
                (mouse.getMouseWheel() == 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void Main_Closing(object sender, CancelEventArgs e)
        {
            Unsubscribe();
        }

        private void SubscribeApplication()
        {
            Unsubscribe();
            Subscribe(Hook.AppEvents());
        }

        private void SubscribeGlobal()
        {
            Unsubscribe();
            Subscribe(Hook.GlobalEvents());
        }

        private void Subscribe(IKeyboardMouseEvents events)
        {
            m_Events = events;
            m_Events.KeyDown += OnKeyDown;
            m_Events.KeyUp += OnKeyUp;

            m_Events.MouseUp += OnMouseUp;
            m_Events.MouseClick += OnMouseClick;
            m_Events.MouseDoubleClick += OnMouseDoubleClick;

            m_Events.MouseMove += HookManager_MouseMove;


            m_Events.MouseWheelExt += HookManager_MouseWheelExt;

        }

        private void Unsubscribe()
        {
            if (m_Events == null) return;
            m_Events.KeyDown -= OnKeyDown;
            m_Events.KeyUp -= OnKeyUp;

            m_Events.MouseUp -= OnMouseUp;
            m_Events.MouseClick -= OnMouseClick;
            m_Events.MouseDoubleClick -= OnMouseDoubleClick;

            m_Events.MouseMove -= HookManager_MouseMove;


            m_Events.MouseWheelExt -= HookManager_MouseWheelExt;

            m_Events.Dispose();
            m_Events = null;
        }

        private void HookManager_Supress(object sender, MouseEventExtArgs e)
        {
            /*if (e.Button != MouseButtons.Right)
            {
                Console.WriteLine(string.Format("MouseDown \t\t {0}\n", e.Button));
                return;
            }

            Console.WriteLine(string.Format("MouseDown \t\t {0} Suppressed\n", e.Button));
            e.Handled = true;*/
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            keyboard.setPressedKeys(keyboard.getPressedKeys() + 1);
            //keyboard.setWords(keyboard.getWords() + e.KeyCode.ToString());
            keyboard.isbackSpaceKey(e);
            keyboard.keyDown();
            wordsFile.write(e.KeyCode.ToString(),false);
            pressedKey = true;

        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            keyboard.keyUp();
        }

        private void HookManager_MouseMove(object sender, MouseEventArgs e)
        {

            mouse.setPositionMouse(e.X, e.Y);
            //Console.WriteLine(string.Format("x={0:0000}; y={1:0000}", e.X, e.Y));

        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            //Console.WriteLine(string.Format("MouseDown \t\t {0}\n", e.Button));
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            //Console.WriteLine(string.Format("MouseUp \t\t {0}\n", e.Button));
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            mouse.setClicks(mouse.getClicks() + 1);
            if (pressedKey)
            {
                wordsFile.write("", true);
                pressedKey = false;
            }
        }

        private void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            mouse.setClicks(mouse.getClicks() + 2);
        }

        private void OnMouseDragStarted(object sender, MouseEventArgs e)
        {
            //Console.WriteLine("MouseDragStarted\n");
        }

        private void OnMouseDragFinished(object sender, MouseEventArgs e)
        {
            //Console.WriteLine("MouseDragFinished\n");
        }

        private void HookManager_MouseWheel(object sender, MouseEventArgs e)
        {
            //Console.WriteLine(string.Format("Wheel={0:000}", e.Delta));

            mouse.setMouseWheel(e.Delta);

        }

        private void HookManager_MouseWheelExt(object sender, MouseEventExtArgs e)
        {
            /*Console.WriteLine(string.Format("Wheel={0:000}", e.Delta));
            Console.WriteLine("Mouse Wheel Move Suppressed.\n");*/
            e.Handled = true;
            mouse.setMouseWheel(e.Delta);
        }

    }
}
