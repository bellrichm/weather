# Cannot log cmd because it has the secret key
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

if ($env:UNIT_TEST -eq "NO" `
  -And $env:UPLOAD_COVERALLS -eq "NO" `
  -And $env:UPLOAD_SONARQUBE -EQ 'NO')
{
  return
}

if ($env:BUILD_PLATFORM-eq "Windows")
{
  $TESTDIR="api\test\"
  $TESTDLLS = $TESTDIR + "BellRichM.Weather.Api.Test\bin\Debug\net462\BellRichM.Weather.Api.Test.dll "
  $TESTDLLS = $TESTDLLS + $TESTDIR + "BellRichM.Identity.Api.Test\bin\Debug\net462\BellRichM.Identity.Api.Test.dll "

  $parms = ''
  $parms = $parms + '-register:user -output:opencover.xml '
  $parms = $parms + '-filter:"+[BellRichM*]* -[*]*.Migrations.* -[*.Test]*" '
  $parms = $parms + '-excludebyattribute:BellRichM.Attribute.CodeCoverage.ExcludeFromCodeCoverageAttribute '
  $parms = $parms + '-target:$env:MSPEC '
  $parms = $parms + '-targetargs:"--xml .\mspec.xml $TESTDLLS"'

  $cmd = "$env:OPENCOVER $parms"
  RunCmd $cmd

  if ($env:UPLOAD_COVERALLS -ne "NO")
  {
    $cmd = "$env:COVERALLS --opencover opencover.xml --full-sources"
    RunCmd $cmd
  }

  if ($env:UPLOAD_SONARQUBE -ne 'NO')
  {
    $parms = '/d:sonar.login=$env:SONARQUBE_REPO_TOKEN'
    $cmd = "sonarqube.scanner.msbuild.exe end $parms"
    RunCmd $cmd
  }
}

if ($env:BUILD_PLATFORM -eq "Unix" `
  -And $env:UNIT_TEST -ne "NO")
{
  $cmd = "dotnet test --no-restore --no-build -f netcoreapp2.0 api/test/BellRichM.Weather.Test.sln"
  RunCmd $cmd
}