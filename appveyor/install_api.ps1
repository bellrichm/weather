""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

#choco upgrade "msbuild-sonarqube-runner" -y
if ($env:BUILDTYPE -ne "LOCAL")
{
    $env:TOOLDIR = "buildtools/"
    dotnet tool install dotnet-reportgenerator-globaltool --tool-path buildtools
    dotnet tool install coveralls.net --version 1.0.0 --tool-path buildtools
}
