Function RunCmd {
  Param ($cmd = $cmd)
  Process{
    Write-Host "Running: $cmd"
    Invoke-Expression $cmd
    Write-Host "Return code: $LastExitCode"

    if ($LastExitCode -ne 0)
    {
      Write-Host "Error running: $cmd"
      exit $LastExitCode
    }
  }
}

"******************************** " + $MyInvocation.InvocationName + " ********************************"
# TODO: check return codes of build steps

$env:APPVEYOR_BUILD_VERSION = "local"
$env:APPVEYOR_BUILD_FOLDER =  Split-Path $MyInvocation.MyCommand.Path

Invoke-Expression -Command ./localtools/init.ps1

$env:BUILDPATH = "C:\Program Files\7-Zip;C:\RMBData\sonar-scanner-msbuild;"
$env:PATH = $env:PATH + $env:BUILDPATH

$preClean = "NO"
if ($preClean -ne "NO")
{
  dotnet clean api/test/BellRichM.Weather.Test.sln
  dotnet clean api/src/BellRichM.Weather.sln
  dotnet clean api/integration/BellRichM.Identity.Api.Integration/BellRichM.Identity.Api.Integration.csproj
  dotnet clean api/smoke/BellRichM.Identity.Api.Smoke/BellRichM.Identity.Api.Smoke.csproj
  Remove-Item $env:APPVEYOR_BUILD_FOLDER/dist -Force -Recurse -ErrorAction Ignore
  Remove-Item $env:APPVEYOR_BUILD_FOLDER/$env:ARTIFACT_NAME.zip -ErrorAction Ignore
}

$env:ARTIFACT_NAME = "weather-branch"
$env:BRANCH_NAME = "buildexperiments"

$env:RESTORE = "NO"
$env:BUILD = "NO"
$env:UNIT_TEST = "YES"
$env:UPLOAD_COVERALLS = "NO"
$env:UPLOAD_SONARQUBE = 'NO'
$env:INTEGRATION_TEST = "NO"
$env:SMOKE_TEST = "NO"
$env:BUILD_ARTIFACT = "NO"
$env:DEPLOY = "NO"

RunCmd " ./appveyor/init.ps1"

RunCmd "./appveyor/before_build.ps1"

RunCmd "./appveyor/build.ps1"

RunCmd "./appveyor/after_build.ps1"

RunCmd "./appveyor/test.ps1"

RunCmd "./appveyor/before_deploy.ps1"

RunCmd "./appveyor/deploy.ps1"

RunCmd "./appveyor/after_deploy.ps1"