""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

Install-Product node 10
choco install googlechrome

$cmd = "type 'C:\ProgramData\chocolatey\logs\chocolatey.log'"
# RunCmd $cmd