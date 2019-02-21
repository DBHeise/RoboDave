# RoboDave - a powershell Module for misc stuff
[![Build Status](https://h2net.visualstudio.com/RoboDave/_apis/build/status/DBHeise.RoboDave?branchName=master)](https://h2net.visualstudio.com/RoboDave/_build/latest?definitionId=1&branchName=master)



## Available cmdlets:
### Networking
* Get-LocalAddresses - returns the IPAddress object for all local network interfaces
* Get-LocalhostFQDN - returns the fully qualified domain name (FQDN) of the local host
* Get-RemoteAddresses - returns the IPAddress returned from a remote service
### Forensics
* Get-BrowserHistory - returns a list of URLs/Domains found in each browser's history (IE, Chrome, Firefox)
### Random
* Get-RandomCoordinate - generates a random lat&lon 
* Get-RandomDateTime - generates a random DateTime (relative to now)
* Get-RandomMEID - generates a random MEID
* Get-RandomString - generates a random string (many options)
#### MadLib
* Use-MadLib - fills out a MadLib string
* New-MadLib - creates a MadLib string from text
#### Imaging
* New-RandomBitmap - generates a random bitmap
* Get-BitmapFromFile - generates a bitmap from the bytes of the given file