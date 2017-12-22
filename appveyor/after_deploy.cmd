echo "******************************** After Deploy ********************************"

dotnet test ^
  --no-restore ^
  --no-build ^
  api/smoke/BellRichM.Identity.Api.Smoke/BellRichM.Identity.Api.Smoke.csproj