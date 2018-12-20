""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

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
if ($env:UPLOAD_COVERALLS_API -eq "NO" `
  -And $env:UPLOAD_COVERALLS_APP -eq "NO")
{
  return
}

  $url = "https://coveralls.io/webhook?repo_token="
  $url = $url + $env:COVERALLS_REPO_TOKEN

  $payload = @{
    payload = @{
      build_num = $env:COVERALLS_SERVICE_JOB_ID
      status = 'done'
    }
  }
  $body = $payload | ConvertTo-Json

  # Write-Host $body
  $response = Invoke-RestMethod $url -Method Post -Body $bodY -ContentType 'application/json'
  Write-Host $response
