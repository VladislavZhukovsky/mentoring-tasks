using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerManagement.PowerManagementObjects
{
    public class SystemBatteryState
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

        internal SystemBatteryState()
        {

        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("AcOnLine :          {0}", AcOnLine));
            sb.AppendLine(string.Format("BatteryPresent :    {0}", BatteryPresent));
            sb.AppendLine(string.Format("Charging :          {0}", Charging));
            sb.AppendLine(string.Format("Discharging :       {0}", Discharging));
            sb.AppendLine(string.Format("MaxCapacity :       {0}", MaxCapacity));
            sb.AppendLine(string.Format("RemainingCapacity : {0}", RemainingCapacity));
            sb.AppendLine(string.Format("Rate :              {0}", Rate));
            sb.AppendLine(string.Format("EstimatedTime :     {0}", EstimatedTime));
            return sb.ToString();
        }
    }
}
