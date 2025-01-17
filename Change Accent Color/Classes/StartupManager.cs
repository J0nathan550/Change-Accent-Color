using Microsoft.Win32;
using System.Diagnostics;

namespace Change_Accent_Color.Classes;

public static class StartupManager
{
    private const string AppName = "Change Accent Color";
    private static readonly string AppPath = Environment.ProcessPath!;

    /// <summary>
    /// Checks if the app is set to start with Windows.
    /// </summary>
    /// <returns>True if startup is enabled, false otherwise.</returns>
    public static bool IsStartupEnabled
    {
        get
        {
            try
            {
                using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", false)!;
                return key != null && key.GetValue(AppName) != null;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return false;
            }
        }
    }

    /// <summary>
    /// Enables the app to start with Windows.
    /// </summary>
    public static void EnableStartup()
    {
        try
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true)!;
            key?.SetValue(AppName, $"\"{AppPath}\"");
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
        }
    }

    /// <summary>
    /// Disables the app from starting with Windows.
    /// </summary>
    public static void DisableStartup()
    {
        try
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true)!;
            if (key != null && key.GetValue(AppName) != null)
            {
                key.DeleteValue(AppName);
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
        }
    }

}