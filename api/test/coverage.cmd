echo off

set MSPEC=%HOMEPATH%\.nuget\packages\Machine.Specifications.Runner.Console\0.9.3\tools\mspec-clr4.exe
set OPENCOVER=%HOMEPATH%\.nuget\packages\OpenCover\4.6.519\tools\OpenCover.Console.exe
set REPORTGENERATOR=%HOMEPATH%\.nuget\packages\ReportGenerator\3.0.2\tools\ReportGenerator.exe

dotnet build -f net462

rem this runs mspec standalone, not needed because it is run in OpenCover
rem %MSPEC% --html coverage\mspec --xml coverage\mspec\mspec.xml BellRichM.Weather.Api.Test\bin\Debug\net462\BellRichM.Weather.Api.Test.dll BellRichM.Identity.Api.Test\bin\Debug\net462\BellRichM.Identity.Api.Test.dll

%OPENCOVER% -register:user -output:coverage\opencover\opencover.xml -filter:"+[BellRichM*]* -[*]*.Migrations.* -[*.Test]*" -excludebyattribute:BellRichM.Attribute.CodeCoverage.ExcludeFromCodeCoverageAttribute -target:%MSPEC% -targetargs:"--html coverage\mspec --xml coverage\mspec\mspec.xml BellRichM.Weather.Api.Test\bin\Debug\net462\BellRichM.Weather.Api.Test.dll BellRichM.Identity.Api.Test\bin\Debug\net462\BellRichM.Identity.Api.Test.dll"

%REPORTGENERATOR% -reporttypes:Html;XmlSummary;Xml -reports:coverage\opencover\opencover.xml -targetdir:coverage\report -historydir:coverage\report\history