using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MKHOOK
{
    /// <summary>
    /// Clase que despliega un formulario que contiene un resumen con los parámetros que se están midiendo en cada momento 
    /// y en donde se establece el valor que puede tomar cada parámetro para considerarse anómalo. 
    /// Cuando algún parámetro tome el valor que se ha puesto como anómalo se pondrá en rojo. 
    /// En cambio si no es anómalo, aparecerá de color verde.
    /// </summary>
    class AlarmForm : Form
    {
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private TextBox textBox5;
        private TextBox textBox6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Button button1;
        private Label label1;
        private Events events;
        public event System.ComponentModel.ProgressChangedEventHandler LoadProgressChanged;

        private string keyPressedAlarm = "10";
        private string scapeKeyAlarm = "10";
        private string twoPressedKeysAlarm = "10";
        private string mouseClicksAlarm = "20";
        private string euclideanDistanceAlarm = "10000";
        private string mouseWheelAlarm = "20";
        private System.Timers.Timer timer;
        private PictureBox pictureBox3;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Label label15;
        private Label label16;
        private Label label17;
        private string outputJSON;

        public AlarmForm(Events events)
        {
            string docPath = @"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\x86\Debug";
            outputJSON = "{ \"keyPressedAlarm\":" + keyPressedAlarm + ",\"scapeKeyAlarm\":" + scapeKeyAlarm+ ",\"twoPressedKeysAlarm\":" + twoPressedKeysAlarm + ",\"mouseClicksAlarm\":" + mouseClicksAlarm + ",\"euclideanDistanceAlarm\":" + euclideanDistanceAlarm + ",\"mouseWheelAlarm\":" + mouseWheelAlarm + "}";
            File.WriteAllText(Path.Combine(docPath, "alarms.json"), outputJSON);
            this.events = events;
            InitializeComponent();
            timer = new System.Timers.Timer(11000);
            timer.Enabled = true;
            timer.Elapsed += new ElapsedEventHandler(timerElapsed);
            timer.Start();
            CheckForIllegalCrossThreadCalls = false;
        }
        private NotifyIcon create_notification(String text)
        {
            var notification = new System.Windows.Forms.NotifyIcon()
            {
                Visible = true,
                Icon = System.Drawing.SystemIcons.Exclamation,
                BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Warning,
                BalloonTipTitle = "Alerta",
                BalloonTipText = "Alerta de" + text,
            };
            return notification;
        }
        public void run_cmd()
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Users\Victoria\AppData\Local\Programs\Python\Python38-32\python.exe";
            start.Arguments = string.Format("{0} {1}", @"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\pruebaEmoTXT\grafica_mouse.py", "");
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (System.IO.StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
            }
        }
        private void timerElapsed(object sender, System.EventArgs e)
        {
            var notification = new System.Windows.Forms.NotifyIcon()
            {
                Visible = true,
                Icon = System.Drawing.SystemIcons.Exclamation,
                //BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Warning,
                BalloonTipTitle = "Alerta",
                BalloonTipText = "Alerta de",
            };

            label7.Text = Convert.ToString(events.getKeyboard().getPressedKeys());
            if (Convert.ToInt32(label7.Text) > Convert.ToInt32(keyPressedAlarm))
            {
                label7.ForeColor = Color.Red;
                notification = create_notification(" keyPressedAlarm");
                notification.ShowBalloonTip(1000);
                notification.Dispose();
            }
            else
            {
                label7.ForeColor = Color.Green;
            }
            label7.Refresh();
            label8.Text = Convert.ToString(events.getKeyboard().getScapeKey());
            if (Convert.ToInt32(label8.Text) > Convert.ToInt32(scapeKeyAlarm))
            {
                label8.ForeColor = Color.Red;
                notification = create_notification(" scapeKeyAlarm");
                notification.ShowBalloonTip(1000);
                notification.Dispose();
            }
            else
            {
                label8.ForeColor = Color.Green;
            }
            label8.Refresh();
            label9.Text = Convert.ToString(events.getKeyboard().getTwoPressedKeys());
            if (Convert.ToInt32(label9.Text) > Convert.ToInt32(twoPressedKeysAlarm))
            {
                label9.ForeColor = Color.Red;
                notification = create_notification(" twoPressedKeysAlarm");
                notification.ShowBalloonTip(1000);
                notification.Dispose();
            }
            else
            {
                label9.ForeColor = Color.Green;
            }
            label9.Refresh();
            label10.Text = Convert.ToString(events.getMouse().getClicks());
            if (Convert.ToInt32(label10.Text) > Convert.ToInt32(mouseClicksAlarm))
            {
                Console.WriteLine("ffffffffffffffffff" + label10.Text);
                Console.WriteLine("ffffffffffffffffff" + mouseClicksAlarm);
                label10.ForeColor = Color.Red;
                notification = create_notification(" mouseClicksAlarm");
                notification.ShowBalloonTip(1000);
                notification.Dispose();
            }
            else
            {
                label10.ForeColor = Color.Green;
            }
            label10.Refresh();
            label11.Text = Convert.ToString(events.getMouse().getSumDistances());
            if (Convert.ToDouble(label11.Text) > Convert.ToDouble(euclideanDistanceAlarm))
            {
                label11.ForeColor = Color.Red;
                notification = create_notification(" euclideanDistanceAlarm");
                notification.ShowBalloonTip(1000);
                notification.Dispose();
            }
            else
            {
                label11.ForeColor = Color.Green;
            }
            label11.Refresh();
            label12.Text = Convert.ToString(events.getMouse().getMouseWheel());
            if (Convert.ToInt32(label12.Text) > Convert.ToInt32(mouseWheelAlarm))
            {
                label12.ForeColor = Color.Red;
                notification = create_notification(" mouseWheelAlarm");
                notification.ShowBalloonTip(1000);
                //notification.Dispose();
            }
            else
            {
                label12.ForeColor = Color.Green;
            }
            label12.Refresh();
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                run_cmd();
            }).Start();
            ImageForm_Load();
            ImageForm_Load_mouse();
            ImageForm_Load_keyboard();

        }

        public void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlarmForm));
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(537, 270);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label6.Size = new System.Drawing.Size(99, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "MouseWheel Alarm";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(537, 238);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "EuclideanDistance Alarm";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(537, 205);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "MouseClicks Alarm";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(537, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "TwoPressedKeys Alarm";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(537, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "ScapeKey Alarm";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(537, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "PressedKeys Alarm";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(719, 102);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(91, 20);
            this.textBox1.TabIndex = 12;
            this.textBox1.Text = "10";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(719, 132);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(91, 20);
            this.textBox2.TabIndex = 13;
            this.textBox2.Text = "10";
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(719, 167);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(91, 20);
            this.textBox3.TabIndex = 14;
            this.textBox3.Text = "10";
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(719, 200);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(91, 20);
            this.textBox4.TabIndex = 17;
            this.textBox4.Text = "20";
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(719, 231);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(91, 20);
            this.textBox5.TabIndex = 16;
            this.textBox5.Text = "10000";
            this.textBox5.TextChanged += new System.EventHandler(this.textBox5_TextChanged);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(719, 263);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(91, 20);
            this.textBox6.TabIndex = 15;
            this.textBox6.Text = "20";
            this.textBox6.TextChanged += new System.EventHandler(this.textBox6_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(868, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "PressedKeys Alarm";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(869, 135);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "ScapeKey Alarm";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(869, 167);
            this.label9.Name = "label9";
            this.label9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label9.Size = new System.Drawing.Size(118, 13);
            this.label9.TabIndex = 31;
            this.label9.Text = "TwoPressedKeys Alarm";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(869, 200);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 13);
            this.label10.TabIndex = 30;
            this.label10.Text = "MouseClicks Alarm";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(869, 236);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(125, 13);
            this.label11.TabIndex = 29;
            this.label11.Text = "EuclideanDistance Alarm";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(868, 268);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(99, 13);
            this.label12.TabIndex = 28;
            this.label12.Text = "MouseWheel Alarm";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(869, 59);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 13);
            this.label13.TabIndex = 27;
            this.label13.Text = "Obtained Alarm";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(736, 59);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(33, 13);
            this.label14.TabIndex = 26;
            this.label14.Text = "Alarm";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(719, 311);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 32;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(1015, 383);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(451, 349);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 35;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(516, 383);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(451, 349);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 36;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(23, 383);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(451, 349);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 37;
            this.pictureBox2.TabStop = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(219, 758);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(39, 13);
            this.label15.TabIndex = 38;
            this.label15.Text = "Mouse";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(725, 758);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(50, 13);
            this.label16.TabIndex = 39;
            this.label16.Text = "Emotions";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(1233, 758);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(52, 13);
            this.label17.TabIndex = 40;
            this.label17.Text = "Keyboard";
            // 
            // AlarmForm
            // 
            this.ClientSize = new System.Drawing.Size(1489, 834);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AlarmForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private void ImageForm_Load()
        {
            var directory = new DirectoryInfo(@"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\x86\Debug\pictures");
            var myFile = (from f in directory.GetFiles()
                          orderby f.LastWriteTime descending
                          select f).First();
            pictureBox1.ImageLocation = myFile.FullName;
            var name = myFile.FullName;
            var dirPath = @"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\x86\Debug\pictures";
            foreach (string file in Directory.GetFiles(dirPath))
            {
                FileInfo fi = new FileInfo(file);
                if (fi.FullName != name)
                    fi.Delete();
            }
        }
        private void ImageForm_Load_mouse()
        {
            var directory = new DirectoryInfo(@"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\x86\Debug\mouse");
            var myFile = (from f in directory.GetFiles()
                          orderby f.LastWriteTime descending
                          select f).First();
            pictureBox2.ImageLocation = myFile.FullName;
            var name = myFile.FullName;
            var dirPath = @"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\x86\Debug\mouse";
            foreach (string file in Directory.GetFiles(dirPath))
            {
                FileInfo fi = new FileInfo(file);
                if (fi.FullName != name)
                    fi.Delete();
            }
        }
        private void ImageForm_Load_keyboard()
        {
            var directory = new DirectoryInfo(@"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\x86\Debug\keyboard");
            var myFile = (from f in directory.GetFiles()
                          orderby f.LastWriteTime descending
                          select f).First();
            pictureBox3.ImageLocation = myFile.FullName;
            var name = myFile.FullName;
            var dirPath = @"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\x86\Debug\keyboard";
            foreach (string file in Directory.GetFiles(dirPath))
            {
                FileInfo fi = new FileInfo(file);
                if (fi.FullName != name)
                    fi.Delete();
            }
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            keyPressedAlarm = textBox1.Text;
            string docPath = @"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\x86\Debug";
            outputJSON = "{ \"keyPressedAlarm\":" + keyPressedAlarm + ",\"scapeKeyAlarm\":" + scapeKeyAlarm + ",\"twoPressedKeysAlarm\":" + twoPressedKeysAlarm + ",\"mouseClicksAlarm\":" + mouseClicksAlarm + ",\"euclideanDistanceAlarm\":" + euclideanDistanceAlarm + ",\"mouseWheelAlarm\":" + mouseWheelAlarm + "}";
            File.WriteAllText(Path.Combine(docPath, "alarms.json"), outputJSON);

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            scapeKeyAlarm = textBox2.Text;
            string docPath = @"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\x86\Debug";
            outputJSON = "{ \"keyPressedAlarm\":" + keyPressedAlarm + ",\"scapeKeyAlarm\":" + scapeKeyAlarm + ",\"twoPressedKeysAlarm\":" + twoPressedKeysAlarm + ",\"mouseClicksAlarm\":" + mouseClicksAlarm + ",\"euclideanDistanceAlarm\":" + euclideanDistanceAlarm + ",\"mouseWheelAlarm\":" + mouseWheelAlarm + "}";
            File.WriteAllText(Path.Combine(docPath, "alarms.json"), outputJSON);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            twoPressedKeysAlarm = textBox3.Text;
            string docPath = @"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\x86\Debug";
            outputJSON = "{ \"keyPressedAlarm\":" + keyPressedAlarm + ",\"scapeKeyAlarm\":" + scapeKeyAlarm + ",\"twoPressedKeysAlarm\":" + twoPressedKeysAlarm + ",\"mouseClicksAlarm\":" + mouseClicksAlarm + ",\"euclideanDistanceAlarm\":" + euclideanDistanceAlarm + ",\"mouseWheelAlarm\":" + mouseWheelAlarm + "}";
            File.WriteAllText(Path.Combine(docPath, "alarms.json"), outputJSON);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            mouseClicksAlarm = textBox4.Text;
            string docPath = @"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\x86\Debug";
            outputJSON = "{ \"keyPressedAlarm\":" + keyPressedAlarm + ",\"scapeKeyAlarm\":" + scapeKeyAlarm + ",\"twoPressedKeysAlarm\":" + twoPressedKeysAlarm + ",\"mouseClicksAlarm\":" + mouseClicksAlarm + ",\"euclideanDistanceAlarm\":" + euclideanDistanceAlarm + ",\"mouseWheelAlarm\":" + mouseWheelAlarm + "}";
            File.WriteAllText(Path.Combine(docPath, "alarms.json"), outputJSON);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            euclideanDistanceAlarm = textBox5.Text;
            string docPath = @"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\x86\Debug";
            outputJSON = "{ \"keyPressedAlarm\":" + keyPressedAlarm + ",\"scapeKeyAlarm\":" + scapeKeyAlarm + ",\"twoPressedKeysAlarm\":" + twoPressedKeysAlarm + ",\"mouseClicksAlarm\":" + mouseClicksAlarm + ",\"euclideanDistanceAlarm\":" + euclideanDistanceAlarm + ",\"mouseWheelAlarm\":" + mouseWheelAlarm + "}";
            File.WriteAllText(Path.Combine(docPath, "alarms.json"), outputJSON);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            mouseWheelAlarm = textBox6.Text;
            string docPath = @"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\x86\Debug";
            outputJSON = "{ \"keyPressedAlarm\":" + keyPressedAlarm + ",\"scapeKeyAlarm\":" + scapeKeyAlarm + ",\"twoPressedKeysAlarm\":" + twoPressedKeysAlarm + ",\"mouseClicksAlarm\":" + mouseClicksAlarm + ",\"euclideanDistanceAlarm\":" + euclideanDistanceAlarm + ",\"mouseWheelAlarm\":" + mouseWheelAlarm + "}";
            File.WriteAllText(Path.Combine(docPath, "alarms.json"), outputJSON);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
