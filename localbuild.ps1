Import-Module ./appveyor/utils

"******************************** " + $MyInvocation.InvocationName + " ********************************"
$error.Clear()

$env:BUILDTYPE = "LOCAL"

$env:APPVEYOR_BUILD_VERSION = "local"
$env:APPVEYOR_BUILD_FOLDER =  Split-Path $MyInvocation.MyCommand.Path

# pretend its an appveyor build for coveralls.io/coveralls.net.exe
$env:APPVEYOR = "true"
$env:APPVEYOR_JOB_ID = get-random

$env:APPVEYOR_REPO_BRANCH = "local"
# $env:APPVEYOR_REPO_COMMIT=
# $env:APPVEYOR_REPO_COMMIT_MESSAGE =
# $env:APPVEYOR_REPO_COMMIT_AUTHOR =
# $env:APPVEYOR_REPO_COMMIT_AUTHOREMAIL =
# $env:APPVEYOR_PULL_REQUEST_NUMBER =


Invoke-Expression -Command ./localtools/init.ps1

$env:COVERAGE_REPORT_API = "YES"
$env:COVERAGE_REPORT_APP = "YES"
$env:RUNONLY_SONARQUBE_API = "YES"
$env:RUNONLY_SONARQUBE_APP = "YES"

$env:PRECLEAN = "YES"
$env:POSTCLEAN = "YES"

$env:RESTORE_API = "YES"
$env:RESTORE_APP = "YES"
$env:BUILD_API = "YES"
$env:BUILD_APP = "YES"
$env:UNIT_TEST_API = "YES"
$env:UNIT_TEST_APP = "YES"
$env:INTEGRATION_TEST_API = "YES"
$env:INTEGRATION_TEST_APP = "unused"
$env:BUILD_ARTIFACT = "YES"
# Unless testing the process, these should usually be set to NO
# If APPVEYOR_REPO_BRANCH is set to "local", then it is safe to have these set to "YES"
$env:UPLOAD_COVERALLS_API = "NO"
$env:UPLOAD_COVERALLS_APP = "NO" 
$env:UPLOAD_SONARQUBE_API = "NO"
$env:UPLOAD_SONARQUBE_APP = "NO"

RunCmd "./buildflow.ps1"
