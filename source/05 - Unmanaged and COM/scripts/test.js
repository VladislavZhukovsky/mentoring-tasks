adapter = new ActiveXObject("PowerManagement.PowerManagementAdapter");

res = adapter.GetLastSleepTime();
WScript.Echo("---Last Sleep Time: ");
WScript.Echo(res);

res = adapter.GetLastWakeTime();
WScript.Echo("---Last Wake Time: ");
WScript.Echo(res);

res = adapter.GetSystemBatteryState();
WScript.Echo("---System Battery State: ");
WScript.Echo(res.ToString());

res = adapter.GetSystemPowerInformation();
WScript.Echo("---System Power Information: ");
WScript.Echo(res.ToString());