param(
    [ValidateSet('build','clean','restore','test')]
    [string]$Target = 'build'
)

$ErrorActionPreference = 'Stop'

$solution = Join-Path $PSScriptRoot 'CoreBuilder.sln'

if (-not (Test-Path $solution)) {
    throw "Solution not found: $solution"
}

switch ($Target) {
    'restore' { dotnet restore $solution }
    'clean'   { dotnet clean $solution }
    'build'   { dotnet build $solution }
    'test'    { dotnet test $solution }
}
