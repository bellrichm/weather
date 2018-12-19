""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

if ($env:BUILD_APP -eq "NO")
{
  return
}
