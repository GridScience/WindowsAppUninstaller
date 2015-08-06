# Windows App Uninstaller

## Overview

Windows App Uninstaller is a WPF utility that helps uninstall Windows Apps.

The principle is simple. It uses `Get-AppxPackage` and `Remove-AppxPackage`, internal cmdlets from PowerShell.

Technically, this program only runs on Windows 10 since it refers to `Microsoft.Windows.Appx.PackageManager.Commands` and `System.Management.Automation` namespaces, version 10.0. The first one provides `AppxPackage` class, and the latter is used to interop with PowerShell.

## Extension

The `System.Management.Deployment` namespace offers package management (from Windows 8 to Windows 10). However, it can only be referred in Windows App projects. Someone who favors pure calls may use that instead of PowerShell interop.

## Usage

Just load the solution with Visual Studio 2015 (VS 2013 is not tested) and launch it. Note that it requires Adminstrator privileges because of the package managements.

## License

This program is provided as-is. Also, it is in public domain.
