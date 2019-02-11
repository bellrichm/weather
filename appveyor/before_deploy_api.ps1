""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

if ($env:BUILD_ARTIFACT -eq "NO")
{
  return
}

$cmd = 'dotnet publish api/src/BellRichM.Weather.Web/BellRichM.Weather.Web.csproj '
$cmd = $cmd + '--output $env:APPVEYOR_BUILD_FOLDER/dist '
$cmd = $cmd + '--no-restore '
$cmd = $cmd + '-f netcoreapp2.1 '
RunCmd $cmd

$parms = '-x "Data/*" "logs/*" '

if ($env:BUILD_PLATFORM -eq "Windows")
{
  $parms = $parms + 'a $env:APPVEYOR_BUILD_FOLDER/$env:ARTIFACT_NAME.zip $env:APPVEYOR_BUILD_FOLDER/dist/*'
  $cmd = "7z $parms"
  RunCmd $cmd
}

if ($env:BUILD_PLATFORM -eq "Unix")
{
  $parms = $parms + '-r ../$env:ARTIFACT_NAME.zip *'
  Push-Location $env:APPVEYOR_BUILD_FOLDER/dist
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