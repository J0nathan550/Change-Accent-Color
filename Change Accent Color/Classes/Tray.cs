using ColorPickerWPF;
using ColorPickerWPF.Code;
using Hardcodet.Wpf.TaskbarNotification;
using System.Diagnostics;
using System.Reflection;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Timer = System.Timers.Timer;

namespace Change_Accent_Color.Classes;

public class Tray
{
    private static Color _accentColor;
    private static Color _accentColorInactive;
    private static Color _colorizationAfterglow;
    private static Color _colorizationColor;
    private readonly static Timer forceColorTimer = new() { Interval = 1000 };

    public static void Initialize()
    {
        ContextMenu contextMenu = new();

        MenuItem title = new()
        {
            Header = $"by J0nathan550 (version {Assembly.GetEntryAssembly()!.GetName().Version})",
            IsEnabled = false
        };
        MenuItem changeAccentColorMenuItem = new()
        {
            Header = "Change Accent Color"
        };
        changeAccentColorMenuItem.Click += ChangeAccentColorMenuItem_Click;

        MenuItem changeInactiveAccentColorMenuItem = new()
        {
            Header = "Change Inactive Accent Color"
        };
        changeInactiveAccentColorMenuItem.Click += ChangeInactiveAccentColorMenuItem_Click;

        MenuItem changeColorizationAfterGlowMenuItem = new()
        {
            Header = "Change Colorization After Glow Color"
        };
        changeColorizationAfterGlowMenuItem.Click += ChangeColorizationAfterGlowMenuItem_Click;

        MenuItem changeColorizationColorMenuItem = new()
        {
            Header = "Change Colorization Color"
        };
        changeColorizationColorMenuItem.Click += ChangeColorizationColorMenuItem_Click;

        MenuItem runOnStartupItem = new()
        {
            Header = "Run on Startup of Windows",
            IsCheckable = true,
            IsChecked = StartupManager.IsStartupEnabled
        };
        runOnStartupItem.Click += RunOnStartupItem_Click;

        MenuItem githubMenuItem = new()
        {
            Header = "Check GitHub"
        };
        githubMenuItem.Click += GithubMenuItem_Click;

        MenuItem exitKeepApp = new()
        {
            Header = "Exit and keep changes"
        };
        exitKeepApp.Click += ExitKeepApp_Click;
        MenuItem exitRemoveApp = new()
        {
            Header = "Exit and remove changes"
        };
        exitRemoveApp.Click += ExitRemoveApp_Click;

        contextMenu.Items.Add(title);
        contextMenu.Items.Add(new Separator());
        contextMenu.Items.Add(changeAccentColorMenuItem);
        contextMenu.Items.Add(changeInactiveAccentColorMenuItem);
        contextMenu.Items.Add(changeColorizationAfterGlowMenuItem);
        contextMenu.Items.Add(changeColorizationColorMenuItem);
        contextMenu.Items.Add(new Separator());
        contextMenu.Items.Add(runOnStartupItem);
        contextMenu.Items.Add(githubMenuItem);
        contextMenu.Items.Add(new Separator());
        contextMenu.Items.Add(exitKeepApp);
        contextMenu.Items.Add(exitRemoveApp);

        TaskbarIcon myTrayIcon = new()
        {
            ToolTipText = "Change Accent Color",
            IconSource = new BitmapImage(new Uri("/Assets/iconTray.ico", UriKind.RelativeOrAbsolute)),
            ContextMenu = contextMenu
        };
        myTrayIcon.ShowBalloonTip("Change Accent Color", "Press right mouse button on the icon in tray to work...", BalloonIcon.Info);

        SaveSystem.Load();
        
        _accentColor = AccentColorManager.GetAccentColor();
        _accentColorInactive = AccentColorManager.GetAccentColorInactive();
        _colorizationAfterglow = AccentColorManager.GetColorizationAfterglow();
        _colorizationColor = AccentColorManager.GetColorizationColor();

        forceColorTimer.Elapsed += ForceColorTimer_Elapsed;
        forceColorTimer.Start();
    }

