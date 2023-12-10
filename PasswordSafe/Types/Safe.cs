using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordSafe.Types
{
    public class Safe
    {
        public string SafeKey = "";
        public string SafeData = "";
    }

    public class ActualSafe
    {
        public string SafeKey = "";
        public ActualSafeData SafeData = new ActualSafeData();
    }

    public class SafeData
    {
        public string DefaultUsername = "";
        public string DefaultEmail = "";

        public List<string> SafeEntries = new List<string>();
    }

    public class ActualSafeData
    {
        public string DefaultUsername = "";
        public string DefaultEmail = "";
        
        public List<SafeEntry> SafeEntries = new List<SafeEntry>();
    }

    public class SafeEntry
    {
        public string Username = "";
        public string Email = "";
        public string Notes = "";

        public string Password = "";
    }
}
