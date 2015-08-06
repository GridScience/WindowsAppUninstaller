using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Windows.Appx.PackageManager.Commands;

namespace WindowsAppUninstaller
{
    public static class AppxManager
    {

        public static AppxPackage[] GetPackages(AppxParameters parameters)
        {
            using (var runspace = RunspaceFactory.CreateRunspace())
            {
                AppxPackage[] packages = null;

                runspace.Open();

                var pipeline = runspace.CreatePipeline();
                // 无法使用 AppxPackageManager 的功能，只好借用 PowerShell
                // 想使用 AppxPackageManager，请建立 Windows App 工程，而非 WPF 工程
                string command = "Get-AppxPackage";
                command = ConcatCommand(command, parameters);
                pipeline.Commands.AddScript(command);

                var result = pipeline.Invoke();

                if (result.Count > 0)
                {
                    packages = (from psObject in result
                                select (AppxPackage)psObject.BaseObject
                        ).ToArray();
                }

                runspace.Close();

                return packages;
            }
        }

        public static void RemovePackage(string packageFullName, AppxParameters parameters)
        {
            using (var runspace = RunspaceFactory.CreateRunspace())
            {
                AppxPackage[] packages = null;

                runspace.Open();

                var pipeline = runspace.CreatePipeline();
                // 无法使用 AppxPackageManager 的功能，只好借用 PowerShell
                // 想使用 AppxPackageManager，请建立 Windows App 工程，而非 WPF 工程
                string command = "Remove-AppxPackage";
                command += " -Package " + packageFullName;
                command = ConcatCommand(command, parameters);
                pipeline.Commands.AddScript(command);

                var result = pipeline.Invoke();

                if (result.Count > 0)
                {
                    packages = (from psObject in result
                                select (AppxPackage)psObject.BaseObject
                        ).ToArray();
                }

                runspace.Close();
            }
        }

        private static string ConcatCommand(string originalCommand, AppxParameters parameters)
        {
            var c = originalCommand;
            // Get
            if ((parameters.Options & AppxOptions.AllUsers) != 0)
            {
                c += " -AllUsers";
            }
            if ((parameters.Options & AppxOptions.PackageNameFilter) != 0)
            {
                c += " -Name " + parameters.PackageName;
            }
            if ((parameters.Options & AppxOptions.PublisherFilter) != 0)
            {
                c += " -Publisher " + parameters.PublisherName;
            }
            if ((parameters.Options & AppxOptions.AllUsers) == 0 && (parameters.Options & AppxOptions.UserFilter) != 0)
            {
                c += " -User " + parameters.UserName;
            }
            // Remove
            if ((parameters.Options & AppxOptions.Confirm) != 0)
            {
                c += " -confirm";
            }
            if ((parameters.Options & AppxOptions.DisplayWhatIf) != 0)
            {
                c += " -WhatIf";
            }
            return c;
        }

    }
}
