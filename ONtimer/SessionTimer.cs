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
        /// Defines timer modes
        /// </summary>
        public enum TimerModes
        {
            /// <summary>
            /// Counting upward
            /// </summary>
            Up,

            /// <summary>
            /// Counting downward
            /// </summary>
            Down
        }

        /// <summary>
        /// Gets or sets the mode, indication upward, or downward counting
        /// </summary>
        public TimerModes Mode {get;set;}

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
        
        /// <summary>
        /// Gets the string representation of the ten-units for the minute value
        /// </summary>
        public String MinuteTens
        {
            get
            {
                string minutesString = (Minutes / 10).ToString();
                return minutesString[minutesString.Length - 1].ToString();
            }
        }

        /// <summary>
        /// Gets the string representation of the single-units for the minute value
        /// </summary>
        public String MinuteSingle
        {
            get
            {
                string minutesString = Minutes.ToString();
                return minutesString[minutesString.Length - 1].ToString();
            }
        }

        /// <summary>
        /// Gets the string representation of the ten-units for the seconds value
        /// </summary>
        public String SecondsTens
        {
            get
            {
                string secondsString = (Seconds / 10).ToString();
                return secondsString[secondsString.Length - 1].ToString();
            }
        }

        /// <summary>
        /// Gets the string representation of the single-units for the seconds value
        /// </summary>
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

        public event EventHandler TimerExpired;

        public event EventHandler OneSecondTick;

        /// <summary>
        /// The value the timer had when it started
        /// </summary>
        private TimeSpan startValue = new TimeSpan();

        public SessionTimer()
        {
            value = new TimeSpan(0, 00, 00);
            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
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

        /// <summary>
        /// Ensures the current timer value is correct
        /// </summary>
        private void validateTimerValue()
        {
            if (value.TotalSeconds < 0)
            {
                value = new TimeSpan(0, 0, 0);
            }
        }


        /// <summary>
        /// Starts the timer
        /// </summary>
        public void Start()
        {
            startValue = value;
            timer.Start();
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        public void Stop()
        {
            timer.Stop();
        }

        /// <summary>
        /// Resets the timer to the value at which it was started
        /// </summary>
        public void ResetToInitialValue()
        {
            value = startValue;
            notifyValuePropertiesChanged();
        }

        /// <summary>
        /// Resets the timer to zero
        /// </summary>
        public void ResetToZero()
        {
            startValue = new TimeSpan();
            value = new TimeSpan();
            notifyValuePropertiesChanged();
        }


        void timer_Tick(object sender, EventArgs e)
        {
            switch (Mode)
            {
                case TimerModes.Up:
                    value += new TimeSpan(0, 0, 1);
                    break;
                case TimerModes.Down:
                    value -= new TimeSpan(0, 0, 1);
                    break;
                default:
                    throw new Exception("Unknown TimerMode: " + Mode.ToString());
            }

            validateTimerValue();
            
            notifyValuePropertiesChanged();

            if (OneSecondTick != null)
            {
                OneSecondTick(this, new EventArgs());
            }

            if (Mode == TimerModes.Down)
            {
                if ((Minutes <= 0) && (Seconds <= 0))
                {
                    Stop();
                    raiseTimerExpired();
                }
            }
        }


        private void raiseTimerExpired()
        {
            if (TimerExpired != null)
            {
                TimerExpired(this, new EventArgs());
            }
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
