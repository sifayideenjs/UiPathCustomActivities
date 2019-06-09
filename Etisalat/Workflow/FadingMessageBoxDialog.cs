using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Etisalat.CIT.OPS.Robotics
{
    public partial class FadingMessageBoxDialog : Form
    {
        public FadingMessageBoxDialog(string message, string title)
        {
            InitializeComponent();
            this.label_message.Text = message;
            this.Text = title;
            this.Opacity = 1.0;
            this.Activate();
            this.Focus();
        }

        private void FadingMessageBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            //FadeOut(this, 0);

            //int duration = 5000;
            //int steps = 100;
            //Timer timer = new Timer();
            //timer.Interval = duration / steps;

            //int currentStep = 0;
            //timer.Tick += (arg1, arg2) =>
            //{
            //    Opacity = ((double)currentStep) / steps;
            //    currentStep++;

            //    if (currentStep >= steps)
            //    {
            //        timer.Stop();
            //        timer.Dispose();
            //    }
            //};

            //timer.Start();

        }

        private async void FadeIn(Form o, int interval = 80)
        {
            while (o.Opacity < 1.0)
            {
                await Task.Delay(interval);
                o.Opacity += 0.05;
            }
            o.Opacity = 1;
        }

        private async void FadeOut(Form o, int interval = 80)
        {
            while (o.Opacity > 0.0)
            {
                await Task.Delay(interval);
                o.Opacity -= 0.05;
            }
            o.Opacity = 0;
        }

        public void HideMe()
        {
            int duration = 5000;
            int steps = 100;
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = duration / steps;

            int currentStep = 100;
            timer.Tick += (arg1, arg2) =>
            {
                Opacity = ((double)currentStep) / steps;
                currentStep--;

                if (currentStep <= 0)
                {
                    timer.Stop();
                    timer.Dispose();
                    this.Close();
                    this.Dispose();
                }
            };

            timer.Start();
        }

        private void FadingMessageBoxDialog_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(HideMe));
            thread.Start();
        }
    }
}