    private static void ForceColorTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        AccentColorManager.SetAccentColor(AccentColorManager.ConvertAbgrToColor(SaveSystem.UserData.AccentColor), false);
        AccentColorManager.SetAccentColorInactive(AccentColorManager.ConvertAbgrToColor(SaveSystem.UserData.AccentColorInactive), false);
        AccentColorManager.SetColorizationAfterglow(AccentColorManager.ConvertAbgrToColor(SaveSystem.UserData.ColorizationAfterglow), false);
        AccentColorManager.SetColorizationColor(AccentColorManager.ConvertAbgrToColor(SaveSystem.UserData.ColorizationColor), false);
    }

    private static void ChangeAccentColorMenuItem_Click(object sender, RoutedEventArgs e)
    {
        forceColorTimer.Stop();
        bool ok = ColorPickerWindow.ShowDialog(out Color color, ColorPickerDialogOptions.SimpleView);
        if (ok)
        {
            SaveSystem.UserData.AccentColor = AccentColorManager.ConvertColorToAbgr(color);
            AccentColorManager.SetAccentColor(color, true);
            SaveSystem.Save();
        }
        forceColorTimer.Start();
    }

    private static void ChangeInactiveAccentColorMenuItem_Click(object sender, RoutedEventArgs e)
    {
        forceColorTimer.Stop();
        bool ok = ColorPickerWindow.ShowDialog(out Color color, ColorPickerDialogOptions.SimpleView);
        if (ok)
        {
            SaveSystem.UserData.AccentColorInactive = AccentColorManager.ConvertColorToAbgr(color);
            AccentColorManager.SetAccentColorInactive(color, true);
            SaveSystem.Save();
        }
        forceColorTimer.Start();
    }

    private static void ChangeColorizationAfterGlowMenuItem_Click(object sender, RoutedEventArgs e)
    {
        forceColorTimer.Stop();
        bool ok = ColorPickerWindow.ShowDialog(out Color color, ColorPickerDialogOptions.SimpleView);
        if (ok)
        {
            SaveSystem.UserData.ColorizationAfterglow = AccentColorManager.ConvertColorToAbgr(color);
            AccentColorManager.SetColorizationAfterglow(color, true);
            SaveSystem.Save();
        }
        forceColorTimer.Start();
    }

    private static void ChangeColorizationColorMenuItem_Click(object sender, RoutedEventArgs e)
    {
        forceColorTimer.Stop();
        bool ok = ColorPickerWindow.ShowDialog(out Color color, ColorPickerDialogOptions.SimpleView);
        if (ok)
        {
            SaveSystem.UserData.ColorizationColor = AccentColorManager.ConvertColorToAbgr(color);
            AccentColorManager.SetColorizationColor(color, true);
            SaveSystem.Save();
        }
        forceColorTimer.Start();
    }

    private static void RunOnStartupItem_Click(object sender, RoutedEventArgs e)
    {
        if (StartupManager.IsStartupEnabled)
        {
            StartupManager.DisableStartup();
            return;
        }
        StartupManager.EnableStartup();
    }

    private static void GithubMenuItem_Click(object sender, RoutedEventArgs e)
    {
        string url = "https://github.com/J0nathan550/Change-Accent-Color";

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred when openning browser: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }

    private static void ExitRemoveApp_Click(object sender, RoutedEventArgs e)
    {
        forceColorTimer.Stop();
        AccentColorManager.SetAccentColor(_accentColor, false);
        AccentColorManager.SetAccentColorInactive(_accentColorInactive, false);
        AccentColorManager.SetColorizationAfterglow(_colorizationAfterglow, false);
        AccentColorManager.SetColorizationColor(_colorizationColor, false);
        SaveSystem.Save();
        Application.Current.Shutdown();
    }

    private static void ExitKeepApp_Click(object sender, RoutedEventArgs e)
    {
        forceColorTimer.Stop();
        AccentColorManager.SetAccentColor(AccentColorManager.ConvertAbgrToColor(SaveSystem.UserData.AccentColor), false);
        AccentColorManager.SetAccentColorInactive(AccentColorManager.ConvertAbgrToColor(SaveSystem.UserData.AccentColorInactive), false);
        AccentColorManager.SetColorizationAfterglow(AccentColorManager.ConvertAbgrToColor(SaveSystem.UserData.ColorizationAfterglow), false);
        AccentColorManager.SetColorizationColor(AccentColorManager.ConvertAbgrToColor(SaveSystem.UserData.ColorizationColor), false);
        SaveSystem.Save();
        Application.Current.Shutdown();
    }
}