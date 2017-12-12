
set MSPEC=%HOMEPATH%\.nuget\packages\Machine.Specifications.Runner.Console\0.9.3\tools\mspec-clr4.exe
set OPENCOVER=%HOMEPATH%\.nuget\packages\OpenCover\4.6.519\tools\OpenCover.Console.exe
set COVERALLS=%HOMEPATH%\.nuget\packages\coveralls.io\1.4.2\tools\coveralls.net.exe

set TESTDIR=api\test\
set TESTDLLS=
set TESTDLLS=%TESTDLLS% %TESTDIR%BellRichM.Weather.Api.Test\bin\Debug\net462\BellRichM.Weather.Api.Test.dll
set TESTDLLS=%TESTDLLS% %TESTDIR%BellRichM.Identity.Api.Test\bin\Debug\net462\BellRichM.Identity.Api.Test.dll


%OPENCOVER% -register:user -output:opencover.xml -filter:"+[BellRichM*]* -[*]*.Migrations.* -[*.Test]*" -excludebyattribute:BellRichM.Attribute.CodeCoverage.ExcludeFromCodeCoverageAttribute -target:%MSPEC% -targetargs:"--xml .\mspec.xml %TESTDLLS%"

%COVERALLS% --opencover opencover.xml --full-sources
