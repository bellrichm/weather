""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

if ($env:UNIT_TEST_API -eq "NO" `
  -And $env:UPLOAD_COVERALLS_API -eq "NO" `
  -And $env:UPLOAD_SONARQUBE_API -eq "NO" `
  -And $env:COVERAGE_REPORT_API -eq "NO" `
  -And $env:RUNONLY_SONARQUBE_API -ne "YES")
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
  $parms = $parms + '/p:Exclude=\"[*]*.Migrations.*,[*.Test]*,[System.*]*\" '
  $cmd = "dotnet test --logger Appveyor --no-restore --no-build -f netcoreapp3.1 api/test/BellRichM.Weather.Test.sln" + $parms
  RunCmd $cmd
