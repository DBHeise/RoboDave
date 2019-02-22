#

$apiKey = Read-Host -Prompt "Enter PSGallery API Key"
$modFolder =Join-Path (Split-Path $profile) "Modules"
$rbdModFolder = Join-Path $modFolder "RoboDave"

Copy-Item -Path .\RoboDave\bin\Release\RoboDave.dll -Destination (Join-Path -Path $rbdModFolder -ChildPath "RoboDave.dll")
Copy-Item -Path .\RoboDave.psd1 -Destination (Join-Path -Path $rbdModFolder -ChildPath "RoboDave.psd1")


Publish-Module -name RoboDave -NuGetApiKey $apiKey -Verbose