using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Drawing;

namespace MKHOOK
{
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

        private string keyPressedAlarm = "10";
        private string scapeKeyAlarm = "10";
        private string twoPressedKeysAlarm = "10";
        private string mouseClicksAlarm = "10";
        private string euclideanDistanceAlarm = "10";
        private string mouseWheelAlarm = "10";

        private System.Timers.Timer timer;

        public AlarmForm(Events events)
        {
            this.events = events;
            InitializeComponent();
            timer = new System.Timers.Timer(10000);
            timer.Enabled = true;
            timer.Elapsed += new ElapsedEventHandler(timerElapsed);
            timer.Start();
            CheckForIllegalCrossThreadCalls = false;
        }
        private void timerElapsed(object sender, System.EventArgs e)
        {

            label7.Text = Convert.ToString(events.getKeyboard().getPressedKeys());
            if (Convert.ToInt32(label7.Text) > Convert.ToInt32(keyPressedAlarm))
            {
                label7.ForeColor = Color.Red;
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
            }
            else
            {
                label12.ForeColor = Color.Green;
            }
            label12.Refresh();
        }
        public void InitializeComponent()
        {
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
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(154, 242);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label6.Size = new System.Drawing.Size(130, 17);
            this.label6.TabIndex = 11;
            this.label6.Text = "MouseWheel Alarm";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(154, 210);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(165, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "EuclideanDistance Alarm";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(154, 177);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "MouseClicks Alarm";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(154, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "TwoPressedKeys Alarm";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(154, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "ScapeKey Alarm";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(154, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "PressedKeys Alarm";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(408, 76);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(91, 22);
            this.textBox1.TabIndex = 12;
            this.textBox1.Text = "10";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(408, 106);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(91, 22);
            this.textBox2.TabIndex = 13;
            this.textBox2.Text = "10";
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(408, 141);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(91, 22);
            this.textBox3.TabIndex = 14;
            this.textBox3.Text = "10";
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(408, 174);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(91, 22);
            this.textBox4.TabIndex = 17;
            this.textBox4.Text = "10";
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(408, 205);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(91, 22);
            this.textBox5.TabIndex = 16;
            this.textBox5.Text = "10";
            this.textBox5.TextChanged += new System.EventHandler(this.textBox5_TextChanged);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(408, 237);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(91, 22);
            this.textBox6.TabIndex = 15;
            this.textBox6.Text = "10";
            this.textBox6.TextChanged += new System.EventHandler(this.textBox6_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(557, 76);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(131, 17);
            this.label7.TabIndex = 24;
            this.label7.Text = "PressedKeys Alarm";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(558, 109);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(112, 17);
            this.label8.TabIndex = 25;
            this.label8.Text = "ScapeKey Alarm";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(558, 141);
            this.label9.Name = "label9";
            this.label9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label9.Size = new System.Drawing.Size(157, 17);
            this.label9.TabIndex = 31;
            this.label9.Text = "TwoPressedKeys Alarm";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(558, 174);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(126, 17);
            this.label10.TabIndex = 30;
            this.label10.Text = "MouseClicks Alarm";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(558, 210);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(165, 17);
            this.label11.TabIndex = 29;
            this.label11.Text = "EuclideanDistance Alarm";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(557, 242);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(130, 17);
            this.label12.TabIndex = 28;
            this.label12.Text = "MouseWheel Alarm";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(558, 33);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(106, 17);
            this.label13.TabIndex = 27;
            this.label13.Text = "Obtained Alarm";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(425, 33);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(44, 17);
            this.label14.TabIndex = 26;
            this.label14.Text = "Alarm";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(369, 320);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 32;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AlarmForm
            // 
            this.ClientSize = new System.Drawing.Size(802, 395);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            keyPressedAlarm = textBox1.Text;

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            scapeKeyAlarm = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            twoPressedKeysAlarm = textBox3.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            mouseClicksAlarm = textBox4.Text;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            euclideanDistanceAlarm = textBox5.Text;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            mouseWheelAlarm = textBox6.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
