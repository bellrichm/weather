""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

if ($env:RESTORE_APP -eq "NO")
{
  return
}

RunCmd -cmd "dotnet restore -v m app/BellRichM.App.csproj"

set-location app
RunCmd -cmd "npm install --loglevel=error --no-fund"
set-location ..