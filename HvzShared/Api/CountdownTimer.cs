using System;
using System.Collections.Generic;
using System.Text;

namespace Hvz.Api
{
    public class CountdownTimer
    {
        private int days;
        private int hours;
        private int minutes;
        private int seconds;

        private System.Timers.Timer timer;

        public Action<int, int, int, int> Callback;

        public CountdownTimer()
        {
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += TimerCallback;
        }

        public void Start(int days, int hours, int minutes, int seconds)
        {
            this.days = days;
            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
            timer.AutoReset = true;
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        private void TimerCallback(object sender, System.Timers.ElapsedEventArgs e)
        {
            seconds -= 1;
            if (seconds < 0)
            {
                seconds = 59;
                minutes -= 1;
            }

            if (minutes < 0)
            {
                minutes = 59;
                hours -= 1;
            }

            if (hours < 0)
            {
                hours = 23;
                days -= 1;
            }

            if (days < 0)
            {
                days = 0;
                hours = 0;
                minutes = 0;
                seconds = 0;
                timer.Stop();
            }

            Callback(days, hours, minutes, seconds);
        }
    }
}
