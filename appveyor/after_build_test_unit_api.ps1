""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

if ($env:UNIT_TEST_API -eq "NO" `
  -And $env:UPLOAD_COVERALLS_API -eq "NO" `
  -And $env:UPLOAD_SONARQUBE_API -EQ 'NO')
{
  return
}

if ($env:BUILD_PLATFORM -eq "Unix" `
  -And $env:UNIT_TEST_API -ne "NO")
{
  $cmd = "dotnet test --no-restore --no-build -f netcoreapp2.1 api/test/BellRichM.Weather.Test.sln"
  RunCmd $cmd
}

if ($env:BUILD_PLATFORM-eq "Windows")
{
  $TESTDIR = "api\test\"
  $TESTDLLS = ""

  $TESTDLLS = $TESTDLLS + $TESTDIR + "BellRichM.Administration.Api.Test\bin\Debug\net472\BellRichM.Administration.Api.Test.dll "
  $TESTDLLS = $TESTDLLS + $TESTDIR + "BellRichM.Api.Test\bin\Debug\net472\BellRichM.Api.Test.dll "
  $TESTDLLS = $TESTDLLS + $TESTDIR + "BellRichM.Configuration.Test\bin\Debug\net472\BellRichM.Configuration.Test.dll "
  $TESTDLLS = $TESTDLLS + $TESTDIR + "BellRichM.Exceptions.Test\bin\Debug\net472\BellRichM.Exceptions.Test.dll "
  $TESTDLLS = $TESTDLLS + $TESTDIR + "BellRichM.Identity.Api.Test\bin\Debug\net472\BellRichM.Identity.Api.Test.dll "
  $TESTDLLS = $TESTDLLS + $TESTDIR + "BellRichM.Logging.Test\bin\Debug\net472\BellRichM.Logging.Test.dll "
  $TESTDLLS = $TESTDLLS + $TESTDIR + "BellRichM.Weather.Api.Test\bin\Debug\net472\BellRichM.Weather.Api.Test.dll "

  $parms = ''
  $parms = $parms + '-register:user -output:opencover.xml '
  $parms = $parms + '-filter:"+[BellRichM*]* -[*]*.Migrations.* -[*.Test]*" '
  $parms = $parms + '-excludebyattribute:BellRichM.Attribute.CodeCoverage.ExcludeFromCodeCoverageAttribute '
  $parms = $parms + '-target:$env:MSPEC '
  $parms = $parms + '-targetargs:"--xml .\mspec.xml ' + $TESTDLLS + '"'

  if ($env:UNIT_TEST_API -ne "NO")
  {
    $cmd = "$env:OPENCOVER $parms"
    RunCmd $cmd
  }

  if ($env:UPLOAD_COVERALLS_API -ne "NO")
  {
    $cmd = "$env:COVERALLS --opencover opencover.xml --full-sources"
    RunCmd $cmd
  }

  if ($env:UPLOAD_SONARQUBE_API -ne 'NO')
  {
    $parms = '/d:sonar.login=$env:SONARQUBE_REPO_TOKEN'
    $cmd = "SonarScanner.MSBuild.exe end $parms"
    RunCmd $cmd
  }

  if ($env:COVERAGE_REPORT -eq 'YES')
  {
    $parms = ''
    $parms = $parms + '"-reporttypes:Html;XmlSummary;Xml" '
  	$parms = $parms + '"-reports:opencover.xml" '
	  $parms = $parms + '"-targetdir:coverage\report" '
	  $parms = $parms + '"-historydir:coverage\report\history" '
	  $cmd = "ReportGenerator.exe $parms"
	  RunCmd $cmd
  }
}