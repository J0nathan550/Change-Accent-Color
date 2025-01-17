using Microsoft.Win32;
using System.IO;
using System.Windows;

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
                MessageBox.Show($"An error occurred while checking startup status: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
            if (key != null)
            {
                key.SetValue(AppName, $"\"{AppPath}\"");
                MessageBox.Show("Startup enabled successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred while enabling startup: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
                MessageBox.Show("Startup disabled successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("The app is not set to start with Windows.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred while disabling startup: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }

}