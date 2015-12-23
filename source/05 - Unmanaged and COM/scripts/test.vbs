Public Sub Test()
	Dim adapter
	
	Set adapter = CreateObject(PowerManagement.PowerManagementAdapter)
	
	res = adapter.GetLastSleepTime()
	Wscript.Echo("---Last Sleep Time: ")
	Wscript.Echo(res)

	res = adapter.GetLastWakeTime()
	Wscript.Echo("---Last Wake Time: ")
	Wscript.Echo(res)

	res = adapter.GetSystemBatteryState()
	Wscript.Echo("---System Battery State: ")
	Wscript.Echo(res.ToString())

	res = adapter.GetSystemPowerInformation()
	Wscript.Echo("---System Power Information: ")
	Wscript.Echo(res.ToString())
End Sub