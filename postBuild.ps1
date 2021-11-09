[CmdletBinding()]
param([String]$TargetPath)

#Add this to the post-build sln event:
#	powershell.exe -NoProfile -NonInteractive -File $(SolutionDir)postBuild.ps1 -TargetPath $(TargetPath)

if (Test-Path env:libdir) {
	$exe = (Get-ChildItem (Join-Path $env:LIBDIR 'XmlDoc2CmdletDoc*\tools\XmlDoc2CmdletDoc.exe') | Sort-Object LastWriteTime | Select-Object -First 1).FullName

	if (Test-Path $exe) {	
		. $exe $TargetPath
	}
}
