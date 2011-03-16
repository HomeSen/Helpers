using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace HomeSen.Helpers.Conversion
{
    #region Enums

    public enum DISTANCE_RULE
    {
        SameAreaRule,
        LongDistanceRule,
        InternationalRule,
        CANONICAL
    }

    #endregion

    public class PhoneNumbers
    {
        #region Constants

        private const int MAX_COUNTRYCODE_LENGTH = 4;
        private const int MAX_AREACODE_LENGTH = 8;

        /// <summary>
        /// String representation of the canonical phone number format.
        /// </summary>
        public static string CANONICAL_FORMAT = "+E (F) G";

        /// <summary>
        /// String representation of the RegEx-pattern for valid phone numbers.
        /// </summary>
        public static string PHONE_NUMBER_PATTERN = @"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$";

        #endregion


        #region Fields
        private static Base.AbstractPhoneNumberData dataController = new PhoneNumberData();
        #endregion


        #region Properties
        internal Base.AbstractPhoneNumberData DataController
        {
            get { return dataController; }
            set { dataController = value; }
        }
        #endregion


        public static string FormatPhoneNumber(string number, DISTANCE_RULE distanceRule)
        {
            if (String.IsNullOrEmpty(number))
                return String.Empty;
            if (Regex.IsMatch(number, PHONE_NUMBER_PATTERN) == false)
                return String.Empty;

            string result = "";
            if (distanceRule == DISTANCE_RULE.CANONICAL)
                result = CANONICAL_FORMAT;
            else
                result = GetUserPhoneFormat(distanceRule);

            string countryCode = GetCountryCode(number);
            if (String.IsNullOrEmpty(countryCode))
                return String.Empty;

            string areaCode = GetAreaCode(number, countryCode);
            if (String.IsNullOrEmpty(areaCode))
                return String.Empty;

            string localNumber = GetLocalNumber(number, countryCode, areaCode);
            if (String.IsNullOrEmpty(localNumber))
                return String.Empty;

            result = result.Replace("E", countryCode);
            result = result.Replace("F", areaCode);
            result = result.Replace("G", localNumber);

            return result;
        }


        #region Phone Number Parts

        #region Area Data

        private static string GetAreaCode(string number, string countryCode)
        {
            if (String.IsNullOrEmpty(countryCode))
                return String.Empty;
            if (String.IsNullOrEmpty(number))
                return String.Empty;
            if (Regex.IsMatch(number, PHONE_NUMBER_PATTERN) == false)
                return String.Empty;

            string result = "";

            result = GetFullAreaCode(number, countryCode);
            if (String.IsNullOrEmpty(result))
                return String.Empty;

            if (result.StartsWith("+"))
                result = result.Substring(1);
            result = result.Substring(countryCode.Length);

            return result;
        }

        private static string GetFullAreaCode(string number)
        {
            if (String.IsNullOrEmpty(number))
                return String.Empty;
            if (Regex.IsMatch(number, PHONE_NUMBER_PATTERN) == false)
                return String.Empty;

            string countryCode = GetCountryCode(number);
            if (String.IsNullOrEmpty(countryCode))
                return String.Empty;

            return GetFullAreaCode(number, countryCode);
        }

        private static string GetFullAreaCode(string number, string countryCode)
        {
            if (String.IsNullOrEmpty(number))
                return String.Empty;
            if (Regex.IsMatch(number, PHONE_NUMBER_PATTERN) == false)
                return String.Empty;

            string result = "";
            string code = "";

            if (number.StartsWith("00"))
                code = "+" + number.Substring(2);
            else if (number.StartsWith("0"))
                code = "+" + countryCode + number.Substring(1);
            else if (number.StartsWith("+"))
                code = number;
            else
            {
                string areaCode = GetUserAreaCode();
                if (number.StartsWith(areaCode))
                    code = "+" + countryCode + number;
                else
                    code = "+" + countryCode + areaCode + number;
            }

            int length = Math.Min(code.Length, 1 + MAX_COUNTRYCODE_LENGTH + MAX_AREACODE_LENGTH);
            code = code.Substring(0, length);

            result = dataController.GetAreaCode(code);

            return result;
        }

        #endregion


        #region Country Data

        public static string GetCountryID(string number)
        {
            if (String.IsNullOrEmpty(number))
                return String.Empty;
            if (Regex.IsMatch(number, PHONE_NUMBER_PATTERN) == false)
                return String.Empty;

            string result = "";

            string areaCode = GetFullAreaCode(number);
            if (String.IsNullOrEmpty(areaCode))
                return String.Empty;

            result = dataController.GetCountryIDFromXML(areaCode);

            return result;
        }

        private static string GetCountryCode(string number)
        {
            if (String.IsNullOrEmpty(number))
                return String.Empty;
            if (Regex.IsMatch(number, PHONE_NUMBER_PATTERN) == false)
                return String.Empty;

            string result = "";
            string code = "";

            if (number.StartsWith("00"))
                code = number.Substring(2, MAX_COUNTRYCODE_LENGTH);
            else if (number.StartsWith("+"))
                code = number.Substring(1, MAX_COUNTRYCODE_LENGTH);
            else
                return GetUserCountryID();

            result = dataController.GetCountryCode(code);

            return result;
        }

        #endregion


        private static string GetLocalNumber(string number, string countryCode, string areaCode)
        {
            if (String.IsNullOrEmpty(number) || String.IsNullOrEmpty(countryCode) || String.IsNullOrEmpty(areaCode))
                return String.Empty;
            if (Regex.IsMatch(number, PHONE_NUMBER_PATTERN) == false)
                return String.Empty;

            string result = "";
            int pad = 0;

            if (number.StartsWith("00"))
                pad = 2 + countryCode.Length + areaCode.Length;
            else if (number.StartsWith("+"))
                pad = 1 + countryCode.Length + areaCode.Length;
            else if (number.StartsWith("0"))
                pad = 1 + areaCode.Length;

            pad = Math.Min(pad, number.Length);
            result = number.Substring(pad);

            return result;
        }

        #endregion


        #region User Settings

        public static string GetUserCountryID()
        {
            return dataController.GetUserCountryID();
        }

        public static string GetUserAreaCode()
        {
            return dataController.GetUserAreaCode();
        }

        private static string GetUserPhoneFormat(DISTANCE_RULE distanceRule)
        {
            if (distanceRule == DISTANCE_RULE.CANONICAL)
                return CANONICAL_FORMAT;

            string result = "";
            string id = GetUserCountryID();
            if (String.IsNullOrEmpty(id))
                return String.Empty;

            int userCountryID = int.Parse(id);

            result = dataController.GetPhoneFormatFromRegistry(userCountryID, distanceRule);
            if (String.IsNullOrEmpty(result))
                result = dataController.GetPhoneFormatFromXML(userCountryID, distanceRule);

            return result;
        }

        #endregion

        #region Country/City/Carrier Name

        public static string GetCountryName(int countryID)
        {
            string result = "";

            result = dataController.GetCountryNameFromRegistry(countryID);
            if (String.IsNullOrEmpty(result))
                result = dataController.GetCountryNameFromXML(countryID);

            return result;
        }

        public static string GetCountryNameByNumber(string number)
        {
            if (String.IsNullOrEmpty(number))
                return String.Empty;
            if (Regex.IsMatch(number, PHONE_NUMBER_PATTERN) == false)
                return String.Empty;

            string countryID = GetCountryID(number);
            if (String.IsNullOrEmpty(countryID))
                return String.Empty;

            return GetCountryName(int.Parse(countryID));
        }

        public static string GetAreaName(string areaCode)
        {
            if (String.IsNullOrEmpty(areaCode))
                return String.Empty;

            string result = "";

            result = dataController.GetCityName(areaCode);
            if (String.IsNullOrEmpty(result))
                result = dataController.GetCarrierName(areaCode);

            return result;
        }

        public static string GetAreaNameByNumber(string number)
        {
            if (String.IsNullOrEmpty(number))
                return String.Empty;
            if (Regex.IsMatch(number, PHONE_NUMBER_PATTERN) == false)
                return String.Empty;

            string areaCode = GetFullAreaCode(number);
            if (String.IsNullOrEmpty(areaCode))
                return String.Empty;

            return GetAreaName(areaCode);
        }

        #endregion
    }
}
