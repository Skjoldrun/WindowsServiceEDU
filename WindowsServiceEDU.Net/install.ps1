# Installs a service with given configurations for user, path, restart etc.
# open a PowerShell Terminal with Administrator priviledges
# sc.exe is needed in Powershell to not call teh sc commandlet for set content

$ServiceName = "WindowsServiceEDU"  
$DisplayName = "WindowsServiceEDU some Displayname"
$Description = "Some Description"	
$ServiceUser = "otto-chemie\cl-dh"  
$StartUpMode = "delayed-auto"         # possible are boot|system|auto|demand|disabled|delayed-auto
$Path = $PWD.Path                     # the path to the current directory

#Write-Host "Path to Service is: $($Path)\$($ServiceName).exe"

sc.exe create $ServiceName DisplayName= $DisplayName binpath= "$($Path)\$($ServiceName).exe" obj= $ServiceUser start= $StartUpMode
sc.exe description $ServiceName $Description
sc.exe failure $ServiceName reset= 30 actions= restart/5000  # Set restart options on failure
