echo "******************************** Test ********************************"

dotnet test ^
  --no-restore ^
  --no-build ^
  api/integration/BellRichM.Identity.Api.Integration/BellRichM.Identity.Api.Integration.csproj
