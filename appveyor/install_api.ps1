""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

#choco upgrade "msbuild-sonarqube-runner" -y
dotnet tool install dotnet-reportgenerator-globaltool --tool-path buildtools