using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordSafe.Functions
{
    public static class KeyGen
    {
        public static string New(List<string> seed, int? id, int length = 25)
        {
            if (id == null) id = 0;
            List<char> chars = new List<char>();
            seed.ForEach(input =>
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (!chars.Contains(input[i]))
                    {
                        chars.Add(input[i]);
                    }
                }
            });

            string UUID = "";
            Random r = new Random((int)id);

            for (int i = 0; i < length; i++)
            {
                UUID += chars[r.Next(chars.Count)];
            }

            return UUID;
        }
    }
}
