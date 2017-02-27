%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe C:\DEV\Envision\Source Code\Envision.SPS\Envision.SPS.SyncService\Envision.SPS.EventBus\Envision.SPS.EventBus.exe 
Net Start CLPScoreMonitorService
sc config CLPScoreMonitorService start= auto
::pause