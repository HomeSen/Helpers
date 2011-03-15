using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSen.Helpers.Crypto.Tests
{
    class SymmetricTestsHelper
    {
        #region Keys
        public static byte[] GetDefaultKey(Algorithm mode)
        {
            switch (mode)
            {
                case Algorithm.DES:
                    return new byte[] { 68, 177, 81, 238, 56, 55, 129, 47 };
                case Algorithm.TripleDES:
                    return new byte[] { 191, 151, 66, 77, 158, 79, 84, 32, 130, 166, 188, 130, 228, 96, 75, 216, 46, 81, 34, 30, 44, 53, 251, 239 };
                case Algorithm.RC2:
                    return new byte[] { 126, 3, 122, 109, 188, 132, 11, 121, 216, 219, 185, 21, 115, 146, 208, 172 };
                case Algorithm.Rijndael:
                    return new byte[] { 65, 168, 33, 172, 213, 92, 123, 252, 36, 32, 161, 65, 253, 40, 179, 29, 58, 160, 101, 161, 168, 125, 19, 104, 157, 228, 253, 18, 217, 194, 219, 156 };
                default:
                    return null;
            }
        }

        public static byte[] CreateKey(int bits)
        {
            if (bits == 0)
                return new byte[] { };

            int bytes = bits / 8;
            byte[] result = new byte[bytes];

            for (int i = 0; i < bytes; i++)
                result[i] = (byte)(60 + i);

            return result;
        } 
        #endregion

        #region IVs
        public static byte[] GetDefaultIV(Algorithm mode)
        {
            switch (mode)
            {
                case Algorithm.DES:
                    return new byte[] { 78, 100, 44, 214, 107, 182, 251, 214 };
                case Algorithm.TripleDES:
                    return new byte[] { 30, 120, 232, 127, 47, 116, 205, 3 };
                case Algorithm.RC2:
                    return new byte[] { 108, 183, 6, 91, 218, 104, 82, 222 };
                case Algorithm.Rijndael:
                    return new byte[] { 201, 43, 11, 177, 143, 194, 176, 186, 154, 78, 191, 39, 221, 119, 230, 223 };
                default:
                    return null;
            }
        }
        #endregion

        #region Secrets
        public static byte[] GetDefaultSecret(Algorithm mode)
        {
            switch (mode)
            {
                case Algorithm.DES:
                    return new byte[] { 243, 58, 102, 193, 43, 142, 127, 36, 29, 255, 22, 22, 175, 46, 67, 3, 182, 7, 25, 153, 71, 56, 245, 195, 75, 201, 107, 53, 191, 149, 195, 230, 101, 102, 185, 72, 253, 20, 24, 109 };
                    //return new byte[] { 240, 224, 127, 184, 218, 202, 137, 242, 127, 188, 152, 135, 247, 231, 21, 48, 122, 100, 42, 16, 230, 40, 99, 76 };
                case Algorithm.TripleDES:
                    return new byte[] { 185, 222, 186, 169, 119, 157, 105, 207, 244, 52, 182, 86, 5, 206, 200, 167, 190, 208, 209, 141, 37, 37, 122, 90, 126, 41, 216, 52, 18, 1, 68, 59, 163, 248, 222, 226, 109, 133, 157, 234 };
                    //return new byte[] { 23, 10, 158, 45, 105, 28, 238, 141, 107, 222, 251, 111, 68, 10, 140, 8, 200, 254, 10, 121, 121, 176, 177, 82 };
                case Algorithm.RC2:
                    return new byte[] { 216, 11, 233, 28, 16, 255, 111, 144, 78, 147, 9, 46, 158, 241, 63, 116, 115, 230, 87, 126, 59, 115, 87, 163, 209, 2, 114, 237, 172, 173, 48, 24, 52, 234, 160, 57, 255, 173, 27, 22 };
                    //return new byte[] { 171, 78, 29, 183, 63, 148, 44, 149, 57, 183, 63, 171, 170, 51, 92, 97, 205, 163, 135, 215, 226, 35, 65, 61 };
                case Algorithm.Rijndael:
                    return new byte[] { 180, 175, 36, 200, 235, 101, 67, 100, 172, 181, 204, 71, 164, 116, 168, 73, 90, 35, 155, 152, 175, 13, 201, 209, 140, 187, 118, 73, 90, 89, 125, 48, 160, 90, 112, 86, 216, 158, 216, 215, 82, 151, 182, 217, 202, 181, 232, 189 };
                    //return new byte[] { 47, 4, 75, 93, 82, 209, 24, 125, 143, 21, 197, 10, 147, 161, 14, 239 };
                default:
                    return null;
            }
        }

        public static string GetDefaultSecretString(Algorithm mode)
        {
            byte[] secret = GetDefaultSecret(mode);

            return Conversion.StringCreator.ByteArrayToBase64String(secret);
        }
        #endregion

        #region Message
        public const string DEFAULT_MESSAGE = "This is a NUnit Test message.";
        #endregion

        #region Salt
        public const string DEFAULT_SALT = "NUnit Test Salt.";
        #endregion
    }
}
