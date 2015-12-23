using System;
using System.Runtime.InteropServices;

namespace PowerManagement.Internal
{
    internal static class PowerManagementFunctions
    {

        [DllImport("powrprof.dll")]
        internal static extern uint CallNtPowerInformation(
            [In]  UInt32 informationLevel,
            [In]  IntPtr lpInputBuffer,
            [In]  Int32 nInputBufferSize,
            [Out] IntPtr lpOutputBuffer,
            [In]  Int32 nOutputBufferSize
        );

        [DllImport("powrprof.dll")]
        internal static extern byte SetSuspendState(
            [In] byte hibernate,
            [In] byte forceCritical,
            [In] byte disableWakeEvent
        );

    }
}
