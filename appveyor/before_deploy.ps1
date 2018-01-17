Function RunCmd {
  Param ($cmd = $cmd)
  Process{
    Write-Host "Running: $cmd"
    Invoke-Expression $cmd
    Write-Host "Return code: $LastExitCode"

    if ($LastExitCode -ne 0)
    {
      Write-Host "Error running: $cmd"
      if ([string]::IsNullOrEmpty($LastExitCode))
      {
        exit 1
      }

      exit $LastExitCode
    }

    if ( -Not [string]::IsNullOrEmpty($error))
    {
      Write-Host "Error running: $cmd"
      Write-Host "$error"
      exit 1
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

if ($env:UPLOAD_ARTIFACT -eq "NO")
{
  return
}

$cmd = "Push-AppveyorArtifact $env:ARTIFACT_NAME.zip -XDeploymentName $env:ARTIFACT_NAME -type WebDeployPackage"
RunCmd $cmd