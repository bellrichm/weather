""
"******************************** " + $MyInvocation.InvocationName + " ********************************"
Write-Host "finish"
# Note, this is not called when build crashes/exception
if ($env:REMOTE_ACCESS -ne "YES")
{
  return
}

$blockRdp = $true
Invoke-Expression ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/appveyor/ci/master/scripts/enable-rdp.ps1'))
