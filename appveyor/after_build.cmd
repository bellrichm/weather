echo "******************************** After Build ********************************"

set TESTDIR=api\test\
set TESTDLLS=
set TESTDLLS=%TESTDLLS% %TESTDIR%BellRichM.Weather.Api.Test\bin\Debug\net462\BellRichM.Weather.Api.Test.dll
set TESTDLLS=%TESTDLLS% %TESTDIR%BellRichM.Identity.Api.Test\bin\Debug\net462\BellRichM.Identity.Api.Test.dll

%OPENCOVER% -register:user -output:opencover.xml -filter:"+[BellRichM*]* -[*]*.Migrations.* -[*.Test]*" -excludebyattribute:BellRichM.Attribute.CodeCoverage.ExcludeFromCodeCoverageAttribute -target:%MSPEC% -targetargs:"--xml .\mspec.xml %TESTDLLS%"

%COVERALLS% --opencover opencover.xml --full-sources

sonarqube.scanner.msbuild.exe end ^
  /d:sonar.login=%SONARQUBE_REPO_TOKEN%