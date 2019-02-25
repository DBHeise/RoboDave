#

$apiKey = Read-Host -Prompt "Enter PSGallery API Key"

$modFolder = '.\bin\RoboDave'
if (!(Test-Path $modFolder)) { New-Item -Path $modFolder -ItemType Directory -Force | Out-Null}
$modFolder = Resolve-Path $modFolder

Copy-Item -Path .\RoboDave\bin\Release\RoboDave.dll -Destination (Join-Path $modFolder 'RoboDave.dll')
Copy-Item -Path .\RoboDave.psd1 -Destination (Join-Path $modFolder 'RoboDave.psd1')


Publish-Module -Path $modFolder -NuGetApiKey $apiKey -Verbose