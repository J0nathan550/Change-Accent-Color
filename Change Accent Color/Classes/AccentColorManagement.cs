using Microsoft.Win32;
using System.Windows;
using System.Windows.Media;

namespace Change_Accent_Color.Classes;

public class AccentColorManager
{
    private const string DwmRegistryPath = @"Software\Microsoft\Windows\DWM";

    private const string AccentColorValueName = "AccentColor";
    private const string AccentColorInactiveValueName = "AccentColorInactive";
    private const string ColorizationAfterglowValueName = "ColorizationAfterglow";
    private const string ColorizationColorValueName = "ColorizationColor";

    /// <summary>
    /// Reads a color value from the Windows DWM registry.
    /// </summary>
    private static Color GetRegistryColor(string valueName)
    {
        try
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(DwmRegistryPath)!;
            if (key != null)
            {
                object value = key.GetValue(valueName)!;
                if (value is int colorDword)
                {
                    return ConvertAbgrToColor(colorDword);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to read {valueName}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return Colors.Transparent;
    }

    /// <summary>
    /// Sets a color value in the Windows DWM registry.
    /// </summary>
    private static void SetRegistryColor(string valueName, Color color, bool showMessage)
    {
        try
        {
            int colorDword = ConvertColorToAbgr(color);

            using RegistryKey key = Registry.CurrentUser.OpenSubKey(DwmRegistryPath, writable: true)!;
            if (key != null)
            {
                key.SetValue(valueName, colorDword, RegistryValueKind.DWord);
                if (showMessage) MessageBox.Show($"{valueName} updated!\nNOTE: Some apps may require reload to update color.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (showMessage)  MessageBox.Show($"Failed to open the registry key for {valueName}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            if (showMessage) MessageBox.Show($"Failed to set {valueName}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Gets the current AccentColor.
    /// </summary>
    public static Color GetAccentColor() => GetRegistryColor(AccentColorValueName);

    /// <summary>
    /// Sets the AccentColor.
    /// </summary>
    public static void SetAccentColor(Color color, bool showMessage) => SetRegistryColor(AccentColorValueName, color, showMessage);

    /// <summary>
    /// Gets the current AccentColorInactive.
    /// </summary>
    public static Color GetAccentColorInactive() => GetRegistryColor(AccentColorInactiveValueName);

    /// <summary>
    /// Sets the AccentColorInactive.
    /// </summary>
    public static void SetAccentColorInactive(Color color, bool showMessage) => SetRegistryColor(AccentColorInactiveValueName, color, showMessage);

    /// <summary>
    /// Gets the current ColorizationAfterglow.
    /// </summary>
    public static Color GetColorizationAfterglow() => GetRegistryColor(ColorizationAfterglowValueName);

    /// <summary>
    /// Sets the ColorizationAfterglow.
    /// </summary>
    public static void SetColorizationAfterglow(Color color, bool showMessage) => SetRegistryColor(ColorizationAfterglowValueName, color, showMessage);

    /// <summary>
    /// Gets the current ColorizationColor.
    /// </summary>
    public static Color GetColorizationColor() => GetRegistryColor(ColorizationColorValueName);

    /// <summary>
    /// Sets the ColorizationColor.
    /// </summary>
    public static void SetColorizationColor(Color color, bool showMessage) => SetRegistryColor(ColorizationColorValueName, color, showMessage);

    /// <summary>
    /// Converts a ABGR DWORD to a <see cref="Color"/>.
    /// </summary>
    public static Color ConvertAbgrToColor(int abgr)
    {
        byte a = (byte)((abgr >> 24) & 0xFF);
        byte b = (byte)((abgr >> 16) & 0xFF);
        byte g = (byte)((abgr >> 8) & 0xFF);
        byte r = (byte)(abgr & 0xFF);

        return Color.FromArgb(a, r, g, b); // Map to ARGB
    }

    /// <summary>
    /// Converts a <see cref="Color"/> to an ABGR DWORD.
    /// </summary>
    public static int ConvertColorToAbgr(Color color)
    {
        return (color.A << 24) | (color.B << 16) | (color.G << 8) | color.R;
    }
}