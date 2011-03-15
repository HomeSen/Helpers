using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace HomeSen.Helpers.Crypto
{
    public enum Algorithm
    {
        DES,
        TripleDES,
        RC2,
        Rijndael
    };

    public class Symmetric
    {
        #region Fields

        private byte[] _Iv;
        private byte[] _Key;
        private string _Salt;
        private int _KeySize = 0;
        private Algorithm _Mode;
        private SymmetricAlgorithm myCSP;

        private int KEY_SIZE_MIN = 0;
        private int KEY_SIZE_MAX = 0;
        private int KEY_SIZE_STEP = 0;

        public const Algorithm DEFAULT_ALGORITHM = Algorithm.DES;

        #endregion


        #region Properties

        public byte[] IV
        {
            get { return this._Iv; }
            set
            {
                if (value == null || value.Length == 0)
                    return;

                myCSP.IV = value;
                myCSP.GenerateKey();

                this._Iv = value;
                this._Key = myCSP.Key;
                this._KeySize = this._Key.Length * 8;
            }
        }

        public byte[] Key
        {
            get { return this._Key;  }
            set
            {
                if (value == null || value.Length == 0)
                {
                    this._Key = null;
                    return;
                }
                else if (IsKeySizeValid(value.Length * 8) == false)
                    throw new ArgumentException("The given Key value has an invalid size for this algorithm.", "Key");

                this._Key = value;
                this._KeySize = this._Key.Length * 8;

            }
        }

        public string Salt
        {
            get { return this._Salt; }
            set { this._Salt = value; }
        }

        public int KeySize
        {
            get { return this._KeySize; }
        }

        public Algorithm Mode
        {
            get { return this._Mode; }
        }

        #endregion


        #region Constructor(s)

        public Symmetric()
        {
            this._Mode = DEFAULT_ALGORITHM;
            Init();
        }

        public Symmetric(Algorithm mode)
        {
            this._Mode = mode;
            Init();
        }

        public Symmetric(Algorithm mode, int keySize)
        {
            if (IsKeySizeValid(keySize, mode) == false)
                throw new ArgumentException("The given keySize is invalid.", "keySize");
            this._Mode = mode;
            this._KeySize = keySize;
            Init();
        }

        public Symmetric(Algorithm mode, int keySize, string salt)
        {
            if (String.IsNullOrEmpty(salt))
                throw new ArgumentException("salt is null or empty.", "salt");
            if (IsKeySizeValid(keySize, mode) == false)
                throw new ArgumentException("The given keySize is invalid.", "keySize");

            this._Mode = mode;
            this._KeySize = keySize;
            this._Salt = salt;
            Init();
        }

        public Symmetric(Algorithm mode, string salt, byte[] iv, byte[] key)
        {
            if (String.IsNullOrEmpty(salt))
                throw new ArgumentException("salt is null or empty.", "salt");
            if (iv == null || iv.Length == 0)
                throw new ArgumentException("iv is null or empty.", "iv");
            if (key == null || key.Length == 0)
                throw new ArgumentException("key is null or empty.", "key");
            if (IsKeySizeValid(key.Length * 8, mode) == false)
                throw new ArgumentException("The given key has an invalid size for this algorithm.", "key");
            
            this._Mode = mode;
            this._Salt = salt;
            this._KeySize = key.Length * 8;
            this._Iv = iv;
            this._Key = key;
            Init();
        }

        #endregion


        #region Initializers

        private void Init()
        {
            switch (this._Mode)
            {
                case Algorithm.DES:
                    this.myCSP = new DESCryptoServiceProvider();
                    break;
                case Algorithm.TripleDES:
                    this.myCSP = new TripleDESCryptoServiceProvider();
                    break;
                case Algorithm.RC2:
                    this.myCSP = new RC2CryptoServiceProvider();
                    break;
                case Algorithm.Rijndael:
                    this.myCSP = Rijndael.Create();
                    break;
            }

            this.KEY_SIZE_MIN = myCSP.LegalKeySizes[0].MinSize;
            this.KEY_SIZE_MAX = myCSP.LegalKeySizes[0].MaxSize;
            this.KEY_SIZE_STEP = myCSP.LegalKeySizes[0].SkipSize;

            if (IsKeySizeValid(this._KeySize) == false)
            {
                this._KeySize = KEY_SIZE_MAX;
                this._Key = null;
            }
            this.myCSP.KeySize = this._KeySize;

            if (this._Iv == null)
            {
                this.myCSP.GenerateIV();
                this._Iv = this.myCSP.IV;
            }
            else
                this.myCSP.IV = this._Iv;

            if (this._Key == null)
            {
                this.myCSP.GenerateKey();
                this._Key = this.myCSP.Key;
            }
            else
                this.myCSP.Key = this._Key;
        
        }

        #endregion

        #region Check Methods

        private bool IsKeySizeValid(int size)
        {
            return myCSP.ValidKeySize(size);
        }

        private bool IsKeySizeValid(int size, Algorithm algorithm)
        {
            SymmetricAlgorithm alg;

            switch (algorithm)
            {
                case Algorithm.DES:
                    alg = new DESCryptoServiceProvider();
                    break;
                case Algorithm.TripleDES:
                    alg = new TripleDESCryptoServiceProvider();
                    break;
                case Algorithm.RC2:
                    alg = new RC2CryptoServiceProvider();
                    break;
                case Algorithm.Rijndael:
                    alg = Rijndael.Create();
                    break;
                default:
                    return false;
            }

            return alg.ValidKeySize(size);
        }

        #endregion


        #region Encryption

        public byte[] Encrypt(string message)
        {
            if (String.IsNullOrEmpty(message))
                return null;
            if (this._Iv == null || this._Iv.Length == 0)
                Init();
            if (this._Key == null || this._Key.Length == 0)
                Init();

            byte[] encrypted;
            UTF8Encoding textConverter = new UTF8Encoding();
            // Convert data to byte array
            byte[] toEncrypt = textConverter.GetBytes(message);            
            // Get an encryptor
            ICryptoTransform encryptor = this.myCSP.CreateEncryptor(this._Key, this._Iv);
            // Encrypt the data
            MemoryStream msEncrypt = new MemoryStream();
            CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

            // Write all data to the crypto stream and flush it
            byte[] length = new byte[4];
            length[0] = (byte)(message.Length & 0xFF);
            length[1] = (byte)((message.Length >> 8) & 0xFF);
            length[2] = (byte)((message.Length >> 16) & 0xFF);
            length[3] = (byte)((message.Length >> 24) & 0xFF);
            csEncrypt.Write(length, 0, 4);
            csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
            csEncrypt.FlushFinalBlock();
            // Get encrypted array of bytes
            encrypted = msEncrypt.ToArray();

            return encrypted;
        }

        public string EncryptToString(string message)
        {
            if (String.IsNullOrEmpty(message))
                return String.Empty;

            byte[] secret = Encrypt(message);
            
            return Convert.ToBase64String(secret);
        }

        #endregion


        #region Decryption

        public string Decrypt(byte[] encrypted)
        {
            if (encrypted == null || encrypted.Length == 0)
                return String.Empty;

            string message = "";
            UTF8Encoding textConverter = new UTF8Encoding();
            int len = 0;
            // Get a decryptor
            ICryptoTransform decryptor = this.myCSP.CreateDecryptor(this._Key, this._Iv);
            // Decrypt the data
            MemoryStream msDecrypt = new MemoryStream(encrypted);
            CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            byte[] fromEncrypt = new byte[encrypted.Length];

            // Read the data of the crypto stream
            byte[] length = new byte[4];
            csDecrypt.Read(length, 0, 4);
            csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length - 4);
            // Get message length
            len = (int)length[0] | (length[1] << 8) | (length[2] << 16) | (length[3] << 24);
            // Convert the byte array back to a string
            message = textConverter.GetString(fromEncrypt).Substring(0, len);

            return message;
        }

        public string Decrypt(string encrypted)
        {
            if (String.IsNullOrEmpty(encrypted))
                return String.Empty;

            byte[] enc = Convert.FromBase64String(encrypted);

            return Decrypt(enc);
        }

        #endregion
    }
}
