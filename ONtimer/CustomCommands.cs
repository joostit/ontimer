using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ONtimer.Commands
{
    class CustomCommands
    {
        public static readonly RoutedUICommand ToggleFullscreenCommand = new RoutedUICommand("Toggle fullscreen", "toggleFullscreen", typeof(MainWindow));
        public static readonly RoutedUICommand ExitCommand = new RoutedUICommand("Exit", "exit", typeof(MainWindow));
        public static readonly RoutedUICommand StartAutoCommand = new RoutedUICommand("Start (Auto)", "startAuto", typeof(MainWindow));
        public static readonly RoutedUICommand StartUpCommand = new RoutedUICommand("Start Upwards", "startUp", typeof(MainWindow));
        public static readonly RoutedUICommand StartDownCommand = new RoutedUICommand("Start Downwards", "startDown", typeof(MainWindow));
        public static readonly RoutedUICommand StopCommand = new RoutedUICommand("Stop", "Stop", typeof(MainWindow));
        public static readonly RoutedUICommand ResetCommand = new RoutedUICommand("Reset", "reset", typeof(MainWindow));
        public static readonly RoutedUICommand ResetToZeroCommand = new RoutedUICommand("Reset to zero", "resetToZero", typeof(MainWindow));
        public static readonly RoutedUICommand ExitFullscreenCommand = new RoutedUICommand("Exit fullscreen", "exitFullscreen", typeof(MainWindow));
        public static readonly RoutedUICommand ToggleStartStopCommand = new RoutedUICommand("Toggle Start/Stop", "toggleStartStop", typeof(MainWindow));
    }
}
