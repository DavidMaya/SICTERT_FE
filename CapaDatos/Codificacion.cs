using java.io;
using java.security;
using java.security.cert;
using java.util;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AccesoDatos
{
    public class Codificacion
    {
        private static readonly byte[] baClavePredet = ASCIIEncoding.ASCII.GetBytes("sicTERT+");

        public static string EncriptarCadena(string stOriginal)
        {
            try
            {
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(baClavePredet, baClavePredet), CryptoStreamMode.Write);
                StreamWriter writer = new StreamWriter(cryptoStream);
                writer.Write(stOriginal);
                writer.Flush();
                cryptoStream.FlushFinalBlock();
                writer.Flush();

                return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            }
            catch
            {
                return stOriginal;
            }
        }

        public static string EncriptarCadena(string stOriginal, byte[] baClave)
        {
            try
            {
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(baClave, baClave), CryptoStreamMode.Write);
                StreamWriter writer = new StreamWriter(cryptoStream);
                writer.Write(stOriginal);
                writer.Flush();
                cryptoStream.FlushFinalBlock();
                writer.Flush();

                return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            }
            catch
            {
                return stOriginal;
            }
        }

        public static string DesencriptarCadena(string stEncriptada)
        {
            try
            {
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(stEncriptada));
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(baClavePredet, baClavePredet), CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(cryptoStream);

                return reader.ReadToEnd();
            }
            catch
            {
                return stEncriptada;
            }
        }

        public static string DesencriptarCadena(string stEncriptada, byte[] baClave)
        {
            try
            {
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(stEncriptada));
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(baClave, baClave), CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(cryptoStream);

                return reader.ReadToEnd();
            }
            catch
            {
                return stEncriptada;
            }
        }

        public static bool VerificarClaveCertificadoP12(string certificado, string clave)
        {
            try
            {
                X509Certificate certificate = default(X509Certificate);
                PrivateKey key = default(PrivateKey);
                Provider provider = default(Provider);
                string str = clave;
                KeyStore store = KeyStore.getInstance("PKCS12");
                store.load(new FileInputStream(certificado), str.ToCharArray());
                Enumeration enumeration = store.aliases();
                while (enumeration.hasMoreElements())
                {
                    string alias1 = Convert.ToString(enumeration.nextElement());
                    if (store.isKeyEntry(alias1))
                    {
                        certificate = (X509Certificate)store.getCertificate(alias1);
                        key = (PrivateKey)store.getKey(alias1, str.ToCharArray());
                        provider = store.getProvider();
                        break;
                    }
                }
                provider = null;
                key = null;
                certificate = null;
                enumeration = null;
                store = null;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}