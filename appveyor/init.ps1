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

$env:AZURE_USER = '$bellrichm-weather'
$env:AZURE_SITE = "bellrichm-weather"
$env:AZURE_SERVER = "https://bellrichm-weather.scm.azurewebsites.net:443/msdeploy.axd?site=bellrichm-weather"

$env:MSPEC = "$env:HOMEDRIVE$env:HOMEPATH\.nuget\packages\Machine.Specifications.Runner.Console\0.9.3\tools\mspec-clr4.exe"
$env:OPENCOVER = "$env:HOMEDRIVE$env:HOMEPATH\.nuget\packages\OpenCover\4.6.519\tools\OpenCover.Console.exe"
$env:COVERALLS = "$env:HOMEDRIVE$env:HOMEPATH\.nuget\packages\coveralls.io\1.4.2\tools\coveralls.net.exe"

Write-Host "******************************** Environment ********************************"
Write-Host "BUILD_PLATFORM:         $env:BUILD_PLATFORM"
Write-Host "AZURE_USER:             $env:AZURE_USER"
Write-Host "AZURE_SITE:             $env:AZURE_SITE"
Write-Host "AZURE_SERVER:           $env:AZURE_SERVER"
Write-Host "MSPEC:                  $env:MSPEC"
Write-Host "OPENCOVER:              $env:OPENCOVER"
Write-Host "COVERALLS:              $env:COVERALLS"
Write-Host "APPVEYOR_BUILD_VERSION: $env:APPVEYOR_BUILD_VERSION"
Write-Host "APPVEYOR_BUILD_FOLDER:  $env:APPVEYOR_BUILD_FOLDER"
Write-Host "COVERAGE_REPORT:        $env:COVERAGE_REPORT"
Write-Host "BUILDPATH:              $env:BUILDPATH"
Write-Host "PATH:                   $env:PATH"
Write-Host "ARTIFACT_NAME:          $env:ARTIFACT_NAME"
Write-Host "BRANCH_NAME:            $env:BRANCH_NAME"
Write-Host "RESTORE:                $env:RESTORE"
Write-Host "BUILD:                  $env:BUILD"
Write-Host "UNIT_TEST:              $env:UNIT_TEST"
Write-Host "INTEGRATION_TEST:       $env:INTEGRATION_TEST"
Write-Host "BUILD_ARTIFACT:         $env:BUILD_ARTIFACT"
Write-Host "UPLOAD_COVERALLS:       $env:UPLOAD_COVERALLS"
Write-Host "UPLOAD_SONARQUBE:       $env:UPLOAD_SONARQUBE"
Write-Host "UPLOAD_ARTIFACT:        $env:UPLOAD_ARTIFACT"
Write-Host "DEPLOY:                 $env:DEPLOY"
Write-Host "SMOKE_TEST:             $env:SMOKE_TEST"
Write-Host ""

exit 0