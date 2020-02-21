""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

# ToDo - don't hardcode coverlet filename
if ($env:COVERAGE_REPORT_API -eq 'YES' `
  -Or $env:COVERAGE_REPORT_APP -eq 'YES')
{  
  $coverage_files = @()
  if ($env:COVERAGE_REPORT_API -ne "NO")
  {
    $coverage_files += 'api/test/coverlet/coverage.netcoreapp3.1.opencover.xml'
  }
  
  if ($env:COVERAGE_REPORT_APP -ne "NO")
  {
    $coverage_files += 'app/coverage/lcov.info'
  }

  $coverage_list = ''
  for ($i = 0; $i -le ($coverage_files.length - 1); $i += 1) {
    $coverage_list += $coverage_files[$i] + ';'
  }
  
  if ($coverage_list -ge 0) {
    $coverage_list = '"-reports:' + $coverage_list.Substring(0,$coverage_list.Length-1) + '" '
  }

  $parms = ''
  $parms = $parms + '"-reporttypes:Html;XmlSummary;Xml" '
  $parms = $parms + $coverage_list
  $parms = $parms + '"-targetdir:coverage/report" '
  $parms = $parms + '"-historydir:coverage/report/history" '
  $parms = $parms + '-verbosity:Warning '
  $cmd = $env:TOOLDIR + "reportgenerator $parms"
  RunCmd $cmd
}

if ($env:UPLOAD_COVERALLS_API -ne "NO" `
  -Or $env:UPLOAD_COVERALLS_APP -ne "NO")
{
  $coverage_files = @()
  if ($env:UPLOAD_COVERALLS_API -ne "NO")
  {
    $coverage_files += 'opencover=api/test/coverlet/coverage.netcoreapp3.1.opencover.xml'
  }

  if ($env:UPLOAD_COVERALLS_APP -ne "NO")
  {
    $coverage_files += 'lcov=app/coverage/lcov.info'
  }  

  $coverage_list = ''
  for ($i = 0; $i -le ($coverage_files.length - 1); $i += 1) {
    $coverage_list += $coverage_files[$i] + ';'
  }

  if ($coverage_list -ge 0) {
    $coverage_list = '-i "' + $coverage_list.Substring(0,$coverage_list.Length-1) + '" '
  }

  $parms = ''
  $parms = $parms + '--multiple '
  $parms = $parms + '--useRelativePaths '
  $parms = $parms + $coverage_list
  $cmd = $env:TOOLDIR + "csmacnz.Coveralls " + $parms
  RunCmd $cmd  
}

if ($env:UPLOAD_SONARQUBE_API -eq 'NO' `
-And $env:UPLOAD_SONARQUBE_APP -ne 'NO')
{
  $parms = ''
  $parms = $parms + 'begin '
  $parms = $parms + '/k:"weather" '
  $parms = $parms + '/o:"bellrichm" '
  $parms = $parms + '/v:$env:APPVEYOR_BUILD_VERSION '
  $parms = $parms + '/d:sonar.branch.name=$env:BRANCH_NAME '
  $parms = $parms + '/d:sonar.host.url="https://sonarcloud.io" '
  $parms = $parms + '/d:sonar.login=$env:SONARQUBE_REPO_TOKEN '
  $parms = $parms + '/d:sonar.exclusions="**/Migrations/*, **/obj/**/*, **/*.conf.*, **/e2e/**/*, **/coverage/**/*, **/*spec*" '
  $parms = $parms + '/d:sonar.cpd.exclusions="**/Models/*" '
  $parms = $parms + '/d:sonar.test.exclusions="**/obj/**/*" '
  $parms = $parms + '/d:sonar.typescript.lcov.reportPaths="../app/coverage/lcov.info" '
  $parms = $parms + '/d:sonar.log.level=WARN '
  # $parms = $parms + '/d:sonar.verbose=true '

  $cmd =  $env:TOOLDIR + "dotnet-sonarscanner $parms"
  RunCmd $cmd
}

if ($env:UPLOAD_SONARQUBE_APP -ne 'NO') 
{
  $cmd = "dotnet build app\BellRichM.App.csproj --no-restore"
  RunCmd $cmd
}  

# ToDo - option to run, but not upload?
if ($env:UPLOAD_SONARQUBE_API -ne 'NO' `
  -Or $env:UPLOAD_SONARQUBE_APP -ne 'NO')
{
  $parms = '/d:sonar.login=$env:SONARQUBE_REPO_TOKEN '
  # $parms = $parms + '/d:sonar.log.level=WARN ' # - not valid on end phase
  $cmd = $env:TOOLDIR + "dotnet-sonarscanner end $parms"
  RunCmd $cmd
}

