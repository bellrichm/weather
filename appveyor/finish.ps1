""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

if ($env:REMOTE_ACCESS -ne "YES")
{
  return
}

$blockRdp = $true
Invoke-Expression ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/appveyor/ci/master/scripts/enable-rdp.ps1'))
