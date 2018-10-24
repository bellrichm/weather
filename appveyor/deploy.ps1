Function RunCmd {
  Param ($cmd = $cmd)
  Process{
    Write-Host "Running: $cmd"
    Invoke-Expression $cmd
    Write-Host "Return code: $LastExitCode"

    if ($LastExitCode -ne 0)
    {
      Write-Host "Error running: $cmd"
      if ([string]::IsNullOrEmpty($LastExitCode))
      {
        exit 1
      }

      exit $LastExitCode
    }

    if ( -Not [string]::IsNullOrEmpty($error))
    {
      Write-Host "Error running: $cmd"
      Write-Host $error
      exit 1
    }
  }
}

"******************************** " + $MyInvocation.InvocationName + " ********************************"

if ($env:DEPLOY -eq "NO")
{
  return
}

if ($env:BUILD_PLATFORM -eq "Windows")
{
  $parms = ''
  $parms = $parms + '-verb:sync '
  $parms = $parms + '-useChecksum '
  $parms = $parms + '-source:package=$env:APPVEYOR_BUILD_FOLDER/$env:ARTIFACT_NAME.zip '
  $parms = $parms + '-skip:Directory=\\Data '
  $parms = $parms + '-skip:Directory=\\logs '
  $parms = $parms + '-enableRule:AppOffline '
  $parms = $parms + '-allowUntrusted '
  $parms = $parms + '-dest:auto,'
  $parms = $parms + 'computerName=$env:AZURE_SERVER,'
  $parms = $parms + 'UserName=$env:AZURE_USER,'
  $parms = $parms + 'Password=$env:AZURE_PW,'
  $parms = $parms + 'AuthType=Basic'

  $cmd = "& `" C:\Program Files (x86)\IIS\Microsoft Web Deploy V3\msdeploy.exe`" $parms"
  RunCmd $cmd
}