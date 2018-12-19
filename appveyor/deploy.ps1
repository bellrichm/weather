""
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