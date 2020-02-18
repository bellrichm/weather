""
"******************************** " + $MyInvocation.InvocationName + " ********************************"
$env:TOOLDIR = "buildtools/"

if ($env:COVERAGE_REPORT -eq 'YES')
{
    $cmd = "dotnet tool install dotnet-reportgenerator-globaltool --tool-path buildtools/"
    RunCmd $cmd
}

if ($env:UPLOAD_COVERALLS_API -ne "NO")
{
    $cmd = "dotnet tool install coveralls.net --version 1.0.0 --tool-path buildtools/"
    RunCmd $cmd    
}

if ($env:UPLOAD_SONARQUBE_API -ne 'NO')
{
    $cmd = "dotnet tool install dotnet-sonarscanner --tool-path buildtools/"
    RunCmd $cmd    
}
