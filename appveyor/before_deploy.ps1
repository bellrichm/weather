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

if ($env:BUILD_ARTIFACT -eq "NO")
{
  return
}

# TODO: add --no-build when it is supported again
$cmd = 'dotnet publish api/src/BellRichM.Weather.Web/BellRichM.Weather.Web.csproj '
$cmd = $cmd + '--output $env:APPVEYOR_BUILD_FOLDER/dist '
$cmd = $cmd + '--no-restore '
$cmd = $cmd + '-f netcoreapp2.0 '
RunCmd $cmd

#dotnet clean api/test/BellRichM.Weather.Test.sln
#dotnet clean api/src/BellRichM.Weather.sln
#dotnet clean api/integration/BellRichM.Identity.Api.Integration/BellRichM.Identity.Api.Integration.csproj
## dotnet clean api/smoke/BellRichM.Identity.Api.Smoke/BellRichM.Identity.Api.Smoke.csproj

if ($env:BUILD_PLATFORM -eq "Windows")
{
  $parms = 'a $env:APPVEYOR_BUILD_FOLDER/$env:ARTIFACT_NAME.zip $env:APPVEYOR_BUILD_FOLDER/dist/*'
  $cmd = "7z $parms"
  RunCmd $cmd
}

if ($env:BUILD_PLATFORM -eq "Unix")
{
  $parms = '-r ../$env:ARTIFACT_NAME.zip *'
  Push-Location $env:APPVEYOR_BUILD_FOLDER/dist
  $cmd = "zip $parms"
  RunCmd $cmd
  Pop-Location
}