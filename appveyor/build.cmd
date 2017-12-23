echo "******************************** Build ********************************"
sonarqube.scanner.msbuild.exe begin ^
  /k:"weather" ^
  /o:"bellrichm" ^
  /v:%APPVEYOR_BUILD_VERSION% ^
  /d:sonar.log.level=INFO ^
  /d:sonar.branch.name=%BRANCH_NAME% ^
  /d:sonar.host.url="https://sonarcloud.io" ^
  /d:sonar.login=%SONARQUBE_REPO_TOKEN%  ^
  /d:sonar.exclusions="**/Migrations/*, **/obj/**/*, **/*Specs.*" ^
  /d:sonar.cs.opencover.reportsPaths="opencover.xml"

dotnet build api\src\BellRichM.Weather.sln ^
  --no-restore

dotnet build api\test\BellRichM.Weather.Test.sln ^
  --no-restore ^
  --no-dependencies ^
  -f net462

dotnet build api/integration/BellRichM.Identity.Api.Integration/BellRichM.Identity.Api.Integration.csproj ^
  --no-restore ^
  --no-dependencies
  
  
dotnet build api/smoke/BellRichM.Identity.Api.Smoke/BellRichM.Identity.Api.Smoke.csproj ^
  --no-restore ^
  --no-dependencies
  