""
"******************************** " + $MyInvocation.InvocationName + " ********************************"
$env:TOOLDIR = "buildtools/"

# ToDo - move these to an app/api agnostic install
if ($env:COVERAGE_REPORT_API -eq 'YES' `
  -Or $env:COVERAGE_REPORT_APP -eq 'YES')
{
    $cmd = "dotnet tool install dotnet-reportgenerator-globaltool --tool-path buildtools/"
    RunCmd $cmd
}

if ($env:UPLOAD_COVERALLS_API -ne 'NE' `
  -And $env:UPLOAD_COVERALLS_APP -ne 'NO')
{
    $cmd = "dotnet tool install coveralls.net --version 1.0.0 --tool-path buildtools/"
    RunCmd $cmd    
}

if (($env:UPLOAD_SONARQUBE_API -ne 'NO' `
  -And $env:UPLOAD_SONARQUBE_APP -ne 'NO') `
  -Or $env:RUNONLY_SONARQUBE_API -eq 'YES' `
  -Or $env:RUNONLY_SONARQUBE_APP -eq 'YES')
{
    $cmd = "dotnet tool install dotnet-sonarscanner --tool-path buildtools/"
    RunCmd $cmd    
}
