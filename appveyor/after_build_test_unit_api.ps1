""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

if ($env:UNIT_TEST_API -eq "NO" `
  -And $env:UPLOAD_COVERALLS_API -eq "NO" `
  -And $env:UPLOAD_SONARQUBE_API -EQ 'NO')
{
  return
}

  # ToDo - fix indentatiom
  Remove-Item api/test/coverlet -Force -Recurse -ErrorAction Ignore
  $parms = ' --% '
  $parms = $parms + '/p:CollectCoverage=true '
  $parms = $parms + '/p:CoverletOutputFormat=\"json,opencover\" '
  $parms = $parms + '/p:CoverletOutput=../coverlet/ '
  $parms = $parms + '/p:MergeWith=../coverlet/coverage.netcoreapp3.1.json '
  $parms = $parms + '/p:Exclude=\"[*]*.Migrations.*,[*.Test]*\" '
  $cmd = "dotnet test --no-restore --no-build -f netcoreapp3.1 api/test/BellRichM.Weather.Test.sln" + $parms
  RunCmd $cmd

  # ToDo - don't hardcode coverlet filename
  if ($env:COVERAGE_REPORT -eq 'YES')
  {  
    $parms = ''
    $parms = $parms + '"-reporttypes:Html;XmlSummary;Xml" '
    $parms = $parms + '"-reports:api/test/coverlet/coverage.netcoreapp3.1.opencover.xml" '
    $parms = $parms + '"-targetdir:coverage/report" '
    $parms = $parms + '"-historydir:coverage/report/history" '
    $parms = $parms + '-verbosity:Warning '
    $cmd = $env:TOOLDIR + "reportgenerator $parms"
    RunCmd $cmd
  }

  if ($env:UPLOAD_COVERALLS_API -ne "NO")
  {
    $cmd = $env:TOOLDIR + "csmacnz.Coveralls --opencover -i api/test/coverlet/coverage.netcoreapp3.1.opencover.xml --useRelativePaths"
    RunCmd $cmd  
  }

  # ToDo - option to run, but not upload?
  if ($env:UPLOAD_SONARQUBE_API -ne 'NO')
  {
    $parms = '/d:sonar.login=$env:SONARQUBE_REPO_TOKEN'
    $cmd = $env:TOOLDIR + "dotnet-sonarscanner end $parms"
    RunCmd $cmd
  }
