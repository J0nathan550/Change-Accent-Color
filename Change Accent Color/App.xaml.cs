using Change_Accent_Color.Classes;
using System.Windows;

namespace Change_Accent_Color;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Tray.Initialize();
    }
}