""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

# ToDo - don't hardcode coverlet filename
if ($env:COVERAGE_REPORT -eq 'YES')
{  
  $parms = ''
  $parms = $parms + '"-reporttypes:Html;XmlSummary;Xml" '
  $parms = $parms + '"-reports:api/test/coverlet/coverage.netcoreapp3.1.opencover.xml;app/coverage/lcov.info" '
  $parms = $parms + '"-targetdir:coverage/report" '
  $parms = $parms + '"-historydir:coverage/report/history" '
  $parms = $parms + '-verbosity:Warning '
  $cmd = $env:TOOLDIR + "reportgenerator $parms"
  RunCmd $cmd
}

if ($env:UPLOAD_COVERALLS_API -eq "NO" `
  -And $env:UPLOAD_COVERALLS_APP -eq "NO")
{
  return
}

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

if ($env:UPLOAD_COVERALLS_API -ne "NO")
{

  $parms = ''
  $parms = $parms + '--multiple '
  $parms = $parms + '--useRelativePaths '
  $parms = $parms + $coverage_list
  $cmd = $env:TOOLDIR + "csmacnz.Coveralls " + $parms
  RunCmd $cmd  
}

# ToDo - option to run, but not upload?
if ($env:UPLOAD_SONARQUBE_API -ne 'NO')
{
  $parms = '/d:sonar.login=$env:SONARQUBE_REPO_TOKEN'
  $cmd = $env:TOOLDIR + "dotnet-sonarscanner end $parms"
  RunCmd $cmd
}

# todo - upload to sonar if only frontend
<#
$parms = ''

#$parms = $parms + '"-Dsonar.projectKey=RmbTest" '
$parms = $parms + '"-Dsonar.projectKey=weather" '

$parms = $parms + '"-Dsonar.branch.name=buildexperiments" '
$parms = $parms + '"-Dsonar.organization=bellrichm" '
$parms = $parms + '"-Dsonar.sources=app/src" '
$parms = $parms + '"-Dsonar.host.url=https://sonarcloud.io" '
$parms = $parms + '"-Dsonar.typescript.lcov.reportPaths=app/coverage/lcov.info" '
#$parms = $parms + '"-Dsonar.projectVersion=local "'
$parms = $parms + '"-Dsonar.login=?????" '

$parms = $parms + '"-X" '

$cmd = 'c:\RMBData\sonar-scanner-cli-3.2.0.1227-windows\sonar-scanner-3.2.0.1227-windows\bin\sonar-scanner '

$cmd = $cmd + $parms
Write-Host $cmd
Invoke-Expression $cmd
#>
