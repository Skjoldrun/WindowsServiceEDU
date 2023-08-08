@ECHO off

REM Set the variables: 
REM  %sourcePath% to be the current directory path
REM  %installUtilPath% for the .NET InstallUtil.exe
REM  %serviceName% for the ServiceName
SET sourcePath=%cd%
SET installUtilPath="C:\Windows\Microsoft.NET\Framework\v4.0.30319"
SET serviceName=WindowsServiceEDU

REM Change to InstalUtils path
C:
CD %installUtilPath%

REM call InstallUtil to install the service
InstallUtil.exe /u /LogToConsole=true %sourcePath%\%serviceName%.exe

PAUSE