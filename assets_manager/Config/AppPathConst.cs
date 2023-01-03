using System;
using System.Runtime.InteropServices;

namespace assets_manager.Config
{
    public static class AppPathConst
    {
        public static string? AppTempPath { get; set; }
        public static string? BaseDirectory { get; set; }
        public static string? AppDataDirectory { get; set; }

        public static void AppInitValue() {

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                AppTempPath = Environment.GetEnvironmentVariable("TEMP")?.Replace("\\", "/");
                AppDataDirectory = Environment.GetEnvironmentVariable("LocalAppData")?.Replace("\\", "/");
                if (AppDataDirectory != null) {
                    AppDataDirectory = AppDataDirectory + "/assets_manager";
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                AppTempPath = Environment.GetEnvironmentVariable("HOME") + "/.cache";
                AppDataDirectory = Environment.GetEnvironmentVariable("HOME") + "/.local/share/assets_manager";

            }
            else if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {

                AppTempPath = Environment.GetEnvironmentVariable("HOME") + "/Library/Caches";
                AppDataDirectory = Environment.GetEnvironmentVariable("HOME") + "/Library/assets_manager";
            }

            // app directory
            BaseDirectory = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/");

        }
    }
}
