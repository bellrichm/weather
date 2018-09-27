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
      Write-Host $error
      exit 1
    }
  }
}

"******************************** " + $MyInvocation.InvocationName + " ********************************"
if ($env:BUILD -eq "NO")
{
  return
}

if ($env:BUILD_PLATFORM -eq "Windows")
{
  $unitTestFramework = '-f net462 '
}
else
{
    $unitTestFramework = '-f netcoreapp2.0 '
    $buildFramework = '-f netcoreapp2.0 '
}

if ($env:BUILD_PLATFORM-eq "Windows" `
  -And $env:UPLOAD_SONARQUBE -ne 'NO')
{
  $parms = ''
  $parms = $parms + 'begin '
  $parms = $parms + '/k:"weather" '
  $parms = $parms + '/o:"bellrichm" '
  $parms = $parms + '/v:$env:APPVEYOR_BUILD_VERSION '
  $parms = $parms + '/d:sonar.branch.name=$env:BRANCH_NAME '
  $parms = $parms + '/d:sonar.host.url="https://sonarcloud.io" '
  $parms = $parms + '/d:sonar.login=$env:SONARQUBE_REPO_TOKEN '
  $parms = $parms + '/d:sonar.exclusions="**/Migrations/*, **/obj/**/*" '
  $parms = $parms + '/d:sonar.test.exclusions="**/obj/**/*" '
  $parms = $parms + '/d:sonar.cs.opencover.reportsPaths="opencover.xml" '
  # $parms = $parms + '/d:sonar.verbose=true '

  $cmd = "SonarScanner.MSBuild.exe $parms"
  RunCmd $cmd
}

$defBuildParams = '--no-restore --no-dependencies '

$cmd = "dotnet build api\src\BellRichM.Weather.sln --no-restore $buildFramework"
RunCmd $cmd

$cmd = "dotnet build api\test\BellRichM.Weather.Test.sln $defBuildParams $unitTestFramework"
RunCmd $cmd

$cmd = "dotnet build api/integration/BellRichM.Identity.Api.Integration/BellRichM.Identity.Api.Integration.csproj $defBuildParams "
RunCmd $cmd

$cmd = "dotnet build api/smoke/BellRichM.Identity.Api.Smoke/BellRichM.Identity.Api.Smoke.csproj $defBuildParams "
RunCmd $cmd