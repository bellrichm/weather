""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

if ($env:BUILD_APP -eq "NO")
{
  return
}

$cmd = "dotnet build app\BellRichM.App.csproj --no-restore"
RunCmd $cmd
