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

# force all errors to be terminating caught by the catch
$ErrorActionPreference = "Stop"

[string]$dir = get-location
set-location app
write-host $dir
write-host $env:APPVEYOR_BUILD_FOLDER

try
{
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
}
catch
{
  #set-location $env:APPVEYOR_BUILD_FOLDER
  get-location
  write-host $_
  set-location $env:APPVEYOR_BUILD_FOLDER
  get-location
}

