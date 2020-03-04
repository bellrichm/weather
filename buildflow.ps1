RunCmd "dotnet build-server shutdown"

if ($env:PRECLEAN -ne "NO")
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
  ##RunCmd "dotnet clean -v m app/BellRichM.App.csproj" ToDo - uncomment when app developed
  RunCmd "dotnet clean -v m api/test/BellRichM.Weather.Test.sln"
  RunCmd "dotnet clean -v m api/src/BellRichM.Weather.sln"
  RunCmd "dotnet clean -v m api/integration/BellRichM.Identity.Api.Integration/BellRichM.Identity.Api.Integration.csproj"
  RunCmd "dotnet clean -v m api/smoke/BellRichM.Identity.Api.Smoke/BellRichM.Identity.Api.Smoke.csproj"
  Remove-Item $env:APPVEYOR_BUILD_FOLDER/dist -Force -Recurse -ErrorAction Ignore
  Remove-Item $env:APPVEYOR_BUILD_FOLDER/$env:ARTIFACT_NAME.zip -ErrorAction Ignore
  Remove-Item $env:APPVEYOR_BUILD_FOLDER/dist-linux-arm -Force -Recurse -ErrorAction Ignore
  Remove-Item $env:APPVEYOR_BUILD_FOLDER/$env:ARTIFACT_NAME-linux-arm.zip -ErrorAction Ignore
}

# appveyor install
RunCmd " ./appveyor/init.ps1"

# appveyor before-build
RunCmd "./appveyor/before_build_api.ps1"
RunCmd "./appveyor/before_build_app.ps1"

# appveyor build-script
RunCmd "./appveyor/build_pre.ps1"
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

if ($env:DEPLOY -ne "NO")
{
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
}


if ($env:POSTCLEAN -ne "NO")
{
  RunCmd "git checkout api/integration/BellRichM.Identity.Api.Integration/Data/Identity.db"
  if ($env:BUILD_ARTIFACT -ne "NO")
  {
    RunCmd "rm $env:APPVEYOR_BUILD_FOLDER/$env:ARTIFACT_NAME.zip"
    RunCmd "rm $env:APPVEYOR_BUILD_FOLDER/$env:ARTIFACT_NAME-linux-arm.zip"
  }
}
