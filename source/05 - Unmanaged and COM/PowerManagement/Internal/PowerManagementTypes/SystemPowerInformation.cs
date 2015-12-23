using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerManagement.Internal;
using System.Runtime.InteropServices;

namespace PowerManagement.Internal.PowerManagementTypes
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SystemPowerInformation
    {
        public UInt32 MaxIdlenessAllowed;
        public UInt32 Idleness;
        public UInt32 TimeRemaining;
        public byte CoolingMode;
    }
}
