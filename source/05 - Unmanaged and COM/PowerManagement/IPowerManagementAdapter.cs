using PowerManagement.PowerManagementObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PowerManagement
{
    [ComVisible(true)]
    [Guid("94704042-2A44-462B-B5B9-AB08BBD3C953")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IPowerManagementAdapter
    {
        ulong GetLastSleepTime();
        ulong GetLastWakeTime();
        SystemBatteryState GetSystemBatteryState();
        SystemPowerInformation GetSystemPowerInformation();
        void ReserveHibernationFile();
        void RemoveHibernationFile();
        void SetSuspendState();
        void SetHibernateState();
    }
}
