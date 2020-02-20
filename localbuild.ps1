Import-Module ./appveyor/utils

"******************************** " + $MyInvocation.InvocationName + " ********************************"
$error.Clear()

$env:BUILDTYPE = "LOCAL"

$env:APPVEYOR_BUILD_VERSION = "local"
$env:APPVEYOR_BUILD_FOLDER =  Split-Path $MyInvocation.MyCommand.Path

# pretend its an appveyor build for coveralls.io/coveralls.net.exe
$env:APPVEYOR = "true"
$env:APPVEYOR_JOB_ID = get-random

$env:APPVEYOR_REPO_BRANCH = "buildexperiments"
# $env:APPVEYOR_REPO_COMMIT=
# $env:APPVEYOR_REPO_COMMIT_MESSAGE =
# $env:APPVEYOR_REPO_COMMIT_AUTHOR =
# $env:APPVEYOR_REPO_COMMIT_AUTHOREMAIL =
# $env:APPVEYOR_PULL_REQUEST_NUMBER =


Invoke-Expression -Command ./localtools/init.ps1

$env:COVERAGE_REPORT = "YES"

##$env:BUILDPATH = ";C:\Program Files\7-Zip;C:\RMBData\sonar-scanner-msbuild;$env:HOMEPATH\.nuget\packages\ReportGenerator\3.0.2\tools;"
##$env:PATH = $env:PATH + $env:BUILDPATH

$preClean = "YES"
$postClean = "YES"

RunCmd "dotnet build-server shutdown"

if ($preClean -ne "NO")
{
  Write-Host "******************************** PreCleaning ********************************"
  # RunCmd "git clean -xdf api"
  if (Test-Path mspec.xml)
  {
    Remove-Item mspec.xml
  }
  if (Test-Path opencover.xml)
  {
    Remove-Item opencover.xml
  }
  RunCmd "git clean -xdf app/coverage"
  RunCmd "git clean -xdf app/*/*/obj"
  RunCmd "git clean -xdf api/*/*/obj"
  RunCmd "git clean -xdf api/*/*/bin"
  RunCmd "dotnet clean -v m app/BellRichM.App.csproj" ToDo - uncomment when app developed
  RunCmd "dotnet clean -v m api/test/BellRichM.Weather.Test.sln"
  RunCmd "dotnet clean -v m api/src/BellRichM.Weather.sln"
  RunCmd "dotnet clean -v m api/integration/BellRichM.Identity.Api.Integration/BellRichM.Identity.Api.Integration.csproj"
  RunCmd "dotnet clean -v m api/smoke/BellRichM.Identity.Api.Smoke/BellRichM.Identity.Api.Smoke.csproj"
  Remove-Item $env:APPVEYOR_BUILD_FOLDER/dist -Force -Recurse -ErrorAction Ignore
  Remove-Item $env:APPVEYOR_BUILD_FOLDER/$env:ARTIFACT_NAME.zip -ErrorAction Ignore
}

$env:ARTIFACT_NAME = "weather-branch"
$env:BRANCH_NAME = "buildexperiments"

$env:RESTORE_API = "YES"
$env:RESTORE_APP = "YES"
$env:BUILD_API = "YES"
$env:BUILD_APP = "YES"
$env:UNIT_TEST_API = "YES"
$env:UNIT_TEST_APP = "YES"
$env:INTEGRATION_TEST_API = "YES"
$env:INTEGRATION_TEST_APP = "unused"
$env:BUILD_ARTIFACT = "YES"
$env:SMOKE_TEST = "YES"
# Unless testing the process, these should usually be set to NO
# If APPVEYOR_REPO_BRANCH is set to "buildexperiments", then it is safe to have these set to "YES"
$env:UPLOAD_COVERALLS_API = "YES"
$env:UPLOAD_COVERALLS_APP = "YES" 
$env:UPLOAD_SONARQUBE_API = "YES"
$env:UPLOAD_SONARQUBE_APP = "YES"

$env:UPLOAD_ARTIFACT = "NO"
$env:DEPLOY = "NO"

# appveyor install
RunCmd " ./appveyor/init.ps1"

# appveyor before-build
RunCmd "./appveyor/before_build_api.ps1"
RunCmd "./appveyor/before_build_app.ps1"

# appveyor build-script
RunCmd "./appveyor/build_api.ps1"
RunCmd "./appveyor/build_app.ps1"

# appveyor after-build
# Order matters, Need to run app first to generate the lcov
# The sonarqube end in api will upload it
RunCmd "./appveyor/after_build_test_unit_app.ps1"
RunCmd "./appveyor/after_build_test_unit_api.ps1"
RunCmd "./appveyor/after_build_test_unit_post.ps1"

# appveyor test-script
RunCmd "./appveyor/test_integration_api.ps1"

# appveyor before-deplot
RunCmd "./appveyor/before_deploy_api.ps1"

# appveyor deploy-script
RunCmd "./appveyor/deploy.ps1"

# appveyor after-deploy
# set up smoke test environmen
$env:ASPNETCORE_ENVIRONMENT = "Production"
$env:IDENTITY__SECRETKEY = "DevelopmentSecretKey"
$process = Start-Process -Passthru -FilePath dotnet -ArgumentList BellRichM.Weather.Web.dll -WorkingDirectory dist

Try
{
  $env:SMOKE_BASEURL = "https://localhost:5001"
  RunCmd "./appveyor/after_deploy_smoke.ps1"
}
Finally
{
  Stop-Process $process
}


if ($postClean -ne "NO")
{
  RunCmd "git checkout api/integration/BellRichM.Identity.Api.Integration/Data/Identity.db"
  if ($env:BUILD_ARTIFACT -ne "NO")
  {
    RunCmd "rm $env:APPVEYOR_BUILD_FOLDER/$env:ARTIFACT_NAME.zip"
  }
}
