using System.Security.Cryptography;
using System.Text;
using DatingApp.Client.Models;

namespace DatingApp.Server{

    public class Utility
    {
        public static string Encrypt(string password)
        {
            //add unique salts later
            var provider = MD5.Create();
            var salt = "SaLtIs@ddedH@r@";
            byte[] bytes = provider.ComputeHash(Encoding.UTF32.GetBytes(salt+password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();

        }
        public static List<string> GetStringAsList(string matches)
        {
            List<string> matchArr = new();
            if(matches.Length == 0)
            {
                return matchArr;
            }
            matchArr = matches.Remove(matches.Length - 1).Split(',').ToList();
            return matchArr;
        }
    }
}