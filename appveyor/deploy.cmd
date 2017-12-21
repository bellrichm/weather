"C:\Program Files (x86)\IIS\Microsoft Web Deploy V3\msdeploy.exe" ^
  -verb:sync ^
  -useChecksum ^
  -source:package=%APPVEYOR_BUILD_FOLDER%\%ARTIFACT_NAME%.zip ^
  -skip:Directory=\\Data ^
  -skip:Directory=\\logs ^
  -enableRule:AppOffline ^
  -allowUntrusted ^
  -dest:auto,computerName=%AZURE_SERVER%,^
UserName="%AZURE_USER%",^
Password="%AZURE_PW%"^
,AuthType="Basic" 
