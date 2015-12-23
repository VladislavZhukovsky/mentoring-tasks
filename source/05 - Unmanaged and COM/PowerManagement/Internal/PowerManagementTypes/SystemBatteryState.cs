using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PowerManagement.Internal.PowerManagementTypes
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SystemBatteryState
    {
        public bool AcOnLine;
        public bool BatteryPresent;
        public bool Charging;
        public bool Discharging;
        public byte Spare1;
        public byte Spare2;
        public byte Spare3;
        public byte Spare4;
        public UInt32 MaxCapacity;
        public UInt32 RemainingCapacity;
        public UInt32 Rate;
        public UInt32 EstimatedTime;
        public UInt32 DefaultAlert1;
        public UInt32 DefaultAlert2;
    }
}
