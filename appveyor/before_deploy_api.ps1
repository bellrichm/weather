""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

if ($env:BUILD_ARTIFACT -eq "NO")
{
  return
}

Copy-Item -Path $env:APPVEYOR_BUILD_FOLDER/dist -Destination $env:APPVEYOR_BUILD_FOLDER/dist-linux-arm -Recurse -Force

$cmd = 'dotnet publish api/src/BellRichM.Weather.Web/BellRichM.Weather.Web.csproj '
$cmd = $cmd + '--output $env:APPVEYOR_BUILD_FOLDER/dist '
$cmd = $cmd + '--no-restore '
$cmd = $cmd + '-f netcoreapp3.1 ' # ToDo - move to env var
RunCmd $cmd

$cmd = 'dotnet publish api/src/BellRichM.Weather.Web/BellRichM.Weather.Web.csproj '
$cmd = $cmd + '--output $env:APPVEYOR_BUILD_FOLDER/dist-linux-arm '
# $cmd = $cmd + '--no-restore '
$cmd = $cmd + '-r linux-arm '
$cmd = $cmd + '-f netcoreapp3.1 ' # ToDo - move to env var
RunCmd $cmd

if ($env:BUILD_PLATFORM -eq "Windows")
{
  $parms = '-xr!"Data" -xr!"logs" '
  $parms = $parms + 'a $env:APPVEYOR_BUILD_FOLDER/$env:ARTIFACT_NAME.zip $env:APPVEYOR_BUILD_FOLDER/dist/*'
  $cmd = "7z $parms"
  RunCmd $cmd
  
  $parms = '-xr!"Data" -xr!"logs" '
  $parms = $parms + 'a $env:APPVEYOR_BUILD_FOLDER/$env:ARTIFACT_NAME-linux-arm.zip $env:APPVEYOR_BUILD_FOLDER/dist-linux-arm/*'
  $cmd = "7z $parms"
  RunCmd $cmd
}

if ($env:BUILD_PLATFORM -eq "Unix")
{
  $parms = '-x "Data/*" "logs/*" '
  $parms = $parms + '-r ../$env:ARTIFACT_NAME.zip *'
  Push-Location $env:APPVEYOR_BUILD_FOLDER/dist
  $cmd = "zip $parms"
  RunCmd $cmd
  Pop-Location
  
  $parms = '-x "Data/*" "logs/*" '
  $parms = $parms + '-r ../$env:ARTIFACT_NAME-linux-arm.zip *'
  Push-Location $env:APPVEYOR_BUILD_FOLDER/dist-linux-arm
  $cmd = "zip $parms"
  RunCmd $cmd
  Pop-Location
}

if ($env:UPLOAD_ARTIFACT -eq "NO")
{
  return
}

$cmd = "Push-AppveyorArtifact $env:ARTIFACT_NAME.zip -DeploymentName $env:ARTIFACT_NAME -type WebDeployPackage"
RunCmd $cmd
$cmd = "Push-AppveyorArtifact $env:ARTIFACT_NAME-linux-arm.zip -DeploymentName $env:ARTIFACT_NAME-linux-arm -type WebDeployPackage"
RunCmd $cmd
