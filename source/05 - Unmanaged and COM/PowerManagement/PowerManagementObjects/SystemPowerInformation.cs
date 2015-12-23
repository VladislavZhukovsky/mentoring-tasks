using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerManagement.PowerManagementObjects
{
    public class SystemPowerInformation
    {
        public UInt32 MaxIdlenessAllowed;
        public UInt32 Idleness;
        public UInt32 TimeRemaining;
        public byte CoolingMode;

        internal SystemPowerInformation()
        {

        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Max Idleness Allowed: {0}%", MaxIdlenessAllowed));
            sb.AppendLine(string.Format("Idleness: {0}%", Idleness));
            sb.AppendLine(string.Format("Time Remaining : {0} sec", TimeRemaining));

            string coolingMode = string.Empty;
            switch (CoolingMode)
            {
                case PowerManagement.Internal.CoolingMode.Active:
                    coolingMode = "Active";
                    break;
                case PowerManagement.Internal.CoolingMode.Passive:
                    coolingMode = "Passive";
                    break;
                case PowerManagement.Internal.CoolingMode.InvalidMode:
                    coolingMode = "No CPU throttling or no thermal zone defined in the system";
                    break;
                default:
                    break;
            }
            sb.AppendLine(string.Format("Cooling Mode : {0}", coolingMode));

            return sb.ToString();
        }
    }
}
