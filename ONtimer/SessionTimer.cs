using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ONtimer
{
    public class SessionTimer : INotifyPropertyChanged
    {

        /// <summary>
        /// Returns if the timer is currently running
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return timer.IsEnabled;
            }
        }


        /// <summary>
        /// Gets or sets the amount of seconds
        /// </summary>
        public int Seconds
        {
            get { return value.Seconds; }
            set
            {
                this.value = new TimeSpan(0, this.value.Minutes, value);
                validateTimerValue();
                notifyValuePropertiesChanged();
            }
        }

        /// <summary>
        /// Gets or sets the amount of minutes
        /// </summary>
        public int Minutes
        {
            get { return value.Minutes; }
            set
            {
                this.value = new TimeSpan(0, value, this.value.Seconds);
                validateTimerValue();
                notifyValuePropertiesChanged();
            }
        }

        private void notifyValuePropertiesChanged()
        {
            NotifyPropertyChanged("Seconds");
            NotifyPropertyChanged("SecondsTens");
            NotifyPropertyChanged("SecondsSingle");
            NotifyPropertyChanged("Minutes");
            NotifyPropertyChanged("MinuteSingle");
            NotifyPropertyChanged("MinuteTens");
        }

        public String MinuteTens
        {
            get
            {
                string minutesString = (Minutes / 10).ToString();
                return minutesString[minutesString.Length - 1].ToString();
            }
        }

        public String MinuteSingle
        {
            get
            {
                string minutesString = Minutes.ToString();
                return minutesString[minutesString.Length - 1].ToString();
            }
        }

        public String SecondsTens
        {
            get
            {
                string secondsString = (Seconds / 10).ToString();
                return secondsString[secondsString.Length - 1].ToString();
            }
        }

        public String SecondsSingle
        {
            get
            {
                string secondsString = Seconds.ToString();
                return secondsString[secondsString.Length - 1].ToString();
            }
        }

        private TimeSpan value;

        private DispatcherTimer timer;


        public event PropertyChangedEventHandler PropertyChanged;


        public SessionTimer()
        {
            value = new TimeSpan(0, 00, 00);
            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
        }

        private void validateTimerValue()
        {
            if (value.TotalSeconds < 0)
            {
                value = new TimeSpan(0, 0, 0);
            }
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void Reset()
        {
            value = new TimeSpan();
            notifyValuePropertiesChanged();
        }


        void timer_Tick(object sender, EventArgs e)
        {
            value += new TimeSpan(0, 0, 1);
            notifyValuePropertiesChanged();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
