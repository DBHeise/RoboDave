# RoboDave - a powershell Module for misc stuff
[![PowerShell Gallery - RoboDave](https://img.shields.io/badge/Powershell_Gallery-RoboDave-0072c6.svg)](https://www.powershellgallery.com/packages/robodave)
[![Build Status](https://h2net.visualstudio.com/RoboDave/_apis/build/status/DBHeise.RoboDave?branchName=master)](https://h2net.visualstudio.com/RoboDave/_build/latest?definitionId=1&branchName=master)

Current Version: 1.4.17.2712

## Available cmdlets:
### Networking
* [Get-LocalAddresses](https://github.com/DBHeise/RoboDave/wiki/Get-LocalAddresses) - returns the IPAddress object for all local network interfaces
* [Get-LocalhostFQDN](https://github.com/DBHeise/RoboDave/wiki/Get-LocalhostFQDN) - returns the fully qualified domain name (FQDN) of the local host
* [Get-RemoteAddresses](https://github.com/DBHeise/RoboDave/wiki/Get-RemoteAddresses) - returns the IPAddress returned from a remote service
### Forensics
* [Get-BrowserHistory](https://github.com/DBHeise/RoboDave/wiki/Get-BrowserHistory) - returns a list of URLs/Domains found in each browser's history (IE, Chrome, Firefox)
* [Get-FileHashBulk](https://github.com/DBHeise/RoboDave/wiki/Get-FileHashBulk) - computes multiple hashes for a given file (or files)
### Random
* [New-RandomCoordinate](https://github.com/DBHeise/RoboDave/wiki/New-RandomCoordinate) - generates a random lat&lon 
* [New-RandomDateTime](https://github.com/DBHeise/RoboDave/wiki/New-RandomDateTime) - generates a random DateTime (relative to now)
* [New-RandomMEID](https://github.com/DBHeise/RoboDave/wiki/New-RandomMEID) - generates a random MEID
* [New-RandomString](https://github.com/DBHeise/RoboDave/wiki/New-RandomString) - generates a random string (many options)
* [New-RandomExcuse](https://github.com/DBHeise/RoboDave/wiki/New-RandomExcuse) - generates a simple excuse letter
* [New-RandomFile](https://github.com/DBHeise/RoboDave/wiki/New-RandomFile) - generates a random filled file
#### MadLib
* [Use-MadLib](https://github.com/DBHeise/RoboDave/wiki/Use-MadLib) - fills out a MadLib string
* [New-MadLib](https://github.com/DBHeise/RoboDave/wiki/New-MadLib) - creates a MadLib string from text
#### Imaging
* [New-RandomBitmap](https://github.com/DBHeise/RoboDave/wiki/New-RandomBitmap) - generates a random bitmap
* [New-RandomImage](https://github.com/DBHeise/RoboDave/wiki/New-RandomImage) - generates a random image based on pattens (random, pixels, shapes)
* [Get-BitmapFromFile](https://github.com/DBHeise/RoboDave/wiki/Get-BitmapFromFile) - generates a bitmap from the bytes of the given file
* [Get-VisualHash](https://github.com/DBHeise/RoboDave/wiki/Get-VisualHash) - generates an image from an input
