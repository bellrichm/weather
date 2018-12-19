""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

if ($env:INTEGRATION_TEST_API -eq "NO")
{
  return
}

$cmd = "dotnet test --no-restore --no-build api/integration/BellRichM.Identity.Api.Integration/BellRichM.Identity.Api.Integration.csproj"
RunCmd $cmd