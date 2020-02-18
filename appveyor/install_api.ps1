""
"******************************** " + $MyInvocation.InvocationName + " ********************************"

choco upgrade "msbuild-sonarqube-runner" -y

Install-Product node 10