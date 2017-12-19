echo "******************************** Build ********************************"
dotnet build api\test\BellRichM.Weather.Test.sln -f net462

sonarqube.scanner.msbuild.exe begin ^
  /k:"weather" ^
  /o:"bellrichm" ^
  /v:%APPVEYOR_BUILD_VERSION% ^
  /d:sonar.branch.name=%BRANCH_NAME% ^
  /d:sonar.host.url="https://sonarcloud.io" ^
  /d:sonar.login=%SONARQUBE_REPO_TOKEN%  ^
  /d:sonar.exclusions="**/Migrations/*, **/obj/**/*"  ^
  /d:sonar.cs.opencover.reportsPaths="opencover.xml"

dotnet publish api\src\BellRichM.Weather.sln ^
  --output %appveyor_build_folder%\dist ^
  -f netcoreapp2.0 ^
  -r win10-x64
