using System;
using System.Runtime.InteropServices;
using PowerManagement.Exceptions;
using PowerManagement.Internal;
using PowerManagement.Internal.PowerManagementTypes;

namespace PowerManagement
{
    [ComVisible(true)]
    [Guid("5FCAA83C-3311-457E-9C9E-C8C9861E4C43")]
    [ClassInterface(ClassInterfaceType.None)]
    public class PowerManagementAdapter: IPowerManagementAdapter
    {
        ulong IPowerManagementAdapter.GetLastSleepTime()
        {
            IntPtr retVal = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(UInt64)));
            PerformOperation(
                PowerManagementFunctions.CallNtPowerInformation(
                    PowerInformationLevel.LastSleepTime,
                    IntPtr.Zero,
                    0,
                    retVal,
                    Marshal.SizeOf(typeof(ulong))
                ),
                false
            );
            ulong lst = (ulong)Marshal.PtrToStructure(retVal, typeof(UInt64));
            return lst;
        }

        ulong IPowerManagementAdapter.GetLastWakeTime()
        {
            IntPtr retVal = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(UInt64)));
            PerformOperation(
                PowerManagementFunctions.CallNtPowerInformation(
                    PowerInformationLevel.LastWakeTime,
                    IntPtr.Zero,
                    0,
                    retVal,
                    Marshal.SizeOf(typeof(ulong))
                ),
                false
            );
            ulong lwt = (ulong)Marshal.PtrToStructure(retVal, typeof(UInt64));
            return lwt;
        }

        PowerManagement.PowerManagementObjects.SystemBatteryState IPowerManagementAdapter.GetSystemBatteryState()
        {
            IntPtr retVal = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(SystemBatteryState)));
            PerformOperation(
                PowerManagementFunctions.CallNtPowerInformation(
                    PowerInformationLevel.SystemBatteryState,
                    IntPtr.Zero,
                    0,
                    retVal,
                    Marshal.SizeOf(typeof(SystemBatteryState))
                ),
                false
            );
            SystemBatteryState sbs = (SystemBatteryState)Marshal.PtrToStructure(retVal, typeof(SystemBatteryState));
            return new PowerManagementObjects.SystemBatteryState()
            {
                AcOnLine = sbs.AcOnLine,
                BatteryPresent = sbs.BatteryPresent,
                Charging = sbs.Charging,
                Discharging = sbs.Discharging,
                Spare1 = sbs.Spare1,
                Spare2 = sbs.Spare2,
                Spare3 = sbs.Spare3,
                Spare4 = sbs.Spare4,
                MaxCapacity = sbs.MaxCapacity,
                RemainingCapacity = sbs.RemainingCapacity,
                Rate = sbs.Rate,
                EstimatedTime = sbs.EstimatedTime,
                DefaultAlert1 = sbs.DefaultAlert1,
                DefaultAlert2 = sbs.DefaultAlert2
            };
            //return sbs;
        }

        PowerManagement.PowerManagementObjects.SystemPowerInformation IPowerManagementAdapter.GetSystemPowerInformation()
        {
            IntPtr retVal = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(SystemPowerInformation)));
            PerformOperation(
                PowerManagementFunctions.CallNtPowerInformation(
                    PowerInformationLevel.SystemPowerInformation,
                    IntPtr.Zero,
                    0,
                    retVal,
                    Marshal.SizeOf(typeof(SystemPowerInformation))
                ),
                false
            );
            SystemPowerInformation spi = (SystemPowerInformation)Marshal.PtrToStructure(retVal, typeof(SystemPowerInformation));
            return new PowerManagementObjects.SystemPowerInformation()
            {
                MaxIdlenessAllowed = spi.MaxIdlenessAllowed,
                Idleness = spi.Idleness,
                TimeRemaining = spi.TimeRemaining,
                CoolingMode = spi.CoolingMode
            };
            //return spi;
        }

        void IPowerManagementAdapter.ReserveHibernationFile()
        {
            byte lpInBuffer = 1;
            IntPtr lpInBufferPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(byte)));
            Marshal.StructureToPtr(lpInBuffer, lpInBufferPtr, false);
            IntPtr lpOutBufferPtr = IntPtr.Zero;
            PerformOperation(
                PowerManagementFunctions.CallNtPowerInformation(
                    10,
                    lpInBufferPtr,
                    Marshal.SizeOf(typeof(byte)),
                    lpOutBufferPtr,
                    0
                ),
                false
            );
        }

        void IPowerManagementAdapter.RemoveHibernationFile()
        {
            byte lpInBuffer = 0;
            IntPtr lpInBufferPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(byte)));
            Marshal.StructureToPtr(lpInBuffer, lpInBufferPtr, false);
            IntPtr lpOutBufferPtr = IntPtr.Zero;
            PerformOperation(
                PowerManagementFunctions.CallNtPowerInformation(
                    10,
                    lpInBufferPtr,
                    Marshal.SizeOf(typeof(byte)),
                    lpOutBufferPtr,
                    0
                ),
                false
            );
        }

        void IPowerManagementAdapter.SetSuspendState()
        {
            PerformOperation(
                PowerManagementFunctions.SetSuspendState(
                    (byte)0,
                    (byte)0,
                    (byte)0
                ),
                true
            );
        }

        void IPowerManagementAdapter.SetHibernateState()
        {
            PerformOperation(
                PowerManagementFunctions.SetSuspendState(
                    (byte)1,
                    (byte)0,
                    (byte)0
                ),
                true
            );
        }

        private void PerformOperation(uint errorCode, bool inverseError)
        {
            if (inverseError)
            {
                if (errorCode == 0)
                {
                    throw new PowerManagementException(string.Format("Function executed with error code {0}", errorCode), errorCode);
                }
                return;
            }
            else
            {
                if (errorCode == 0)
                {
                    return;
                }
                throw new PowerManagementException(string.Format("Function executed with error code {0}", errorCode), errorCode);
            }
        }
    }
}
