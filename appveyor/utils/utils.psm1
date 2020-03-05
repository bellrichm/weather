# be careful to not resolve environment variables - they may have sensitive information, like passwords
Function RunCmd {
  Param ($cmd = $cmd)
  Process{
    if ($env:BUILDTYPE -eq 'LOCAL')
    {
      Write-Host "Running: $cmd"
    }
    else 
    {
      Add-AppveyorMessage -Category Information "Running: $cmd"
    }
    
    Invoke-Expression $cmd
    $rc = $LastExitCode
    if ($env:BUILDTYPE -eq 'LOCAL')
    {
      Write-Host "Return code: $rc"
    }
    else 
    {
      Add-AppveyorMessage -Category Information "Return code: $rc"
    }

    if ($rc -ne 0)
    {
      if ($env:BUILDTYPE -eq 'LOCAL')
      {
        Write-Host "Error running: $cmd"
      }
      else 
      {
        Add-AppveyorMessage -Category Information "Error running: $cmd"
      }
      exit $rc
    }
  }
}

