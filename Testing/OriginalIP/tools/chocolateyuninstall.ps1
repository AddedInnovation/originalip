
$ErrorActionPreference = 'Stop'; # stop on all errors
$toolsDir   = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$packageName = $env:ChocolateyPackageName
$fileName = "AI.Web.Core"
$filePath = Join-Path $toolsDir "$fileName.dll"

function UnInstallGAC([string] $filePath)
{
	Write-Verbose "Uninstalling the assembly."

	[System.Reflection.Assembly]::Load("System.EnterpriseServices, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")            
	$publish = New-Object System.EnterpriseServices.Internal.Publish            
	$publish.GacRemove($filePath)

	iisreset
}

function MainChoco() 
{
	try {
		UnInstallGAC($filePath)		
	}
	catch {
		throw
	}
}

MainChoco