""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

if ($env:SMOKE_TEST -eq "NO")
{
  return
}

$cmd = "dotnet test --no-restore --no-build api/smoke/BellRichM.Identity.Api.Smoke/BellRichM.Identity.Api.Smoke.csproj"
RunCmd $cmd