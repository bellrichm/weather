""
"******************************** " + $MyInvocation.InvocationName + " ********************************"
if ($env:BUILD_API -eq "NO")
{
  return
}

if ($env:BUILD_PLATFORM -eq "Windows")
{
  $unitTestFramework = '-f net472 '
}
else
{
    $unitTestFramework = '-f netcoreapp2.1 '
    $buildFramework = '-f netcoreapp2.1 '
}

if ($env:BUILD_PLATFORM-eq "Windows" `
  -And $env:UPLOAD_SONARQUBE_API -ne 'NO')
{
  $parms = ''
  $parms = $parms + 'begin '
  $parms = $parms + '/k:"weather" '
  $parms = $parms + '/o:"bellrichm" '
  $parms = $parms + '/v:$env:APPVEYOR_BUILD_VERSION '
  $parms = $parms + '/d:sonar.branch.name=$env:BRANCH_NAME '
  $parms = $parms + '/d:sonar.host.url="https://sonarcloud.io" '
  $parms = $parms + '/d:sonar.login=$env:SONARQUBE_REPO_TOKEN '
  $parms = $parms + '/d:sonar.exclusions="**/Migrations/*, **/obj/**/*, **/*.conf.*, **/e2e/**/*, **/coverage/**/*, **/*spec*" '
  $parms = $parms + '/d:sonar.cpd.exclusions="**/Models/*" '
  $parms = $parms + '/d:sonar.test.exclusions="**/obj/**/*" '
  $parms = $parms + '/d:sonar.cs.opencover.reportsPaths="opencover.xml" '
  $parms = $parms + '/d:sonar.typescript.lcov.reportPaths="../app/coverage/lcov.info" '
  # $parms = $parms + '/d:sonar.verbose=true '

  $cmd = "SonarScanner.MSBuild.exe $parms"
  RunCmd $cmd
}

$defBuildParams = '--no-restore '


$cmd = "dotnet build api\src\BellRichM.Weather.sln --no-restore $buildFramework"
RunCmd $cmd

$cmd = "dotnet build api\test\BellRichM.Weather.Test.sln $defBuildParams $unitTestFramework"
RunCmd $cmd

$cmd = "dotnet build api/integration/BellRichM.Identity.Api.Integration/BellRichM.Identity.Api.Integration.csproj $defBuildParams "
RunCmd $cmd

$cmd = "dotnet build api/smoke/BellRichM.Identity.Api.Smoke/BellRichM.Identity.Api.Smoke.csproj $defBuildParams "
RunCmd $cmd
