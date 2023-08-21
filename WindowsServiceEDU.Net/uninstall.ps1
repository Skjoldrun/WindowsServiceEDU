# Deletes the service if not running any more, else mark for deletion after stop
# open a PowerShell Terminal with Administrator priviledges
# sc.exe is needed in Powershell to not call teh sc commandlet for set content

$ServiceName = "WindowsServiceEDU"

sc.exe delete $ServiceName