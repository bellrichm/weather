echo "******************************** After Deploy ********************************"

dotnet test ^
  --no-restore ^
  api/smoke/BellRichM.Identity.Api.Smoke/BellRichM.Identity.Api.Smoke.csproj
