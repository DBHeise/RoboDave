#

$AssemblyInfoFile = Get-ChildItem .\RoboDave\Properties\AssemblyInfo.cs
$AssemblyInfo = Get-Content $AssemblyInfoFile -Raw

$regex= [regex]'AssemblyVersion\(\"(?<version>[^\)]+)\"\)'

$oldVersion = [version]$regex.Matches($AssemblyInfo).Groups[1].value
$oldVersionStr = $oldVersion.ToString()

$newVersion = $oldVersion.Major.ToString() + "." + ($oldVersion.Minor).ToString() + "." + ($oldVersion.Build+1).ToString() + "." + [DateTime]::Now.ToString("ddHH")

$AssemblyInfo.Replace($oldVersionStr, $newVersion) | Set-Content -Path $AssemblyInfoFile

(Get-Content .\RoboDave.psd1 -Raw).Replace($oldVersionStr, $newVersion) | Set-Content -Path .\RoboDave.psd1
