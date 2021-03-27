using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace YrsWeb.Lib.Common
{
    public sealed class AesUtil
    {
        private const String AES_IV = @"8Ehw3K3GA3OJdJVX";
        private const String AES_KEY = @"of78EM6HsXGzAdNL";
        private const char URL_SEPARATOR = ';';

        /// <summary>
        /// AESを使って文字列を暗号化する
        /// </summary>
        /// <param name="plainText">暗号化する文字列</param>
        /// <returns>暗号化された文字列</returns>
        public static String Encrypt(String plainText, bool bUrlEncoding = true)
        {
            using (RijndaelManaged rijndael = new RijndaelManaged())
            {
                rijndael.BlockSize = 128;
                rijndael.KeySize = 128;
                rijndael.Mode = CipherMode.CBC;
                rijndael.Padding = PaddingMode.PKCS7;

                rijndael.IV = Encoding.UTF8.GetBytes(AES_IV);
                rijndael.Key = Encoding.UTF8.GetBytes(AES_KEY);

                ICryptoTransform encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV);

                byte[] encrypted;
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream ctStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(ctStream))
                        {
                            sw.Write(plainText);
                        }
                        encrypted = mStream.ToArray();
                    }
                }

                String encText = System.Convert.ToBase64String(encrypted);
                if (bUrlEncoding)
                {
                    System.Text.Encoding enc = System.Text.Encoding.GetEncoding("iso-2022-jp");
                    //System.Text.Encoding enc = Encoding.UTF8;
                    encText = System.Web.HttpUtility.UrlEncode(encText, enc);
                }

                return encText;
            }
        }

        /// <summary>
        /// AESを使って暗号文を復号する
        /// </summary>
        /// <param name="encText">暗号化された文字列</param>
        /// <returns>復号された文字列</returns>
        public static string Decrypt(string encText, bool bUrlEncoding)
        {
            if (bUrlEncoding)
            {
                System.Text.Encoding enc = System.Text.Encoding.GetEncoding("iso-2022-jp");
                //System.Text.Encoding enc = Encoding.UTF8;
                encText = System.Web.HttpUtility.UrlDecode(encText, enc);
            }

            using (RijndaelManaged rijndael = new RijndaelManaged())
            {
                rijndael.BlockSize = 128;
                rijndael.KeySize = 128;
                rijndael.Mode = CipherMode.CBC;
                rijndael.Padding = PaddingMode.PKCS7;

                rijndael.IV = Encoding.UTF8.GetBytes(AES_IV);
                rijndael.Key = Encoding.UTF8.GetBytes(AES_KEY);

                ICryptoTransform decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);

                string plain = string.Empty;
                using (MemoryStream mStream = new MemoryStream(System.Convert.FromBase64String(encText)))
                {
                    using (CryptoStream ctStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(ctStream))
                        {
                            plain = sr.ReadLine();
                        }
                    }
                }

                return plain;
            }
        }

        /// <summary>
        /// AESを使って文字列を暗号化する
        /// </summary>
        /// <returns>暗号化された文字列</returns>
        public static String Encrypt(String mailAddress, int requestId, bool bUrlEncoding = true)
        {
            string plainText = mailAddress;
            plainText += URL_SEPARATOR.ToString();
            plainText += requestId.ToString();

            return Encrypt(plainText, bUrlEncoding);
        }

        /// <summary>
        /// AESを使って暗号文を復号する
        /// </summary>
        /// <param name="encText">暗号化された文字列</param>
        /// <returns>復号された文字列</returns>
        public static String Decrypt(String encText, out String mailAddress, out int requestId, bool bUrlEncoding = true)
        {
            // 複号化
            String plainText = Decrypt(encText, bUrlEncoding);

            try
            {
                String[] arrText = plainText.Split(URL_SEPARATOR);
                mailAddress = arrText[0];
                requestId = int.Parse(arrText[1]);
            }
            catch
            {
                mailAddress = "";
                requestId = 0;
            }

            return plainText;
        }

        //public static void main()
        //{
        //    String mailAddress = "hmorimoto704@おなまえ.com";
        //    int requestId = 9876;

        //    String decMailAddress = "";
        //    int decRequestId = 0;

        //    String encText = Encrypt(mailAddress, requestId);
        //    String decText = Decrypt(encText, out decMailAddress, out decRequestId);

        //    System.Console.WriteLine("1)MAIL ADRESS=" + mailAddress + " ID=" + requestId.ToString());
        //    System.Console.WriteLine("2)暗号化文字列=" + encText);
        //    System.Console.WriteLine("3)復号化文字列=" + decText);
        //    System.Console.WriteLine("4)MAIL ADRESS=" + decMailAddress + " ID=" + decRequestId.ToString());
        //}
    }
}

