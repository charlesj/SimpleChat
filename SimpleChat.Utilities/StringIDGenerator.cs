using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleChat.Utilities
{
    public class StringIDGenerator
    {
        public static string GetId()
        {
            return GetId(6);
        }

        public static string GetId(int length)
        {
            var str = new StringBuilder();
            var rnd = new Random(DateTime.Now.Millisecond);
            var alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
            for (int i = 0; i < length; i++)
            {
                str.Append(alphabet[rnd.Next(alphabet.Count()-1)]);
            }
            return str.ToString();
        }
    }
}
