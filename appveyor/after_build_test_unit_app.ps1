""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

if ($env:UNIT_TEST_APP -EQ "NO" `
  -And $env:UPLOAD_COVERALLS -eq "NO" )
{
  return
}