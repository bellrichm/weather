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
$env:COVERAGE_REPORT_APP = "NO"
$env:RUNONLY_SONARQUBE_API = "YES"
$env:RUNONLY_SONARQUBE_APP = "NO"

##$env:BUILDPATH = ";C:\Program Files\7-Zip;C:\RMBData\sonar-scanner-msbuild;$env:HOMEPATH\.nuget\packages\ReportGenerator\3.0.2\tools;"
##$env:PATH = $env:PATH + $env:BUILDPATH

$env:PRECLEAN = "YES"
$env:POSTCLEAN = "YES"

$env:ARTIFACT_NAME = "weather-branch"
$env:BRANCH_NAME = "local"

$env:RESTORE_API = "YES"
$env:RESTORE_APP = "NO"
$env:BUILD_API = "YES"
$env:BUILD_APP = "NO"
$env:UNIT_TEST_API = "YES"
$env:UNIT_TEST_APP = "NO"
$env:INTEGRATION_TEST_API = "YES"
$env:INTEGRATION_TEST_APP = "unused"
$env:BUILD_ARTIFACT = "NO"
$env:SMOKE_TEST = "NO"
# Unless testing the process, these should usually be set to NO
# If APPVEYOR_REPO_BRANCH is set to "local", then it is safe to have these set to "YES"
$env:UPLOAD_COVERALLS_API = "NO"
$env:UPLOAD_COVERALLS_APP = "NO" 
$env:UPLOAD_SONARQUBE_API = "NO"
$env:UPLOAD_SONARQUBE_APP = "NO"

$env:UPLOAD_ARTIFACT = "NO"
$env:DEPLOY = "NO"

RunCmd "./buildflow.ps1"