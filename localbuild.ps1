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
$error.Clear()

$env:APPVEYOR_BUILD_VERSION = "local"
$env:APPVEYOR_BUILD_FOLDER =  Split-Path $MyInvocation.MyCommand.Path

Invoke-Expression -Command ./localtools/init.ps1

$env:COVERAGE_REPORT = "YES"

$env:BUILDPATH = "C:\Program Files\7-Zip;C:\RMBData\sonar-scanner-msbuild;$env:HOMEPATH\.nuget\packages\ReportGenerator\3.0.2\tools;"
$env:PATH = $env:PATH + $env:BUILDPATH

$preClean = "YES"
$postClean = "YES"

if ($preClean -ne "NO")
{
  Write-Host "******************************** PreCleaning ********************************"
  dotnet clean -v m api/test/BellRichM.Weather.Test.sln
  dotnet clean -v m api/src/BellRichM.Weather.sln
  dotnet clean -v m api/integration/BellRichM.Identity.Api.Integration/BellRichM.Identity.Api.Integration.csproj
  dotnet clean -v m api/smoke/BellRichM.Identity.Api.Smoke/BellRichM.Identity.Api.Smoke.csproj
  Remove-Item $env:APPVEYOR_BUILD_FOLDER/dist -Force -Recurse -ErrorAction Ignore
  Remove-Item $env:APPVEYOR_BUILD_FOLDER/$env:ARTIFACT_NAME.zip -ErrorAction Ignore
}

$env:ARTIFACT_NAME = "weather-branch"
$env:BRANCH_NAME = "buildexperiments"

$env:RESTORE = "YES"
$env:BUILD = "YES"
$env:UNIT_TEST = "YES"
$env:INTEGRATION_TEST = "YES"
$env:BUILD_ARTIFACT = "YES"
# Unless testing the process, these should usually be set to NO
$env:UPLOAD_COVERALLS = "NO"
$env:UPLOAD_SONARQUBE = "NO"
$env:UPLOAD_ARTIFACT = "NO"
$env:DEPLOY = "NO"
$env:SMOKE_TEST = "NO"

RunCmd " ./appveyor/init.ps1"

RunCmd "./appveyor/before_build.ps1"

RunCmd "./appveyor/build.ps1"

RunCmd "./appveyor/after_build.ps1"

RunCmd "./appveyor/test.ps1"

RunCmd "./appveyor/before_deploy.ps1"

RunCmd "./appveyor/deploy.ps1"

RunCmd "./appveyor/after_deploy.ps1"

if ($postClean -ne "NO")
{
  RunCmd "git checkout api/integration/BellRichM.Identity.Api.Integration/data/Identity.db"
  RunCmd "rm $env:APPVEYOR_BUILD_FOLDER/$env:ARTIFACT_NAME.zip"
}
