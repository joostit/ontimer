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
            doubleClickTimer.Tick += doubleClickTimer_Tick;
        }

        void doubleClickTimer_Tick(object sender, EventArgs e)
        {
            performSingleButtonClick();
        }


        private void performSingleButtonClick()
        {
            doubleClickTimer.Stop();
            clickCount = 0;
            startStopTimer();
        }


        private void startStopTimer()
        {
            if (timer.IsRunning)
            {
                timer.Stop();
            }
            else
            {
                timer.Start();
            }
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
                doubleClickTimer.Stop();
                clickCount = 0;
                resetTimer();
            }
        }


        private void resetTimer()
        {
            if (timer.IsRunning)
            {
                timer.Stop();
            }

            timer.Reset();
        }

        private void startStopResetButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (doubleClickTimer.IsEnabled)
            {
                performSingleButtonClick();
            }
        }

        private void minutesTenBox_MouseDown(object sender, MouseButtonEventArgs e)
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
