using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using HomeSen.Helpers.Types;
using HomeSen.Helpers.Interfaces;

namespace HomeSen.Helpers.Conversion
{
    public class PhoneNumbers
    {
        #region Fields

        private IPhoneNumberDataXml xmlDataProvider = null;
        private IPhoneNumberDataRegistry registryDataProvider = null;

        #endregion


        #region Constructors

        public PhoneNumbers() : this(new PhoneNumberDataXml(), new PhoneNumberDataRegistry()) { }

        internal PhoneNumbers(IPhoneNumberDataXml xmlDataProvider) : this(xmlDataProvider, new PhoneNumberDataRegistry()) { }

        internal PhoneNumbers(IPhoneNumberDataRegistry registryDataProvider) : this(new PhoneNumberDataXml(), registryDataProvider) { }

        internal PhoneNumbers(IPhoneNumberDataXml xmlDataProvider, IPhoneNumberDataRegistry registryDataProvider)
        {
            this.xmlDataProvider = xmlDataProvider;
            this.registryDataProvider = registryDataProvider;
        }

        #endregion

        public string FormatPhoneNumber(string number, DISTANCE_RULE distanceRule)
        {
            if (String.IsNullOrEmpty(number))
                return String.Empty;
            if (Regex.IsMatch(number, PhoneNumberConstants.PHONE_NUMBER_PATTERN) == false)
                return String.Empty;

            string result = "";
            if (distanceRule == DISTANCE_RULE.CANONICAL)
                result = PhoneNumberConstants.CANONICAL_FORMAT;
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

        private string GetAreaCode(string number, string countryCode)
        {
            if (String.IsNullOrEmpty(countryCode))
                return String.Empty;
            if (String.IsNullOrEmpty(number))
                return String.Empty;
            if (Regex.IsMatch(number, PhoneNumberConstants.PHONE_NUMBER_PATTERN) == false)
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

        private string GetFullAreaCode(string number)
        {
            if (String.IsNullOrEmpty(number))
                return String.Empty;
            if (Regex.IsMatch(number, PhoneNumberConstants.PHONE_NUMBER_PATTERN) == false)
                return String.Empty;

            string countryCode = GetCountryCode(number);
            if (String.IsNullOrEmpty(countryCode))
                return String.Empty;

            return GetFullAreaCode(number, countryCode);
        }

        private string GetFullAreaCode(string number, string countryCode)
        {
            if (String.IsNullOrEmpty(number))
                return String.Empty;
            if (Regex.IsMatch(number, PhoneNumberConstants.PHONE_NUMBER_PATTERN) == false)
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

            int length = Math.Min(code.Length, 1 + PhoneNumberConstants.MAX_COUNTRYCODE_LENGTH + PhoneNumberConstants.MAX_AREACODE_LENGTH);
            code = code.Substring(0, length);

            result = xmlDataProvider.GetAreaCode(code);

            return result;
        }

        #endregion


        #region Country Data

        public string GetCountryID(string number)
        {
            if (String.IsNullOrEmpty(number))
                return String.Empty;
            if (Regex.IsMatch(number, PhoneNumberConstants.PHONE_NUMBER_PATTERN) == false)
                return String.Empty;

            string result = "";

            string areaCode = GetFullAreaCode(number);
            if (String.IsNullOrEmpty(areaCode))
                return String.Empty;

            result = xmlDataProvider.GetCountryID(areaCode);

            return result;
        }

        private string GetCountryCode(string number)
        {
            if (String.IsNullOrEmpty(number))
                return String.Empty;
            if (Regex.IsMatch(number, PhoneNumberConstants.PHONE_NUMBER_PATTERN) == false)
                return String.Empty;

            string result = "";
            string code = "";

            if (number.StartsWith("00"))
                code = number.Substring(2, PhoneNumberConstants.MAX_COUNTRYCODE_LENGTH);
            else if (number.StartsWith("+"))
                code = number.Substring(1, PhoneNumberConstants.MAX_COUNTRYCODE_LENGTH);
            else
                return GetUserCountryID();

            result = xmlDataProvider.GetCountryCode(code);

            return result;
        }

        #endregion


        private string GetLocalNumber(string number, string countryCode, string areaCode)
        {
            if (String.IsNullOrEmpty(number) || String.IsNullOrEmpty(countryCode) || String.IsNullOrEmpty(areaCode))
                return String.Empty;
            if (Regex.IsMatch(number, PhoneNumberConstants.PHONE_NUMBER_PATTERN) == false)
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

        public string GetUserCountryID()
        {
            return registryDataProvider.GetUserCountryID();
        }

        public string GetUserAreaCode()
        {
            return registryDataProvider.GetUserAreaCode();
        }

        private string GetUserPhoneFormat(DISTANCE_RULE distanceRule)
        {
            if (distanceRule == DISTANCE_RULE.CANONICAL)
                return PhoneNumberConstants.CANONICAL_FORMAT;

            string result = "";
            string id = GetUserCountryID();
            if (String.IsNullOrEmpty(id))
                return String.Empty;

            int userCountryID = int.Parse(id);

            result = registryDataProvider.GetPhoneFormat(userCountryID, distanceRule);
            if (String.IsNullOrEmpty(result))
                result = xmlDataProvider.GetPhoneFormat(userCountryID, distanceRule);

            return result;
        }

        #endregion

        #region Country/City/Carrier Name

        public string GetCountryName(int countryID)
        {
            string result = "";

            result = registryDataProvider.GetCountryName(countryID);
            if (String.IsNullOrEmpty(result))
                result = xmlDataProvider.GetCountryName(countryID);

            return result;
        }

        public string GetCountryNameByNumber(string number)
        {
            if (String.IsNullOrEmpty(number))
                return String.Empty;
            if (Regex.IsMatch(number, PhoneNumberConstants.PHONE_NUMBER_PATTERN) == false)
                return String.Empty;

            string countryID = GetCountryID(number);
            if (String.IsNullOrEmpty(countryID))
                return String.Empty;

            return GetCountryName(int.Parse(countryID));
        }

        public string GetAreaName(string areaCode)
        {
            if (String.IsNullOrEmpty(areaCode))
                return String.Empty;

            string result = "";

            result = xmlDataProvider.GetCityName(areaCode);
            if (String.IsNullOrEmpty(result))
                result = xmlDataProvider.GetCarrierName(areaCode);

            return result;
        }

        public string GetAreaNameByNumber(string number)
        {
            if (String.IsNullOrEmpty(number))
                return String.Empty;
            if (Regex.IsMatch(number, PhoneNumberConstants.PHONE_NUMBER_PATTERN) == false)
                return String.Empty;

            string areaCode = GetFullAreaCode(number);
            if (String.IsNullOrEmpty(areaCode))
                return String.Empty;

            return GetAreaName(areaCode);
        }

        #endregion
    }
}
