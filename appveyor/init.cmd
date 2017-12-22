echo "******************************** Init ********************************"

set AZURE_USER=$bellrichm-weather
set AZURE_SITE=bellrichm-weather
set AZURE_SERVER=https://bellrichm-weather.scm.azurewebsites.net:443/msdeploy.axd?site=bellrichm-weather

set MSPEC=%HOMEPATH%\.nuget\packages\Machine.Specifications.Runner.Console\0.9.3\tools\mspec-clr4.exe
set OPENCOVER=%HOMEPATH%\.nuget\packages\OpenCover\4.6.519\tools\OpenCover.Console.exe
set COVERALLS=%HOMEPATH%\.nuget\packages\coveralls.io\1.4.2\tools\coveralls.net.exe