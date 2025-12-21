# dotnet CLI (repo root)

Bu repo kök dizininde hem modern .NET (CoreBuilder / CoreBuilder.Admin) hem de legacy WebForms (`w3.vbproj`) bulundu?u için `dotnet build` / `dotnet clean` / `dotnet run` komutlar? kök dizinde **hangi projenin kullan?laca??n? bilemez**.

## Önerilen komutlar

### Temizle + Derle

```powershell
dotnet clean .\web01.sln
dotnet build .\web01.sln
```

### Admin'i çal??t?r

```powershell
dotnet run --project .\CoreBuilder.Admin\CoreBuilder.Admin.csproj
```

### K?sayol komut dosyalar?

Windows:
- `dotnet.clean.cmd`
- `dotnet.build.cmd`
- `dotnet.run-admin.cmd`

## Legacy `w3.vbproj`

`w3.vbproj` Visual Studio WebApplication targets (`Microsoft.WebApplication.targets`) ister.
Sadece `dotnet` CLI ile derlenemez; Visual Studio / Build Tools kurulumu gerektirir.
