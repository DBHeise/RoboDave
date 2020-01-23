# RoboDave - a powershell Module for misc stuff
[![PowerShell Gallery - RoboDave](https://img.shields.io/badge/Powershell_Gallery-RoboDave-0072c6.svg)](https://www.powershellgallery.com/packages/robodave)
[![Build Status](https://h2net.visualstudio.com/RoboDave/_apis/build/status/DBHeise.RoboDave?branchName=master)](https://h2net.visualstudio.com/RoboDave/_build/latest?definitionId=1&branchName=master)



## Available cmdlets:
### Networking
* Get-LocalAddresses - returns the IPAddress object for all local network interfaces
* Get-LocalhostFQDN - returns the fully qualified domain name (FQDN) of the local host
* Get-RemoteAddresses - returns the IPAddress returned from a remote service
### Forensics
* Get-BrowserHistory - returns a list of URLs/Domains found in each browser's history (IE, Chrome, Firefox)
* Get-FileHashBulk - computes multiple hashes for a given file (or files)
### Random
* New-RandomCoordinate - generates a random lat&lon 
* New-RandomDateTime - generates a random DateTime (relative to now)
* New-RandomMEID - generates a random MEID
* New-RandomString - generates a random string (many options)
* New-RandomExcuse - generates a simple excuse letter
* New-RandomFile - generates a random filled file
#### MadLib
* Use-MadLib - fills out a MadLib string
* New-MadLib - creates a MadLib string from text
#### Imaging
* New-RandomBitmap - generates a random bitmap
* New-RandomImage - generates a random image based on pattens (random, pixels, shapes)
* Get-BitmapFromFile - generates a bitmap from the bytes of the given file
