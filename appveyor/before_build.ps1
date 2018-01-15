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

if ($env:RESTORE -eq "NO")
{
  return
}

RunCmd -cmd "dotnet restore api\src\BellRichM.Weather.sln"
RunCmd -cmd "dotnet restore api\test\BellRichM.Weather.Test.sln --no-dependencies"
RunCmd -cmd "dotnet restore api/integration/BellRichM.Identity.Api.Integration/BellRichM.Identity.Api.Integration.csproj --no-dependencies"
RunCmd -cmd "dotnet restore api/smoke/BellRichM.Identity.Api.Smoke/BellRichM.Identity.Api.Smoke.csproj --no-dependencies"