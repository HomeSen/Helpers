using System;
using NUnit.Framework;

namespace HomeSen.Helpers.Crypto.Tests
{
    [TestFixture]
    public class SymmetricTests
    {
        [SetUp]
        public void Init()
        {
        }

        [TearDown]
        public void Cleanup()
        {
        }

        #region PropertyKey Tests
        [Test]
        public void PropertyKey_NullValue_SetsNullKey()
        {
            Symmetric symmetricHelper = new Symmetric();
            symmetricHelper.Key = null;

            Assert.IsNull(symmetricHelper.Key);
        }

        [Test]
        public void PropertyKey_EmptyByteArray_SetsNullKey()
        {
            Symmetric symmetricHelper = new Symmetric();
            symmetricHelper.Key = new byte[] { };

            Assert.IsNull(symmetricHelper.Key);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "The given Key value has an invalid size for this algorithm.\r\nParametername: Key")]
        public void PropertyKey_KeyWithInvalidLength_Throws([Values(16, 1024)] int keySize)
        {
            Symmetric symmetricHelper = new Symmetric();
            symmetricHelper.Key = SymmetricTestsHelper.CreateKey(keySize);
        }

        [Test]
        public void PropertyKey_ValidKey_SetsKey()
        {
            byte[] key = SymmetricTestsHelper.CreateKey(64);
            Symmetric symmetricHelper = new Symmetric();
            symmetricHelper.Key = key;

            Assert.AreEqual(key, symmetricHelper.Key);
        } 
        #endregion

        #region PropertyIV Tests
        [Test]
        public void PropertyIV_NullValue_DoesntChange()
        {
            byte[] newIV = null;
            Symmetric symmetricHelper = new Symmetric();
            byte[] oldIV = symmetricHelper.IV;
            byte[] oldKey = symmetricHelper.Key;

            symmetricHelper.IV = newIV;

            Assert.AreEqual(oldIV, symmetricHelper.IV);
            Assert.AreEqual(oldKey, symmetricHelper.Key);
        }

        [Test]
        public void PropertyIV_EmptyByteArray_DoesntChange()
        {
            byte[] newIV = new byte[] { };
            Symmetric symmetricHelper = new Symmetric();
            byte[] oldIV = symmetricHelper.IV;
            byte[] oldKey = symmetricHelper.Key;

            symmetricHelper.IV = newIV;

            Assert.AreEqual(oldIV, symmetricHelper.IV);
            Assert.AreEqual(oldKey, symmetricHelper.Key);
        }

        [Test]
        public void PropertyIV_DefaultIV_ChangesKeyAndIV(
            [Values(Algorithm.DES, Algorithm.TripleDES, Algorithm.RC2, Algorithm.Rijndael)] Algorithm mode)
        {
            byte[] newIV = SymmetricTestsHelper.GetDefaultIV(mode);
            Symmetric symmetricHelper = new Symmetric(mode);
            byte[] oldIV = symmetricHelper.IV;
            byte[] oldKey = symmetricHelper.Key;

            symmetricHelper.IV = newIV;

            Assert.AreNotEqual(oldIV, symmetricHelper.IV);
            Assert.AreNotEqual(oldKey, symmetricHelper.Key);
        }
        #endregion

        #region Constructor Tests
        [Test]
        public void Constructor_DefaultConstructor_InitializesWithDefaultAlgorithm()
        {
            Symmetric symmetricHelper = new Symmetric();

            Assert.AreEqual(Symmetric.DEFAULT_ALGORITHM, symmetricHelper.Mode);
        }

        [Test]
        public void Constructor_DefaultConstructor_InitializesWithNewKeyAndIV()
        {
            Symmetric symmetricHelper = new Symmetric();

            Assert.IsNotNull(symmetricHelper.Key);
            Assert.IsNotNull(symmetricHelper.IV);
        }

