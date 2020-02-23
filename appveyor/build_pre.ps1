""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

if ($env:UPLOAD_SONARQUBE_API -ne 'NO' `
-Or $env:RUNONLY_SONARQUBE_API -eq 'YES')
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
  $parms = $parms + '/d:sonar.cs.opencover.reportsPaths="api/test/coverlet/coverage.netcoreapp3.1.opencover.xml" '
  $parms = $parms + '/d:sonar.log.level=WARN '
  # $parms = $parms + '/d:sonar.verbose=true '
  if ($env:UPLOAD_SONARQUBE_APP -ne 'NO' `
  -Or $env:RUNONLY_SONARQUBE_API -eq 'YES')
  {
    $parms = $parms + '/d:sonar.typescript.lcov.reportPaths="../app/coverage/lcov.info" '
  }

  $cmd =  $env:TOOLDIR + "dotnet-sonarscanner $parms"
  RunCmd $cmd
}