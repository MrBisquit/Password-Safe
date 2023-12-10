using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PasswordSafe.Functions
{
    public static class Settings
    {
        public static Types.Settings GetSettings()
        {
            Types.Settings settings = new Types.Settings();
            string AppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WTDawson", "PasswordSafe");

            if(!Directory.Exists(AppData))
            {
                Directory.CreateDirectory(AppData);
            }

            if(File.Exists(Path.Combine(AppData, ".settings")))
            {
                settings = JsonSerializer.Deserialize<Types.Settings>(File.ReadAllText(Path.Combine(AppData, ".settings")));
                if(settings == null) settings = new Types.Settings();
            }

            return settings;
        }

        public static void SaveSettings(Types.Settings Settings)
        {
            string AppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WTDawson", "PasswordSafe");
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WTDawson", "PasswordSafe");
            File.WriteAllText(Path.Combine(AppData, ".settings"), JsonSerializer.Serialize(Settings));
        }
    }
}
