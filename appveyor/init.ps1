""
"******************************** " + $MyInvocation.InvocationName + " ********************************"
#$ErrorActionPreference = "SilentlyContinue"
try
{
  throw "test exception"
  $env:BUILD_API_LOG = '-l:"C:\Program Files\AppVeyor\BuildAgent\dotnetcore\Appveyor.MSBuildLogger.dll" '
  
  if ($env:APPVEYOR_REPO_BRANCH -eq 'local')
  {
    $env:BUILD_API_LOG = ' '
    $env:BRANCH_NAME = 'local';
    $env:ARTIFACT_NAME = 'weather-local';
    
    # if not already set, set to defaults
    if (-not (Test-Path env:UPLOAD_ARTIFACT)) { $env:UPLOAD_ARTIFACT = 'NO' }
    if (-not (Test-Path env:DEPLOY)) { $env:DEPLOY = 'NO' }
    if (-not (Test-Path env:SMOKE_TEST)) { $env:SMOKE_TEST = 'NO' }
  }
  elseif ($env:APPVEYOR_REPO_BRANCH -ne 'master')
  {
    $env:BRANCH_NAME = 'development';
    $env:ARTIFACT_NAME = 'weather-branch';
  
    # if not already set, set to defaults
    if (-not (Test-Path env:UPLOAD_ARTIFACT)) { $env:UPLOAD_ARTIFACT = 'NO' }
    if (-not (Test-Path env:DEPLOY)) { $env:DEPLOY = 'NO' }
    if (-not (Test-Path env:SMOKE_TEST)) { $env:SMOKE_TEST = 'NO' }
          
    # if 'development' build project, set build version to a guid
    $buildnum =  New-Guid;
    $buildnum = [int64](([datetime]::UtcNow)-(get-date "1/1/1970")).TotalSeconds
    Update-AppveyorBuild -Version "$buildnum";
    
    # then reset build number
    $headers = @{
      "Authorization" = "Bearer $env:APPVEYOR_TOKEN";
      "Content-type" = "application/json";
      "Accept" = "application/json";
    }
    
    $build = @{
      nextBuildNumber = $env:APPVEYOR_BUILD_NUMBER
    }
              
    $json = $build | ConvertTo-Json;
    Invoke-RestMethod -Method Put "$env:APPVEYOR_URL/api/projects/$env:APPVEYOR_ACCOUNT_NAME/$env:APPVEYOR_PROJECT_SLUG/settings/build-number" -Body $json -Headers $headers
    }
  else
  {
    $env:BRANCH_NAME = 'master';
    $env:ARTIFACT_NAME = 'weather';
    
    # if not already set, set to defaults
    if (-not (Test-Path env:UPLOAD_ARTIFACT)) { $env:UPLOAD_ARTIFACT = 'YES' }
    if (-not (Test-Path env:DEPLOY)) { $env:DEPLOY = 'YES' }
    if (-not (Test-Path env:SMOKE_TEST)) { $env:SMOKE_TEST = 'YES' }
  }
    
  try
  {
      if ([System.Boolean](Get-CimInstance -ClassName Win32_OperatingSystem -ErrorAction Ignore))
      {
          $env:BUILD_PLATFORM = "Windows"
          #$Env:CHROME_BIN = "C:\Program Files\Google\Chrome\Application\chrome.exe" # google install location for karma on appveyor
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
  Write-Host "COVERAGE_REPORT_API:      $env:COVERAGE_REPORT_API"
  Write-Host "COVERAGE_REPORT_APP:      $env:COVERAGE_REPORT_APP"
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
  Write-Host "RUNONLY_SONARQUBE_API:    $env:RUNONLY_SONARQUBE_API"
  Write-Host "RUNONLY_SONARQUBE_APP:    $env:RUNONLY_SONARQUBE_APP"
  Write-Host "UPLOAD_ARTIFACT:          $env:UPLOAD_ARTIFACT"
  Write-Host "DEPLOY:                   $env:DEPLOY"
  Write-Host "SMOKE_TEST:               $env:SMOKE_TEST"
  Write-Host ""
  
  exit 1
}
catch
{
  Write-Host $_
  exit 1
  # Exit-AppveyorBuild # terminates with success
}