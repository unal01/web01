@echo off
setlocal

dotnet run --project "%~dp0CoreBuilder.Admin\CoreBuilder.Admin.csproj" %*
