using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSen.Helpers.Conversion
{
    public class StringCreator
    {
        public static string DumpByteArray(byte[] arr)
        {
            if (arr == null || arr.Length == 0)
                return String.Empty;

            string result = @"{ ";
            for (int i = 0; i < (arr.Length - 1); i++)
                result += arr[i] + ", ";
            result += arr[arr.Length - 1] + @" }";

            return result;
        }

        public static string ByteArrayToHexString(byte[] arr)
        {
            if (arr == null || arr.Length == 0)
                return String.Empty;

            string result = "";
            foreach (byte b in arr)
                result += b.ToString("X2");

            return result;
        }

        public static string ByteArrayToUTF8String(byte[] arr)
        {
            if (arr == null || arr.Length == 0)
                return String.Empty;

            string result = "";
            UTF8Encoding textConverter = new UTF8Encoding();
            result = textConverter.GetString(arr);
            
            return result;
        }

        public static string ByteArrayToBase64String(byte[] arr)
        {
            if (arr == null || arr.Length == 0)
                return String.Empty;

            return Convert.ToBase64String(arr);
        }
    }
}
