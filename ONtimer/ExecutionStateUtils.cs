using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ONtimer
{
    internal static class ExecutionStateUtils
    {
        [FlagsAttribute]
        private enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        public static void PreventMonitorPowerdown()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
        }

        public static void AllowMonitorPowerdown()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
        }


        public static void PreventSleep()
        {
            // Prevent Idle-to-Sleep (monitor not affected) (see note above)
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_AWAYMODE_REQUIRED);
        }

        public static void KeepSystemAwake()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_SYSTEM_REQUIRED);
        }

        /// <summary>
        /// Notifies the system that the screen is in use. Only resets the screen sleep timer, so this method has to b called periodically to keep the screen turned on.
        /// </summary>
        public static void NotifyScreenInUse()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED);
        }
    }
}
