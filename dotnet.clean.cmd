@echo off
setlocal

dotnet clean "%~dp0web01.sln" %*