        [Test]
        public void Constructor_WithMode_InitializesWithKeyAndIV(
            [Values(Algorithm.DES, Algorithm.TripleDES, Algorithm.RC2, Algorithm.Rijndael)] Algorithm mode)
        {
            Symmetric symmetricHelper = new Symmetric(mode);

            Assert.IsNotNull(symmetricHelper.Key);
            Assert.IsNotNull(symmetricHelper.IV);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "The given keySize is invalid.\r\nParametername: keySize")]
        public void Constructor_WithModeAndInvalidKeySize_Throws(
            [Values(Algorithm.DES, Algorithm.TripleDES, Algorithm.RC2, Algorithm.Rijndael)] Algorithm mode,
            [Values(0, 1024)] int keySize)
        {
            Symmetric symmetricHelper = new Symmetric(mode, keySize);
        }

        [Test, Sequential]
        public void Constructor_WithModeAndValidKeySize_InitializesWithKeyAndIV(
            [Values(Algorithm.DES, Algorithm.TripleDES, Algorithm.RC2, Algorithm.Rijndael)] Algorithm mode,
            [Values(64, 128, 48, 192)] int keySize)
        {
            Symmetric symmetricHelper = new Symmetric(mode, keySize);

            Assert.AreEqual(mode, symmetricHelper.Mode);
            Assert.AreEqual(keySize, symmetricHelper.KeySize);
            Assert.IsNotNull(symmetricHelper.Key);
            Assert.IsNotNull(symmetricHelper.IV);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "The given keySize is invalid.\r\nParametername: keySize")]
        public void Constructor_WithModeInvalidKeySizeAndValidSalt_Throws(
            [Values(Algorithm.DES, Algorithm.TripleDES, Algorithm.RC2, Algorithm.Rijndael)] Algorithm mode,
            [Values(0, 1024)] int keySize)
        {
            string salt = SymmetricTestsHelper.DEFAULT_SALT;
            Symmetric symmetricHelper = new Symmetric(mode, keySize, salt);
        }

        [Test, Sequential]
        [ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "salt is null or empty.\r\nParametername: salt")]
        public void Constructor_WithModeValidKeySizeAndEmptySalt_Throws(
            [Values(Algorithm.DES, Algorithm.TripleDES, Algorithm.RC2, Algorithm.Rijndael)] Algorithm mode,
            [Values(64, 128, 48, 192)] int keySize)
        {
            string salt = String.Empty;
            Symmetric symmetricHelper = new Symmetric(mode, keySize, salt);
        }

        [Test, Sequential]
        [ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "salt is null or empty.\r\nParametername: salt")]
        public void Constructor_WithModeValidKeySizeAndNullSalt_Throws(
            [Values(Algorithm.DES, Algorithm.TripleDES, Algorithm.RC2, Algorithm.Rijndael)] Algorithm mode,
            [Values(64, 128, 48, 192)] int keySize)
        {
            Symmetric symmetricHelper = new Symmetric(mode, keySize, null);
        }

        [Test, Sequential]
        public void Constructor_WithModeValidKeySizeAndEmptySalt_InitializesWithSaltKeyAndIV(
            [Values(Algorithm.DES, Algorithm.TripleDES, Algorithm.RC2, Algorithm.Rijndael)] Algorithm mode,
            [Values(64, 128, 48, 192)] int keySize)
        {
            string salt = SymmetricTestsHelper.DEFAULT_SALT;
            Symmetric symmetricHelper = new Symmetric(mode, keySize, salt);

            Assert.AreEqual(mode, symmetricHelper.Mode);
            Assert.AreEqual(keySize, symmetricHelper.KeySize);
            Assert.AreEqual(salt, symmetricHelper.Salt);
            Assert.IsNotNull(symmetricHelper.Key);
            Assert.IsNotNull(symmetricHelper.IV);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "The given key has an invalid size for this algorithm.\r\nParametername: key")]
        public void Constructor_FullConstructorWithInvalidKeyLength_Throws(
            [Values(Algorithm.DES, Algorithm.TripleDES, Algorithm.RC2, Algorithm.Rijndael)] Algorithm mode,
            [Values(16, 1024)] int keySize)
        {
            string salt = SymmetricTestsHelper.DEFAULT_SALT;
            byte[] iv = SymmetricTestsHelper.GetDefaultIV(mode);
            byte[] key = SymmetricTestsHelper.CreateKey(keySize);

            Symmetric symmetricHelper = new Symmetric(mode, salt, iv, key);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "key is null or empty.\r\nParametername: key")]
        public void Constructor_FullConstructorWithNullKey_Throws(
            [Values(Algorithm.DES, Algorithm.TripleDES, Algorithm.RC2, Algorithm.Rijndael)] Algorithm mode)
        {
            string salt = SymmetricTestsHelper.DEFAULT_SALT;
            byte[] iv = SymmetricTestsHelper.GetDefaultIV(mode);
            byte[] key = null;

            Symmetric symmetricHelper = new Symmetric(mode, salt, iv, key);
        }

        [Test, Sequential]
        [ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "salt is null or empty.\r\nParametername: salt")]
        public void Constructor_FullConstructorWithEmptySalt_Throws(
            [Values(Algorithm.DES, Algorithm.TripleDES, Algorithm.RC2, Algorithm.Rijndael)] Algorithm mode,
            [Values(64, 128, 48, 192)] int keySize)
        {
            string salt = String.Empty;
            byte[] iv = SymmetricTestsHelper.GetDefaultIV(mode);
            byte[] key = SymmetricTestsHelper.CreateKey(keySize);

            Symmetric symmetricHelper = new Symmetric(mode, salt, iv, key);
        }

        [Test, Sequential]
        [ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "iv is null or empty.\r\nParametername: iv")]
        public void Constructor_FullConstructorWithNullIV_Throws(
            [Values(Algorithm.DES, Algorithm.TripleDES, Algorithm.RC2, Algorithm.Rijndael)] Algorithm mode,
            [Values(64, 128, 48, 192)] int keySize)
        {
            string salt = SymmetricTestsHelper.DEFAULT_SALT;
            byte[] iv = null;
            byte[] key = SymmetricTestsHelper.CreateKey(keySize);

            Symmetric symmetricHelper = new Symmetric(mode, salt, iv, key);
        }

        [Test, Sequential]
        [ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "iv is null or empty.\r\nParametername: iv")]
        public void Constructor_FullConstructorWithEmptyIV_Throws(
            [Values(Algorithm.DES, Algorithm.TripleDES, Algorithm.RC2, Algorithm.Rijndael)] Algorithm mode,
            [Values(64, 128, 48, 192)] int keySize)
        {
            string salt = SymmetricTestsHelper.DEFAULT_SALT;
            byte[] iv = new byte[] { };
            byte[] key = SymmetricTestsHelper.CreateKey(keySize);

            Symmetric symmetricHelper = new Symmetric(mode, salt, iv, key);
        }

        [Test, Sequential]
        public void Constructor_FullConstructorWithValidParameters_InitializesWithSaltKeyAndIV(
            [Values(Algorithm.DES, Algorithm.TripleDES, Algorithm.RC2, Algorithm.Rijndael)] Algorithm mode,
            [Values(64, 128, 48, 192)] int keySize)
        {
            string salt = SymmetricTestsHelper.DEFAULT_SALT;
            byte[] iv = SymmetricTestsHelper.GetDefaultIV(mode);
            byte[] key = SymmetricTestsHelper.CreateKey(keySize);

            Symmetric symmetricHelper = new Symmetric(mode, salt, iv, key);

            Assert.AreEqual(mode, symmetricHelper.Mode);
            Assert.AreEqual(keySize, symmetricHelper.KeySize);
            Assert.AreEqual(salt, symmetricHelper.Salt);
            Assert.AreEqual(key, symmetricHelper.Key);
            Assert.AreEqual(iv, symmetricHelper.IV);
        }
        #endregion

        #region Encrypt
        [Test]
        public void Encrypt_WithNullMessage_ReturnsNull()
        {
            string message = null;
            Symmetric symmetricHelper = new Symmetric();
            byte[] actual = symmetricHelper.Encrypt(message);

            Assert.IsNull(actual);
        }

        [Test]
        public void Encrypt_WithEmptyMessage_ReturnsNull()
        {
            string message = String.Empty;
            Symmetric symmetricHelper = new Symmetric();
            byte[] actual = symmetricHelper.Encrypt(message);

            Assert.IsNull(actual);
        }

        [Test]
        public void Encrypt_WithMessage_ReturnsSecret(
            [Values(Algorithm.DES, Algorithm.TripleDES, Algorithm.RC2, Algorithm.Rijndael)] Algorithm mode)
        {
            string message = SymmetricTestsHelper.DEFAULT_MESSAGE;
            byte[] expected = SymmetricTestsHelper.GetDefaultSecret(mode);

            Symmetric symmetricHelper = new Symmetric(mode);
            symmetricHelper.Salt = SymmetricTestsHelper.DEFAULT_SALT;
            symmetricHelper.IV = SymmetricTestsHelper.GetDefaultIV(mode);
            symmetricHelper.Key = SymmetricTestsHelper.GetDefaultKey(mode);

            byte[] actual = symmetricHelper.Encrypt(message);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EncryptToString_WithNullMessage_ReturnsEmptyString()
        {
            string message = null;
            Symmetric symmetricHelper = new Symmetric();
            string actual = symmetricHelper.EncryptToString(message);

            Assert.IsNullOrEmpty(actual);
        }

        [Test]
        public void EncryptToString_WithEmptyMessage_ReturnsEmptyString()
        {
            string message = String.Empty;
            Symmetric symmetricHelper = new Symmetric();
            string actual = symmetricHelper.EncryptToString(message);

            Assert.IsNullOrEmpty(actual);
        }

        [Test]
        public void EncryptToString_WithMessage_ReturnsSecret(
            [Values(Algorithm.DES, Algorithm.TripleDES, Algorithm.RC2, Algorithm.Rijndael)] Algorithm mode)
        {
            string message = SymmetricTestsHelper.DEFAULT_MESSAGE;
            string expected = SymmetricTestsHelper.GetDefaultSecretString(mode);

            Symmetric symmetricHelper = new Symmetric(mode);
            symmetricHelper.Salt = SymmetricTestsHelper.DEFAULT_SALT;
            symmetricHelper.IV = SymmetricTestsHelper.GetDefaultIV(mode);
            symmetricHelper.Key = SymmetricTestsHelper.GetDefaultKey(mode);

            string actual = symmetricHelper.EncryptToString(message);

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Decrypt
        [Test]
        public void Decrypt_WithNullSecret_ReturnsEmptyString()
        {
            byte[] secret = null;

            Symmetric symmetricHelper = new Symmetric();
            string actual = symmetricHelper.Decrypt(secret);

            Assert.IsNullOrEmpty(actual);
        }

        [Test]
        public void Decrypt_WithByteArraySecret_ReturnsMessage(
            [Values(Algorithm.DES, Algorithm.TripleDES, Algorithm.RC2, Algorithm.Rijndael)] Algorithm mode)
        {
            byte[] secret = SymmetricTestsHelper.GetDefaultSecret(mode);
            string expected = SymmetricTestsHelper.DEFAULT_MESSAGE;

            Symmetric symmetricHelper = new Symmetric(mode);
            symmetricHelper.Salt = SymmetricTestsHelper.DEFAULT_SALT;
            symmetricHelper.IV = SymmetricTestsHelper.GetDefaultIV(mode);
            symmetricHelper.Key = SymmetricTestsHelper.GetDefaultKey(mode);

            string actual = symmetricHelper.Decrypt(secret);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Decrypt_WithNullString_ReturnsEmptyString(
            [Values(Algorithm.DES, Algorithm.TripleDES, Algorithm.RC2, Algorithm.Rijndael)] Algorithm mode)
        {
            string secret = null;

            Symmetric symmetricHelper = new Symmetric(mode);
            symmetricHelper.Salt = SymmetricTestsHelper.DEFAULT_SALT;
            symmetricHelper.IV = SymmetricTestsHelper.GetDefaultIV(mode);
            symmetricHelper.Key = SymmetricTestsHelper.GetDefaultKey(mode);

            string actual = symmetricHelper.Decrypt(secret);

            Assert.IsNullOrEmpty(actual);
        }

        [Test]
        public void Decrypt_WithEmptyString_ReturnsEmptyString(
            [Values(Algorithm.DES, Algorithm.TripleDES, Algorithm.RC2, Algorithm.Rijndael)] Algorithm mode)
        {
            string secret = String.Empty;

            Symmetric symmetricHelper = new Symmetric(mode);
            symmetricHelper.Salt = SymmetricTestsHelper.DEFAULT_SALT;
            symmetricHelper.IV = SymmetricTestsHelper.GetDefaultIV(mode);
            symmetricHelper.Key = SymmetricTestsHelper.GetDefaultKey(mode);

            string actual = symmetricHelper.Decrypt(secret);

            Assert.IsNullOrEmpty(actual);
        }

        [Test]
        public void Decrypt_WithDefaultSecretString_ReturnsEmptyString(
            [Values(Algorithm.DES, Algorithm.TripleDES, Algorithm.RC2, Algorithm.Rijndael)] Algorithm mode)
        {
            string secret = SymmetricTestsHelper.GetDefaultSecretString(mode);
            string expected = SymmetricTestsHelper.DEFAULT_MESSAGE;

            Symmetric symmetricHelper = new Symmetric(mode);
            symmetricHelper.Salt = SymmetricTestsHelper.DEFAULT_SALT;
            symmetricHelper.IV = SymmetricTestsHelper.GetDefaultIV(mode);
            symmetricHelper.Key = SymmetricTestsHelper.GetDefaultKey(mode);

            string actual = symmetricHelper.Decrypt(secret);

            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
