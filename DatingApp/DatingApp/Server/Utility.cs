using System.Security.Cryptography;
using System.Text;

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
    }
}