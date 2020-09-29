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

$blockRdp = $true
Invoke-Expression ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/appveyor/ci/master/scripts/enable-rdp.ps1'))

#$cmd = "dir 'C:\Program Files (x86)'"
#RunCmd $cmd

$cmd = "dir 'C:\Program Files (x86)\Google'"
RunCmd $cmd

$cmd = "dir 'C:\Program Files (x86)\Google\Chrome'"
RunCmd $cmd

$cmd = "dir 'C:\Program Files (x86)\Google\Chrome\Application'"
RunCmd $cmd

$cmd = "dir 'C:\Program Files (x86)\Google\Chrome\Application'"
RunCmd $cmd

$cmd = "dir 'C:\Program Files (x86)\Google\Chrome\Application\chrome.exe'"
RunCmd $cmd

$cmd = "'C:\Program Files (x86)\Google\Chrome\Application\chrome.exe --user-data-dir=C:\Users\appveyor\AppData\Local\Temp\1\karma-45328791 --no-default-browser-check --no-first-run --disable-default-apps --disable-popup-blocking --disable-translate --disable-background-timer-throttling --disable-renderer-backgrounding --disable-device-discovery-notifications --disable-web-security --disable-gpu --enable-logging=stdout  --v=1 --no-sandbox http://localhost:9876/?id=45328791 --headless --disable-gpu --remote-debugging-port=9222'"
RunCmd $cmd

# stderr on appveyor workaround
# $cmd = "npm run-script ng test -- --progress=false --watch=false --browsers ChromeHeadless --code-coverage --source-map=false 2>t.txt"
$cmd = "npm run-script ng test -- --progress=false --watch=false --browsers ChromeHeadlessCI --code-coverage --source-map=false"
#RunCmd $cmd
"t.txt content beg:"
Get-Content t.txt
"t.txt content end:"
set-location ..

if ($env:BUILDTYPE -ne 'LOCAL')
{
  $wc = New-Object 'System.Net.WebClient'
  $wc.UploadFile("https://ci.appveyor.com/api/testresults/junit/$($env:APPVEYOR_JOB_ID)", (Resolve-Path ./app/output/junit.xml))
}
