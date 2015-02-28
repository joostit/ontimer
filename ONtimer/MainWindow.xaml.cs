using ONtimer.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ONtimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private SessionTimer timer = new SessionTimer();
        private DispatcherTimer doubleClickTimer = new DispatcherTimer();
        private int clickCount = 0;
        private TimeSpan mouseCursorTimeout = new  TimeSpan(0, 0, 5);
        private DispatcherTimer mouseHideTimer = new DispatcherTimer();
        private double clockBorderThickness = 5;

        /// <summary>
        /// Holds the digit index of manual numeric input. Is -1 when nu input is active
        /// </summary>
        private int manualInputDigitIndex = -1;

        private bool IsFullscreen
        {
            get
            {
                return this.WindowState == System.Windows.WindowState.Maximized;
            }
            set
            {
                if (value == true) // Go fullscreen
                {
                    this.WindowStyle = System.Windows.WindowStyle.None;
                    this.WindowState = System.Windows.WindowState.Maximized;
                    mouseHideTimer.Start();
                }
                else
                {
                    this.WindowStyle = System.Windows.WindowStyle.None;
                    this.WindowState = System.Windows.WindowState.Normal;
                    Mouse.OverrideCursor = null;
                    mouseHideTimer.Stop();
                }
                
            }
        }

        public SessionTimer Timer
        {
            get
            {
                return timer;
            }
        }


        /// <summary>
        /// Gets the orange border thickness
        /// </summary>
        public double ClockBorderThickness
        {
            get { return clockBorderThickness; }
            set
            {
                clockBorderThickness = value;
                raisePropertyChanged("ClockBorderThickness");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;;
            doubleClickTimer = new DispatcherTimer();
            timer.TimerExpired += timer_TimerExpired;
            timer.OneSecondTick += timer_OneSecondTick;
            timer.TimerStarted += timer_TimerStarted;
            doubleClickTimer.Tick += doubleClickTimerTick;
            this.SizeChanged += MainWindow_SizeChanged;
            mouseHideTimer.Interval = mouseCursorTimeout;
            mouseHideTimer.Tick += mouseHideTimer_Tick;
        }


        void timer_OneSecondTick(object sender, EventArgs e)
        {
            ExecutionStateUtils.NotifyScreenInUse();
        }

        void timer_TimerExpired(object sender, EventArgs e)
        {
            Storyboard blinkAnimation = (Storyboard)FindResource("clockBlink");
            blinkAnimation.Begin(this);
        }

        void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double lowest = this.ActualHeight < this.ActualWidth ? this.ActualHeight : this.ActualWidth;
            ClockBorderThickness = lowest / 25;
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
            autoStartStopTimer();
        }

        private void performDoubleButtonClick()
        {
            CustomCommands.ResetCommand.Execute(null, this);
        }

        private void performTripleButtonClick()
        {
            CustomCommands.ResetToZeroCommand.Execute(null, this);
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

        private void autoStartStopTimer()
        {
            if (timer.IsRunning)
            {
                timer.Stop();
            }
            else
            {
                autoStartTimer();
            }
        }


        private void autoStartTimer()
        {
            if (timer.IsZero)
            {
                timer.Mode = SessionTimer.TimerModes.Up;
            }
            else
            {
                timer.Mode = SessionTimer.TimerModes.Down;
            }
            timer.Start();
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
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                timer.Minutes += 10;
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                timer.Minutes -= 10;
            }
            e.Handled = true;
        }

        private void minutesSingleBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                timer.Minutes += 1;
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                timer.Minutes -= 1;
            }
            e.Handled = true;
        }

        private void secondsTenBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                timer.Seconds += 10;
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                timer.Seconds -= 10;
            }
            e.Handled = true;
        }

        private void secondsSingleBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                timer.Seconds += 1;
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                timer.Seconds -= 1;
            }
            e.Handled = true;
        }

        private void raisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void toggleWindowState()
        {
            IsFullscreen = !IsFullscreen;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ResizeMode = System.Windows.ResizeMode.CanResizeWithGrip;
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
        }

        private void clockBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            e.Handled = true; //need to suppress contextmenu from opening when right-clicking on digits
        }


        private void FullscreenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            toggleWindowState();
        }

        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void StartCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !timer.IsRunning;
        }

        private void StartAutoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            autoStartTimer();
        }

        private void StartUpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            timer.Mode = SessionTimer.TimerModes.Up;
            timer.Start();
        }

        private void StartDownCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            timer.Mode = SessionTimer.TimerModes.Down;
            timer.Start();
        }

        private void StopCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = timer.IsRunning;
        }

        private void StopCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            timer.Stop();
        }

        private void StartDownCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !timer.IsRunning && !timer.IsZero;
        }

        private void ResetCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            timer.ResetToInitialValue();
            resetNumericInput();
        }

        private void ResetToZeroCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            timer.ResetToZero();
            resetNumericInput();
        }

        private void ExitFullscreenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsFullscreen;
        }

        private void ExitFullscreenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IsFullscreen = false;
        }

        private void ToggleStartStopCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            autoStartStopTimer();
            if (!startStopResetButton.IsFocused)
            {
                Storyboard blinkAnimation = (Storyboard)FindResource("buttonPress");
                blinkAnimation.Begin(startStopResetButton);
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = null;
            if (IsFullscreen)
            {
                mouseHideTimer.Stop();
                mouseHideTimer.Start();
            }
        }

        void mouseHideTimer_Tick(object sender, EventArgs e)
        {
            mouseHideTimer.Stop();
            Mouse.OverrideCursor = Cursors.None;
        }


        private void Window_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length == 1)
            {
                char input = e.Text[0];
                if (char.IsNumber(input))
                {
                    processManualDigitInput(input.ToString());
                    e.Handled = true;
                }
            }
        }

        private void processManualDigitInput(string digit)
        {
            if (timer.IsRunning) return;
            int inputValue = Convert.ToInt32(digit);

            // When the first digit is pressed, reset the timer value and start at the first digit (highest index)
            if (manualInputDigitIndex < 0)
            {
                timer.ResetToZero();
                manualInputDigitIndex = 3;
            }

            // The seconds
            if ((manualInputDigitIndex <= 1) && (manualInputDigitIndex >= 0))
            {
                timer.Seconds = timer.Seconds + (inputValue * (int)Math.Pow(10, manualInputDigitIndex));
            }

            // The minutes
            if ((manualInputDigitIndex > 1) && (manualInputDigitIndex <= 3))
            {
                timer.Minutes = timer.Minutes + (inputValue * (int)Math.Pow(10, manualInputDigitIndex - 2));
            }
            manualInputDigitIndex--;
        }

        private void resetNumericInput()
        {
            manualInputDigitIndex = -1;
        }

        void timer_TimerStarted(object sender, EventArgs e)
        {
            resetNumericInput();
        }

        private void Window_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource == minutesSingleBox) return;
            if (e.OriginalSource == minutesTenBox) return;
            if (e.OriginalSource == secondsSingleBox ) return;
            if (e.OriginalSource == secondsTenBox) return;
            if (e.OriginalSource == startStopResetButton) return;
            if (e.OriginalSource == ONButtonImage) return;

            CustomCommands.ToggleFullscreenCommand.Execute(null, this);

        }

    }
}
