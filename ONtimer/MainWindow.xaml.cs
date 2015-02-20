using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ONtimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SessionTimer timer = new SessionTimer();

        private DispatcherTimer doubleClickTimer = new DispatcherTimer();
        private int clickCount = 0;

        public SessionTimer Timer
        {
            get
            {
                return timer;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this.Timer;
            doubleClickTimer = new DispatcherTimer();
            doubleClickTimer.Tick += doubleClickTimerTick;
        }

        private void doubleClickTimerTick(object sender, EventArgs e)
        {
            applyClickAmount();
        }

        void applyClickAmount()
        {
            doubleClickTimer.Stop();

            if (clickCount == 1)
            {
                performSingleButtonClick();
            }
            if (clickCount == 2)
            {
                performDoubleButtonClick();
            }
            if (clickCount == 3)
            {
                performTripleButtonClick();
            }

            clickCount = 0;
        }


        private void performSingleButtonClick()
        {
            startStopTimer();
        }


        private void performDoubleButtonClick()
        {
            if (timer.IsRunning)
            {
                timer.Stop();
            }

            timer.ResetToInitialValue();
        }

        private void performTripleButtonClick()
        {
            if (timer.IsRunning)
            {
                timer.Stop();
            }

            timer.ResetToZero();
        }


        private void startStopResetButton_Click(object sender, RoutedEventArgs e)
        {
            clickCount++;

            if (clickCount == 1)
            {
                doubleClickTimer.Interval = new TimeSpan(0, 0, 0, 0, 250);
                doubleClickTimer.Start();
            }
            else
            {
                if (clickCount <= 3)
                {
                    doubleClickTimer.Stop();
                    doubleClickTimer.Start();
                }
                else
                {
                    doubleClickTimer.Stop();
                    clickCount = 0;
                }
            }
        }

        private void startStopTimer()
        {
            if (timer.IsRunning)
            {
                timer.Stop();
            }
            else
            {
                if ((timer.Minutes == 0) && (timer.Seconds == 0))
                {
                    timer.Mode = SessionTimer.TimerModes.Up;
                }
                else
                {
                    timer.Mode = SessionTimer.TimerModes.Down;
                }
                timer.Start();
            }
        }


        private void startStopResetButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (doubleClickTimer.IsEnabled)
            {
                applyClickAmount();
            }
        }

        private void minutesTenBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!timer.IsRunning)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    timer.Minutes += 10;
                }
                if (e.RightButton == MouseButtonState.Pressed)
                {
                    timer.Minutes -= 10;
                }
            }
        }

        private void minutesSingleBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!timer.IsRunning)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    timer.Minutes += 1;
                }
                if (e.RightButton == MouseButtonState.Pressed)
                {
                    timer.Minutes -= 1;
                }
            }
        }

        private void secondsTenBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!timer.IsRunning)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    timer.Seconds += 10;
                }
                if (e.RightButton == MouseButtonState.Pressed)
                {
                    timer.Seconds -= 10;
                }
            }
        }

        private void secondsSingleBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!timer.IsRunning)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    timer.Seconds += 1;
                }
                if (e.RightButton == MouseButtonState.Pressed)
                {
                    timer.Seconds -= 1;
                }
            }
        }

    }
}
