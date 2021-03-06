﻿
$ErrorActionPreference = 'Stop'; # stop on all errors
$toolsDir   = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$packageName = $env:ChocolateyPackageName
$fileName = "Added.Web.Core"
$filePath = Join-Path $toolsDir "$fileName.dll"

function InstallGAC([string] $filePath)
{
	[System.Reflection.Assembly]::Load("System.EnterpriseServices, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")            
	$publish = New-Object System.EnterpriseServices.Internal.Publish            
	$publish.GacInstall($filePath)
}

function ResetIIS()
{
	$iis = Get-WmiObject -Query "select * from Win32_Service where name = 'W3svc'"

	if ($iis)
	{
		Start-Process "iisreset.exe" -NoNewWindow -Wait
	}
}

function MainChoco() 
{
	try {
		InstallGAC($filePath)
		ResetIIS
	}
	catch {
		throw
	}
}

MainChoco