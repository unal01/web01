@echo off
setlocal

dotnet build "%~dp0web01.sln" %*
