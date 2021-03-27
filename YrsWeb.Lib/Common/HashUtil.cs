using System;
using System.Security.Cryptography;
using System.Text;
namespace YrsWeb.Lib.Common
{
    public sealed class HashUtil
    {
        public static String GetHashString<T>(String text) where T : HashAlgorithm, new()
        {
            // 文字列をバイト型配列に変換する
            byte[] data = Encoding.UTF8.GetBytes(text);

            // ハッシュアルゴリズム生成
            var algorithm = new T();

            // ハッシュ値を計算する
            byte[] bs = algorithm.ComputeHash(data);

            // リソースを解放する
            algorithm.Clear();

            // バイト型配列を16進数文字列に変換
            var result = new StringBuilder();
            foreach (byte b in bs)
            {
                result.Append(b.ToString("X2"));
            }
            return result.ToString();
        }

        public static String GetHashString(String text, String algorithm = "SHA512")
        {
            switch (algorithm.ToUpper())
            {
                case "MD5": return GetHashString<MD5CryptoServiceProvider>(text);
                case "SHA1": return GetHashString<SHA1CryptoServiceProvider>(text);
                case "SHA256": return GetHashString<SHA256CryptoServiceProvider>(text);
                case "SHA512": return GetHashString<SHA512CryptoServiceProvider>(text);
                default: return GetHashString<SHA512CryptoServiceProvider>(text);
            }
        }

        //public static void main()
        //{
        //    var key = "password0123456789";

        //    var md5 = GetHashString(key, "MD5");
        //    var sha1 = GetHashString(key, "SHA1");
        //    var sha256 = GetHashString(key, "SHA128");
        //    var sha512 = GetHashString(key, "SHA512");

        //    System.Console.WriteLine(md5);
        //    System.Console.WriteLine(sha1);
        //    System.Console.WriteLine(sha256);
        //    System.Console.WriteLine(sha512);
        //}
    }
}
