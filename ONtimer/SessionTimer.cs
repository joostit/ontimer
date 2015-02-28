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
        /// Gets of the timer value is zero. This property is not covered by INotifyPropertyChanged
        /// </summary>
        public bool IsZero
        {
            get
            {
                return (Minutes <= 0) && (Seconds <= 0);
            }
        }

        /// <summary>
        /// Gets or sets the mode, indication upward, or downward counting. This property is not covered by INotifyPropertyChanged
        /// </summary>
        public TimerModes Mode {get;set;}

        /// <summary>
        /// Returns if the timer is currently running. This property is not covered by INotifyPropertyChanged
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
            get { return timeValue.Seconds; }
            set
            {
                this.timeValue = new TimeSpan(0, this.timeValue.Minutes, value);
                validateTimerValue();
                notifyValuePropertiesChanged();
            }
        }

        /// <summary>
        /// Gets or sets the amount of minutes
        /// </summary>
        public int Minutes
        {
            get { return timeValue.Minutes; }
            set
            {
                this.timeValue = new TimeSpan(0, value, this.timeValue.Seconds);
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

        private TimeSpan timeValue;
        private int startTicks;

        private DispatcherTimer timer;
        
        /// <summary>
        /// Gets raised if a property value has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets raised if the timer expires
        /// </summary>
        public event EventHandler TimerExpired;

        /// <summary>
        /// Gets raised every second when the timer is running
        /// </summary>
        public event EventHandler OneSecondTick;

        /// <summary>
        /// Gets raised when the timer is started
        /// </summary>
        public event EventHandler TimerStarted;

        /// <summary>
        /// The value the timer had when it started
        /// </summary>
        private TimeSpan startValue = new TimeSpan();

        public SessionTimer()
        {
            timeValue = new TimeSpan(0, 00, 00);
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
            if (timeValue.TotalSeconds < 0)
            {
                timeValue = new TimeSpan(0, 0, 0);
            }
        }

        /// <summary>
        /// Starts the timer
        /// </summary>
        public void Start()
        {
            startTicks = Environment.TickCount;
            startValue = timeValue;
            timer.Start();
            if (TimerStarted != null)
            {
                TimerStarted(this, new EventArgs());
            }
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
            timeValue = startValue;
            startTicks = Environment.TickCount;
            if (timer.IsEnabled)
            {
                timer.Stop();
                timer.Start();
            }
            notifyValuePropertiesChanged();
        }

        /// <summary>
        /// Resets the timer to zero
        /// </summary>
        public void ResetToZero()
        {
            startValue = new TimeSpan();
            ResetToInitialValue();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            // Use system ticks for this calculation because the DispatcherTImer is inaccurate
            TimeSpan elapsed = TimeSpan.FromMilliseconds(Environment.TickCount - startTicks);

            switch (Mode)
            {
                case TimerModes.Up:
                    timeValue = elapsed;
                    break;
                case TimerModes.Down:
                    timeValue = startValue - elapsed;
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
                if (IsZero)
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
