""
"******************************** " + $MyInvocation.InvocationName + " ********************************"
if ($env:BUILD_API -eq "NO" `
  -And $env:UPLOAD_SONARQUBE_API -eq 'NO' `
  -And $env:RUNONLY_SONARQUBE_API -ne 'YES')
{
  return
}

# ToDo - move to env var
$unitTestFramework = '-f netcoreapp3.1 '
$buildFramework = '-f netcoreapp3.1 '

$defBuildParams = '--no-restore '

$cmd = 'dotnet build api/src/BellRichM.Weather.sln -l:"C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll" --no-restore ' + $buildFramework
RunCmd $cmd

$cmd = "dotnet build api\test\BellRichM.Weather.Test.sln $defBuildParams $unitTestFramework"
RunCmd $cmd

$cmd = "dotnet build api/integration/BellRichM.Identity.Api.Integration/BellRichM.Identity.Api.Integration.csproj $defBuildParams "
RunCmd $cmd

$cmd = "dotnet build api/smoke/BellRichM.Identity.Api.Smoke/BellRichM.Identity.Api.Smoke.csproj $defBuildParams "
RunCmd $cmd
