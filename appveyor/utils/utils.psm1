# be careful to not resolve environment variables - they may have sensitive information, like passwords
Function RunCmd {
  Param ($cmd = $cmd)
  Process{
    Write-Host "Running: $cmd"
    
    Invoke-Expression $cmd
    $rc = $LastExitCode

    if ($env:BUILDTYPE -eq 'LOCAL')
    {
      Write-Host "Return code: $rc"
    }
    else 
    {
      Add-AppveyorMessage -Category Information "Completed: $cmd" -Details "return code: $rc"
    }

    if ($rc -ne 0)
    {
      if ($env:BUILDTYPE -eq 'LOCAL')
      {
        Write-Host "Error running: $cmd"
      }
      else 
      {
        Add-AppveyorMessage -Category Error "Error running: $cmd" -Details "return code: $rc"
        throw
      }
      exit $rc
    }
  }
}

