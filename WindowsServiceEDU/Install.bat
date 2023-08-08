@ECHO off

REM Set the variables: 
REM  %sourcePath% to be the current directory path
REM  %installUtilPath% for the .NET InstallUtil.exe
REM  %serviceName% for the ServiceName
REM  %serviceUser% for the ServiceUser
SET sourcePath=%cd%
SET installUtilPath="C:\Windows\Microsoft.NET\Framework\v4.0.30319"
SET serviceName=WindowsServiceEDU
SET serviceUser=otto-chemie\cl-dh

REM Change to InstalUtils path
C:
CD %installUtilPath%

REM call InstallUtil to install the service
InstallUtil.exe /LogToConsole=true /username=%serviceUser% %sourcePath%\%serviceName%.exe

PAUSE