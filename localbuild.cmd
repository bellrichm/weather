rem TODO: check return codes of build steps

SET ARTIFACT_NAME=weather
set BRANCH_NAME=master
set DEPLOY=YES

set APPVEYOR_BUILD_VERSION=local
set APPVEYOR_BUILD_FOLDER=C:\RMBData\VSCode\weather\master

set buildpath=C:\Program Files\7-Zip;C:\RMBData\sonar-scanner-msbuild;C:\Program Files (x86)\IIS\Microsoft Web Deploy V3;
set path=%path%;%buildpath%

call localtools\init.cmd

call appveyor\init.cmd

call appveyor\build.cmd

call appveyor\after_build.cmd

call appveyor\test.cmd

call appveyor\before_deploy.cmd

call appveyor\deploy.cmd

call appveyor\after_deploy.cmd
