echo "******************************** Before Build ********************************"

dotnet restore api\src\BellRichM.Weather.sln 
dotnet restore api\test\BellRichM.Weather.Test.sln ^
  --no-dependencies
dotnet restore api/integration/BellRichM.Identity.Api.Integration/BellRichM.Identity.Api.Integration.csproj ^
  --no-dependencies
dotnet restore api/smoke/BellRichM.Identity.Api.Smoke/BellRichM.Identity.Api.Smoke.csproj ^
  --no-dependencies