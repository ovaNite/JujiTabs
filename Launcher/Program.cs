using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace JujiTABSLauncher
{
    internal static class Program
    {
        private const string PluginName = "JujiTABS.dll";

        private static int Main()
        {
            try
            {
                string baseDir = AppContext.BaseDirectory;
                string pluginDir = Path.Combine(baseDir, "BepInEx", "plugins");
                Directory.CreateDirectory(pluginDir);

                string pluginPath = Path.Combine(pluginDir, PluginName);
                ExtractEmbeddedPlugin(pluginPath);

                string tabsExe = FindTabsExecutable(baseDir);
                if (tabsExe == null)
                {
                    Console.Error.WriteLine("JujiTABS: TABS.exe wurde nicht gefunden.");
                    Console.Error.WriteLine("Lege die JujiTABS.exe in den TABS-Ordner oder starte sie direkt dort.");
                    return 2;
                }

                string tabsDir = Path.GetDirectoryName(tabsExe)!;
                string targetPluginDir = Path.Combine(tabsDir, "BepInEx", "plugins");
                Directory.CreateDirectory(targetPluginDir);
                File.Copy(pluginPath, Path.Combine(targetPluginDir, PluginName), true);

                Console.WriteLine("JujiTABS wird gestartet...");
                var psi = new ProcessStartInfo
                {
                    FileName = tabsExe,
                    WorkingDirectory = tabsDir,
                    UseShellExecute = true
                };

                using Process? process = Process.Start(psi);
                return process == null ? 3 : 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("JujiTABS Launcher Fehler: " + ex.Message);
                return 1;
            }
        }

        private static void ExtractEmbeddedPlugin(string destination)
        {
            using Stream? resource = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("JujiTABS.dll");
            if (resource == null)
                throw new InvalidOperationException("Eingebettetes JujiTABS.dll fehlt.");

            using var file = File.Create(destination);
            resource.CopyTo(file);
        }

        private static string? FindTabsExecutable(string launcherDir)
        {
            string[] candidates =
            {
                Path.Combine(launcherDir, "TotallyAccurateBattleSimulator.exe"),
                Path.Combine(launcherDir, "TABS.exe")
            };

            foreach (string p in candidates)
                if (File.Exists(p)) return p;

            string[] roots =
            {
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
            };

            foreach (string root in roots.Where(Directory.Exists))
            {
                try
                {
                    var files = Directory.EnumerateFiles(root, "TotallyAccurateBattleSimulator.exe", SearchOption.AllDirectories);
                    foreach (string p in files) return p;
                }
                catch { }
            }

            return null;
        }
    }
}
