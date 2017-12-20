echo "******************************** Test ********************************"
call api\test\coverage.cmd

sonarqube.scanner.msbuild.exe end ^
  /d:sonar.login=%SONARQUBE_REPO_TOKEN%

dotnet test api/integration/BellRichM.Identity.Api.Integration/BellRichM.Identity.Api.Integration.csproj
