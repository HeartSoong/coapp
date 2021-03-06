﻿//-----------------------------------------------------------------------
// <copyright company="CoApp Project">
//     Copyright (c) 2011 Garrett Serack . All rights reserved.
// </copyright>
// <license>
//     The software is licensed under the Apache 2.0 License (the "License")
//     You may not use the software except in compliance with the License. 
// </license>
//-----------------------------------------------------------------------

namespace CoApp.Toolkit.Win32 {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Extensions;

    public static class EnvironmentUtility {
        private const Int32 HWND_BROADCAST = 0xffff;
        private const Int32 WM_SETTINGCHANGE = 0x001A;
        private const Int32 SMTO_ABORTIFHUNG = 0x0002;

        public static void BroadcastChange() {
#if COAPP_ENGINE_CORE
            Rehash.ForceProcessToReloadEnvironment("explorer", "services");
#endif
            Task.Factory.StartNew(() => {
                User32.SendMessageTimeout(HWND_BROADCAST, WM_SETTINGCHANGE, 0, "Environment", SMTO_ABORTIFHUNG, 1000, IntPtr.Zero);
            });

        }

        public static string GetSystemEnvironmentVariable(string name) {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);
        }

        public static void SetSystemEnvironmentVariable(string name, string value) {
            Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.Machine);
        }

        public static string GetUserEnvironmentVariable(string name) {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User);
        }

        public static void SetUserEnvironmentVariable(string name, string value) {
            Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.User);
        }

        public static IEnumerable<string> PowershellModulePath {
            get {
                var path = GetSystemEnvironmentVariable("PSModulePath");
                return string.IsNullOrEmpty(path) ? Enumerable.Empty<string>() : path.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
            set {
                var newValue = value.Any() ? value.Aggregate((current, each) => current + ";" + each) : null;
                if (newValue != GetSystemEnvironmentVariable("PSModulePath")) {
                    SetSystemEnvironmentVariable("PSModulePath", newValue);
                }
            }
        }

        public static IEnumerable<string> SystemPath {
            get {
                var path = GetSystemEnvironmentVariable("PATH");
                return string.IsNullOrEmpty(path) ? Enumerable.Empty<string>() : path.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
            set {
                var newValue = value.Any() ? value.Aggregate((current, each) => current + ";" + each) : null;
                if (newValue != GetSystemEnvironmentVariable("PATH")) {
                    SetSystemEnvironmentVariable("PATH", newValue);
                }
            }
        }

        public static IEnumerable<string> UserPath {
            get {
                var path = GetUserEnvironmentVariable("PSModulePath");
                return string.IsNullOrEmpty(path) ? Enumerable.Empty<string>() : path.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
            set {
                var newValue = value.Any() ? value.Aggregate((current, each) => current + ";" + each) : null;
                if (newValue != GetUserEnvironmentVariable("PSModulePath")) {
                    SetUserEnvironmentVariable("PSModulePath", newValue);
                }
            }
        }

        public static IEnumerable<string> EnvironmentPath {
            get {
                var path = Environment.GetEnvironmentVariable("path");
                return string.IsNullOrEmpty(path) ? Enumerable.Empty<string>() : path.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
            set {
                Environment.SetEnvironmentVariable("path", value.Any() ? value.Aggregate((current, each) => current + ";" + each) : "");
            }
        }

        public static IEnumerable<string> Append(this IEnumerable<string> searchPath, string pathToAdd) {
            if (searchPath.Any(s => s.Equals(pathToAdd, StringComparison.CurrentCultureIgnoreCase))) {
                return searchPath;
            }
            return searchPath.UnionSingleItem(pathToAdd);
        }

        public static IEnumerable<string> Prepend(this IEnumerable<string> searchPath, string pathToAdd) {
            if (searchPath.Any(s => s.Equals(pathToAdd, StringComparison.CurrentCultureIgnoreCase))) {
                return searchPath;
            }
            return pathToAdd.SingleItemAsEnumerable().Union(searchPath);
        }

        public static IEnumerable<string> Remove(this IEnumerable<string> searchPath, string pathToRemove) {
            return searchPath.Where(s => !s.Equals(pathToRemove, StringComparison.CurrentCultureIgnoreCase));
        }

        

        internal static string FindInPath(string filename, string searchPath = null) {
            if (string.IsNullOrEmpty(filename))
                return string.Empty;
            var p = new IntPtr();
            var s = new StringBuilder(260); // MAX_PATH

            if (Path.GetExtension(filename) != string.Empty) {
                Kernel32.SearchPath(searchPath, filename, null, s.Capacity, s, out p);
            }

            // Step 2b: ... otherwise, iterate through some defaults.
            else {
                foreach (var ext in new[] { "", ".exe", ".com" }) {
                    if (Kernel32.SearchPath(searchPath, filename + ext, null, s.Capacity, s, out p) != 0)
                        break;
                }
            }

            // Step 3: Return the result.
            return (s.Length == 0 ? filename : s.ToString());
        }
    }
}