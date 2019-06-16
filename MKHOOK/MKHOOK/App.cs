using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MKHOOK
{
    class App : Form
    {
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;
        private Label label1;
        private TextBox textBox1;
        private Button button1;
        private System.ComponentModel.IContainer components;
        private Label label2;
        private TextBox textBox2;
        private Events events;
        private AlarmForm alarm;

        public App(Events events)
        {
            CreateNotifyicon();         
            this.events = events;
            alarm = new AlarmForm(events);
            
        }

        private void CreateNotifyicon()
        {

            this.components = new System.ComponentModel.Container();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();

            // Initialize menuItem1
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "Config";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);

            // Initialize menuItem2
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "Alarm";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);

            // Initialize menuItem3
            this.menuItem3.Index = 0;
            this.menuItem3.Text = "Train the model";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);

            // Initialize menuItem4
            this.menuItem4.Index = 0;
            this.menuItem4.Text = "Classification";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);

            // Initialize menuItem5
            this.menuItem5.Index = 0;
            this.menuItem5.Text = "Exit";
            this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);


            // Initialize contextMenu1
            this.contextMenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.menuItem1 });

            // Initialize contextMenu1
            this.contextMenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.menuItem2 });

            // Initialize contextMenu3
            this.contextMenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.menuItem3 });

            // Initialize contextMenu4
            this.contextMenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.menuItem4 });

            // Initialize contextMenu5
            this.contextMenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.menuItem5 });

            // Create the NotifyIcon.
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);

            // The Icon property sets the icon that will appear
            // in the systray for this application.
            notifyIcon1.Icon = new Icon("computer.ico");

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            notifyIcon1.ContextMenu = this.contextMenu1;

            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.
            notifyIcon1.Text = "MouseKeyHok";
            notifyIcon1.Visible = true;

            /*// Handle the DoubleClick event to activate the form.
            notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);*/

        }
        /*
        private void notifyIcon1_Click(object Sender, EventArgs e)
        {
            InitializeComponent();
            this.Show();
        }

        private void notifyIcon1_DoubleClick(object Sender, EventArgs e)
        {
            MessageBox.Show("Double clicked");
        }*/

        private void menuItem1_Click(object Sender, EventArgs e)
        {
            InitializeComponent();
            this.Show();
        }

        private void menuItem2_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            alarm.Show();
        }

        private void menuItem3_Click(object Sender, EventArgs e)
        {
            SentimentClassifier sentiment = new SentimentClassifier();
            Thread th1 = new Thread(new ThreadStart(sentiment.trainClassifier));
            th1.Start();
            th1.Join();

        }

        private void menuItem4_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            SentimentClassifier sentiment = new SentimentClassifier();
            Thread th1 = new Thread(new ThreadStart(sentiment.trainedClassifier));
            th1.Start();
            th1.Join();

        }

        private void menuItem5_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            SentimentClassifier sentiment = new SentimentClassifier();
            Thread th1 = new Thread(new ThreadStart(sentiment.trainedClassifier));
            th1.Start();
            th1.Join();
            Application.Exit();

        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sample Time per second";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(327, 80);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(91, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "10";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(217, 248);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(77, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Mouse samples per second";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(327, 140);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(91, 20);
            this.textBox2.TabIndex = 4;
            this.textBox2.Text = "10";
            // 
            // App
            // 
            this.ClientSize = new System.Drawing.Size(494, 324);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "App";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            events.setTime(textBox1.Text);
            events.setTimeMouse(textBox2.Text);
        }

        [STAThread]
        private static void Main()
        {
            Events events; 
            events = new Events();
            App app = new App(events);
            Application.Run(events);
        }

    } 
}
