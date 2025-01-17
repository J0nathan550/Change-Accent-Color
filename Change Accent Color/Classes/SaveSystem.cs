using System.IO;
using System.Text.Json;

namespace Change_Accent_Color.Classes;

public class SaveSystem
{
    private static UserData userData = new();

    public static UserData UserData { get => userData; set => userData = value; }

    public static void Load()
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        path = Path.Combine(path, "J0nathan550", "save.json");
        if (File.Exists(path))
        {
            string data = File.ReadAllText(path);
            UserData ud = JsonSerializer.Deserialize<UserData>(data)!;
            if (ud != null) { UserData = ud; }
        }
    }

    public static void Save()
    {
        string info = JsonSerializer.Serialize(UserData);
        string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        path = Path.Combine(path, "J0nathan550");
        Directory.CreateDirectory(path);
        path = Path.Combine(path, "save.json");
        File.WriteAllText(path, info);
    }
}

public class UserData
{
    public int AccentColor { get; set; }
    public int AccentColorInactive { get; set; }
    public int ColorizationAfterglow { get; set; }
    public int ColorizationColor { get; set; }
}