using PasswordSafe.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordSafe.Functions
{
    public static class Safe
    {
        public static void CreateSafe(string Location, string Password)
        {
            Globals.settings.LastOpenedSafe = Location;

            Globals.currentSafe = new Types.Safe();
            Globals.currentSafe.SafeKey = KeyGen.New(new List<string> { Password }, 0, 16);
            Globals.currentSafe.SafeData = "{}";

            Settings.SaveSettings(Globals.settings);

            SaveSafe(Location, Password, Globals.currentSafe);
            Types.Safe safe = LoadSafe(Location, Password);

            if(safe == Globals.currentSafe)
            {
                MessageBox.Show("Yes");
            } else
            {
                MessageBox.Show("No");
            }
        }

        public static void SaveSafe(string Location, string Password, Types.Safe currentSafe)
        {
            Types.SafeData data = JsonSerializer.Deserialize<Types.SafeData>(currentSafe.SafeData);
            byte[] keyAA = Encryption.GenerateKey(KeyGen.New(new List<string> { currentSafe.SafeKey }, 0));
            byte[] keyA = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                keyA[i] = keyAA[i];
            }

            for (int i = 0; i < data.SafeEntries.Count; i++)
            {
                data.SafeEntries[i] = Encryption.Encrypt(data.SafeEntries[i], keyA);
            }

            string dataA = JsonSerializer.Serialize(data);
            currentSafe.SafeData = Encryption.Encrypt(dataA, keyA);

            byte[] keyBB = Encryption.GenerateKey(KeyGen.New(new List<string> { Password }, 0));
            byte[] keyB = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                keyB[i] = keyBB[i];
            }

            string dataB = JsonSerializer.Serialize(currentSafe);
            string file = Encryption.Encrypt(dataB, keyB);

            File.WriteAllText(Location, file);
        }

        public static Types.Safe LoadSafe(string Location, string Password)
        {
            if(!File.Exists(Location))
            {
                throw new Exception("Invalid file location");
            }

            string file = File.ReadAllText(Location);

            byte[] keyBB = Encryption.GenerateKey(KeyGen.New(new List<string> { Password }, 0));
            byte[] keyB = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                keyB[i] = keyBB[i];
            }

            Types.Safe currentSafe = JsonSerializer.Deserialize<Types.Safe>(Encryption.Decrypt(file, keyB));

            byte[] keyAA = Encryption.GenerateKey(KeyGen.New(new List<string> { currentSafe.SafeKey }, 0));
            byte[] keyA = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                keyA[i] = keyAA[i];
            }

            currentSafe.SafeData = Encryption.Decrypt(currentSafe.SafeData, keyA);
            Types.SafeData data = JsonSerializer.Deserialize<Types.SafeData>(currentSafe.SafeData);

            for (int i = 0; i < data.SafeEntries.Count; i++)
            {
                data.SafeEntries[i] = Encryption.Decrypt(data.SafeEntries[i], keyA);
            }

            currentSafe.SafeData = JsonSerializer.Serialize(data);

            return currentSafe;
        }

        public static Types.ActualSafe SafeToActual(Types.Safe safe)
        {
            Types.ActualSafe actual = new Types.ActualSafe();
            actual.SafeKey = safe.SafeKey;
            Types.SafeData safeData = JsonSerializer.Deserialize<Types.SafeData>(safe.SafeData);
            actual.SafeData = new Types.ActualSafeData();
            actual.SafeData.DefaultEmail = safeData.DefaultEmail;
            actual.SafeData.DefaultUsername = safeData.DefaultUsername;
            actual.SafeData.SafeEntries = new List<SafeEntry>();

            for (int i = 0; i < safeData.SafeEntries.Count; i++)
            {
                actual.SafeData.SafeEntries.Add(JsonSerializer.Deserialize<Types.SafeEntry>(safeData.SafeEntries[i]));
            }

            return actual;
        }

        // ChatGPT because yes
        public static Types.Safe ActualToSafe(Types.ActualSafe actual)
        {
            Types.Safe safe = new Types.Safe();
            safe.SafeKey = actual.SafeKey;

            Types.SafeData safeData = new Types.SafeData();
            safeData.DefaultEmail = actual.SafeData.DefaultEmail;
            safeData.DefaultUsername = actual.SafeData.DefaultUsername;
            safeData.SafeEntries = new List<string>();

            for (int i = 0; i < actual.SafeData.SafeEntries.Count; i++)
            {
                safeData.SafeEntries.Add(JsonSerializer.Serialize(actual.SafeData.SafeEntries[i]));
            }

            safe.SafeData = JsonSerializer.Serialize(safeData);

            return safe;
        }
    }
}
