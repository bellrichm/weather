""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

#choco upgrade "msbuild-sonarqube-runner" -y
if ($env:BUILDTYPE -ne "LOCAL")
{
    $env:TOOLDIR = "buildtools/"
    #dotnet tool install dotnet-reportgenerator-globaltool --tool-path buildtools
}
