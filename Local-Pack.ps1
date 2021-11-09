param ([Parameter(Mandatory=$true)][string] $version, [Parameter()][string] $output = ($Env:NUGET_LOCAL_PACKAGES ?? 'C:/nuget-packages'))

# https://semver.org/
if ($version -match '^(?:0|[1-9]\d*)\.(?:0|[1-9]\d*)\.(?:0|[1-9]\d*)(?:-(?:(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+(?:[0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$') {
    Invoke-Expression -Command "dotnet pack -c Release /p:NoDefaultExcludes=true /p:PackageVersion=${version} -o ${output}"
} else {
    Write-Host 'Invalid version!'
}