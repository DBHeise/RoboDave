[CmdletBinding()]
param([String]$TargetPath)

$exe = Join-Path $env:LIBDIR 'XmlDoc2CmdletDoc.0.2.13\tools\XmlDoc2CmdletDoc.exe'

if (Test-Path $exe) {	
	. $exe $TargetPath
}
