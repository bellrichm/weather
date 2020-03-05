""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

if ($env:RESTORE_API -eq "NO")
{
  return
}

RunCmd -cmd "dotnet restore -v m api/src/BellRichM.Weather.sln"
RunCmd -cmd "dotnet restore -v m api/test/BellRichM.Weather.Test.sln --no-dependencies"
RunCmd -cmd "dotnet restore -v m api/integration/BellRichM.Identity.Api.Integration/BellRichM.Identity.Api.Integration.csproj --no-dependencies"
RunCmd -cmd "dotnet restore -v m api/smoke/BellRichM.Identity.Api.Smoke/BellRichM.Identity.Api.Smoke.csproj --no-dependencies"
