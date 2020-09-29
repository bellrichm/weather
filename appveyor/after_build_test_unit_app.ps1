""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

if ($env:UNIT_TEST_APP -EQ "NO" `
-And $env:UPLOAD_COVERALLS_APP -eq "NO" `
-And $env:UPLOAD_SONARQUBE_APP -EQ "NO" `
-And $env:COVERAGE_REPORT_APP -eq "NO" `
-And $env:RUNONLY_SONARQUBE_APP -ne "YES")
{
  return
}

set-location app

# stderr on appveyor workaround
$cmd = "npm run-script ng test -- --progress=false --watch=false --browsers ChromeHeadless --code-coverage 2>t.txt"
RunCmd $cmd
"t.txt content beg:"
Get-Content t.txt
"t.txt content end:"
set-location ..

if ($env:BUILDTYPE -ne 'LOCAL')
{
  $wc = New-Object 'System.Net.WebClient'
  $wc.UploadFile("https://ci.appveyor.com/api/testresults/junit/$($env:APPVEYOR_JOB_ID)", (Resolve-Path ./app/output/junit.xml))
}
