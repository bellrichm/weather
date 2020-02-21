""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

if ($env:BUILD_APP -eq "NO")
{
  return
}

set-location app
# ToDo - stderr work around...
$cmd = "npm run-script ng build -- --progress=false --aot=true --prod=true 2>t.txt"
RunCmd $cmd
"t.txt content beg:"
Get-Content t.txt
"t.txt content end:"
set-location ..  
