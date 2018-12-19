""
"******************************** " + $MyInvocation.InvocationName + " ********************************"
try
{
    if ([System.Boolean](Get-CimInstance -ClassName Win32_OperatingSystem -ErrorAction Ignore))
    {
        $env:BUILD_PLATFORM = "Windows"
    }
}
catch
{
    $error.clear()
    $env:BUILD_PLATFORM = "Other"
}

if ($PSVersionTable.Platform -eq "Unix")
{
    $env:BUILD_PLATFORM = "Unix"
}

$env:PSModulePath = $env:APPVEYOR_BUILD_FOLDER +"/appveyor;" + $env:PSModulePath

$env:AZURE_USER = '$bellrichm-weather'
$env:AZURE_SITE = "bellrichm-weather"
$env:AZURE_SERVER = "https://bellrichm-weather.scm.azurewebsites.net:443/msdeploy.axd?site=bellrichm-weather"

$env:MSPEC = "$env:HOMEDRIVE$env:HOMEPATH\.nuget\packages\Machine.Specifications.Runner.Console\0.9.3\tools\mspec-clr4.exe"
$env:OPENCOVER = "$env:HOMEDRIVE$env:HOMEPATH\.nuget\packages\OpenCover\4.6.519\tools\OpenCover.Console.exe"
$env:COVERALLS = "$env:HOMEDRIVE$env:HOMEPATH\.nuget\packages\coveralls.io\1.4.2\tools\coveralls.net.exe"

$env:COVERALLS_SERVICE_JOB_ID = $env:APPVEYOR_JOB_ID
$env:COVERALLS_SERVICE_NAME = "appveyor"
$env:COVERALLS_PARALLEL = "TRUE"

Write-Host "******************************** Environment ********************************"
Write-Host "BUILD_PLATFORM:           $env:BUILD_PLATFORM"
Write-Host "AZURE_USER:               $env:AZURE_USER"
Write-Host "AZURE_SITE:               $env:AZURE_SITE"
Write-Host "AZURE_SERVER:             $env:AZURE_SERVER"
Write-Host "MSPEC:                    $env:MSPEC"
Write-Host "OPENCOVER:                $env:OPENCOVER"
Write-Host "COVERALLS:                $env:COVERALLS"
Write-Host "APPVEYOR_BUILD_VERSION:   $env:APPVEYOR_BUILD_VERSION"
Write-Host "APPVEYOR_BUILD_FOLDER:    $env:APPVEYOR_BUILD_FOLDER"
Write-Host "COVERALLS_SERVICE_JOB_ID: $env:COVERALLS_SERVICE_JOB_ID"
Write-Host "COVERALLS_SERVICE_NAME:   $env:COVERALLS_SERVICE_NAME"
Write-Host "COVERALLS_PARALLEL:       $env:COVERALLS_PARALLEL"
Write-Host "COVERAGE_REPORT:          $env:COVERAGE_REPORT"
Write-Host "BUILDPATH:                $env:BUILDPATH"
Write-Host "PATH:                     $env:PATH"
Write-Host "PSModulePath:             $env:PSModulePath"
Write-Host "ARTIFACT_NAME:            $env:ARTIFACT_NAME"
Write-Host "BRANCH_NAME:              $env:BRANCH_NAME"
Write-Host "RESTORE:                  $env:RESTORE"
Write-Host "BUILD_API:                $env:BUILD_API"
Write-Host "BUILD_APP:                $env:BUILD_APP"
Write-Host "UNIT_TEST_API:            $env:UNIT_TEST_API"
Write-Host "UNIT_TEST_APP:            $env:UNIT_TEST_APP"
Write-Host "INTEGRATION_TEST_API:     $env:INTEGRATION_TEST_API"
Write-Host "INTEGRATION_TEST_APP:     $env:INTEGRATION_TEST_APP"
Write-Host "BUILD_ARTIFACT:           $env:BUILD_ARTIFACT"
Write-Host "UPLOAD_COVERALLS_API:     $env:UPLOAD_COVERALLS_API"
Write-Host "UPLOAD_COVERALLS_APP:     $env:UPLOAD_COVERALLS_APP"
Write-Host "UPLOAD_SONARQUBE_API:     $env:UPLOAD_SONARQUBE_API"
Write-Host "UPLOAD_SONARQUBE_APP:     $env:UPLOAD_SONARQUBE_APP"
Write-Host "UPLOAD_ARTIFACT:          $env:UPLOAD_ARTIFACT"
Write-Host "DEPLOY:                   $env:DEPLOY"
Write-Host "SMOKE_TEST:               $env:SMOKE_TEST"
Write-Host ""

exit 0