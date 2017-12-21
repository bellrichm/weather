rem TODO: check return codes of build steps

SET ARTIFACT_NAME=weather
set BRANCH_NAME=master
set DEPLOY=YES

set APPVEYOR_BUILD_VERSION=local
set APPVEYOR_BUILD_FOLDER=C:\RMBData\VSCode\weather\master

set buildpath=C:\Program Files\7-Zip;C:\RMBData\sonar-scanner-msbuild;
set path=%path%;%buildpath%

call localtools\init.cmd

call appveyor\init.cmd

dotnet clean api\test\BellRichM.Weather.Test.sln
dotnet clean api\src\BellRichM.Weather.sln
dotnet clean api/integration/BellRichM.Identity.Api.Integration/BellRichM.Identity.Api.Integration.csproj
dotnet clean api/smoke/BellRichM.Identity.Api.Smoke/BellRichM.Identity.Api.Smoke.csproj 
rmdir %APPVEYOR_BUILD_FOLDER%\dist /s /q
del %APPVEYOR_BUILD_FOLDER%\%ARTIFACT_NAME%.zip 

call appveyor\before_build.cmd

call appveyor\build.cmd

call appveyor\after_build.cmd

call appveyor\test.cmd

call appveyor\before_deploy.cmd

call appveyor\deploy.cmd

call appveyor\after_deploy.cmd
