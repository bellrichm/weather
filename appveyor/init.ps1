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
exit 0