echo "******************************** Before Deploy ********************************"

rem TODO: add --no-build when it is supported again
dotnet publish api\src\BellRichM.Weather.Web\BellRichM.Weather.Web.csproj ^
  --output %APPVEYOR_BUILD_FOLDER%\dist ^
  --no-restore ^
  -f netcoreapp2.0 

7z a %APPVEYOR_BUILD_FOLDER%\%ARTIFACT_NAME%.zip  %APPVEYOR_BUILD_FOLDER%\dist\*