Function RunCmd {
  Param ($cmd = $cmd)
  Process{
    Write-Host "Running: $cmd"
    Invoke-Expression $cmd
    Write-Host "Return code: $LastExitCode"

    if ($LastExitCode -ne 0)
    {
      Write-Host "Error running: $cmd"
      exit $LastExitCode
    }
  }
}

"******************************** " + $MyInvocation.InvocationName + " ********************************"
$env:COVERAGE_REPORT = "YES"

$env:BUILDPATH = "C:\Program Files\7-Zip;C:\RMBData\sonar-scanner-msbuild;$env:HOMEPATH\.nuget\packages\ReportGenerator\3.0.2\tools;"
$env:PATH = $env:PATH + $env:BUILDPATH

$env:UNIT_TEST_API = "YES"
$env:UPLOAD_COVERALLS = "NO"
$env:UPLOAD_SONARQUBE = "NO"

RunCmd "./appveyor/after_build.ps1"