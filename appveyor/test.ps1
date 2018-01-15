Function RunCmd {
  Param ($cmd = $cmd)
  Process{
    Write-Host "Running: $cmd"
    Invoke-Expression $cmd
    Write-Host "Return code: $LastExitCode"

    if ($LastExitCode -ne 0)
    {
      Write-Host "Error running: $cmd"
      exit $LastExitCode
    }
  }
}

"******************************** " + $MyInvocation.InvocationName + " ********************************"

if ($env:INTEGRATION_TEST -eq "NO")
{
  return
}

$cmd = "dotnet test --no-restore --no-build api/integration/BellRichM.Identity.Api.Integration/BellRichM.Identity.Api.Integration.csproj"
RunCmd $cmd