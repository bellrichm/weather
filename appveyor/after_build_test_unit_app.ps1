""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

if ($env:UNIT_TEST_APP -EQ "NO" `
  -And $env:UPLOAD_COVERALLS -eq "NO" )
{
  return
}

if ($env:UNIT_TEST_APP -ne "NO")
{
  set-location app
  # ToDo - where in build process to put this
  # stderr on appveyor workaround
  $cmd = "npm run-script ng build -- --progress=false --aot=true --prod=true 2>t.txt"
  RunCmd $cmd
  Get-Content t.txt

  # stderr on appveyor workaround
  $cmd = "npm run-script ng test -- --progress=false --watch=false --browsers ChromeHeadless --code-coverage 2>t.txt"
  RunCmd $cmd
  Get-Content t.txt
  set-location ..
}

if ($env:UPLOAD_COVERALLS_APP -ne "NO")
{
  Get-Content app/coverage/lcov.info | & node ./app/node_modules/coveralls/bin/coveralls.js
}
