using System;
using System.Collections.Generic;
using System.Text;
using HomeSen.Helpers.Conversion;
using System.Xml;
using Microsoft.Win32;

namespace HomeSen.Helpers.Base
{
    internal abstract class AbstractPhoneNumberData
    {
        #region Fields

        protected static Dictionary<DISTANCE_RULE, string> distanceRuleXmlNameMap = null;
        protected bool initialized = false;

        protected const string REGISTRY_LOCATIONS = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Telephony\Locations";
        protected const string REGISTRY_COUNTRYLIST_LEGACY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Telephony\CountryList";
        protected const string REGISTRY_COUNTRYLIST_MODERN = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Telephony\Country List";

        #endregion


        #region Initialization

        public virtual void Initialize()
        {
            if (this.initialized)
                return;

            distanceRuleXmlNameMap = new Dictionary<DISTANCE_RULE, string>(3);
            distanceRuleXmlNameMap.Add(DISTANCE_RULE.SameAreaRule, "sameAreaRule");
            distanceRuleXmlNameMap.Add(DISTANCE_RULE.LongDistanceRule, "longDistanceRule");
            distanceRuleXmlNameMap.Add(DISTANCE_RULE.InternationalRule, "internationalRule");

            initialized = true;
        }

        #endregion


        #region XML Data

        /// <summary>
        /// Gets the country ID from the XML resource.
        /// </summary>
        /// <param name="areaCode">The area code.</param>
        /// <returns>A string containing the country ID for the given area code or <value>String.Empty</value>, if an error occured.</returns>
        public virtual string GetCountryIDFromXML(string areaCode)
        {
            if (String.IsNullOrEmpty(areaCode))
                return String.Empty;

            string result = "";

            XmlDocument cities = new XmlDocument();
            cities.LoadXml(Properties.Resources.Cities);
            XmlNode city = cities.SelectSingleNode(@"/cities/city[id='" + areaCode + "']");
            if (city != null)
            {
                try { result = city.SelectSingleNode("countryID").InnerText; }
                catch { result = String.Empty; }
            }

            if (String.IsNullOrEmpty(result))
            {
                XmlDocument mobiles = new XmlDocument();
                mobiles.LoadXml(Properties.Resources.Mobiles);
                XmlNode mobile = mobiles.SelectSingleNode(@"/mobiles/mobile[id='" + areaCode + "']");
                if (mobile != null)
                {
                    try { result = mobile.SelectSingleNode("countryID").InnerText; }
                    catch { result = String.Empty; }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the phone format from the XML resource.
        /// </summary>
        /// <param name="countryID">The country ID.</param>
        /// <param name="distanceRule">The distance rule.</param>
        /// <returns>The string representation of the country's selected phone number format or <value>String.Empty</value>, if an error occured.</returns>
        public virtual string GetPhoneFormatFromXML(int countryID, DISTANCE_RULE distanceRule)
        {
            if (distanceRule == DISTANCE_RULE.CANONICAL)
                return PhoneNumbers.CANONICAL_FORMAT;

            if (this.initialized == false)
                this.Initialize();

            string result = "";

            XmlDocument countries = new XmlDocument();
            countries.LoadXml(Properties.Resources.Countries);
            XmlNode country = countries.SelectSingleNode(@"/countries/country[id=" + countryID + "]");
            if (country == null)
                return String.Empty;

            try { result = country.SelectSingleNode(distanceRuleXmlNameMap[distanceRule]).InnerText; }
            catch { result = String.Empty; }

            return result;
        }

        /// <summary>
        /// Gets the country code for a given phone number fragment by 
        /// iteratively matching the fragment against valid country codes.
        /// </summary>
        /// <param name="code">The phone number fragment.</param>
        /// <returns>A string containing the country code for the given phone number fragment or an empty string, if no valid country code matches the fragment or an error occured.</returns>
        public virtual string GetCountryCode(string code)
        {
            if (String.IsNullOrEmpty(code))
                return String.Empty;

            string result = "";
            string guess = code;

            XmlDocument countries = new XmlDocument();
            countries.LoadXml(Properties.Resources.Countries);

            while (guess.Length > 0)
            {
                XmlNode country = countries.SelectSingleNode(@"/countries/country[countryCode=" + guess + "]");
                if (country != null)
                {
                    try { result = country.SelectSingleNode("countryCode").InnerText; }
                    catch { result = String.Empty; }
                    break;
                }

                guess = guess.Substring(0, guess.Length - 1);
            }

            return result;
        }

        /// <summary>
        /// Gets the area/carrier code for a given phone number fragment by 
        /// iteratively matching the fragment against valid area/carrier codes.
        /// </summary>
        /// <param name="code">The phone number fragment.</param>
        /// <returns>A string containing the area/carrier code for the given phone number fragment or an empty string, if no valid area/carrier code matches the fragment or an error occured.</returns>
        public virtual string GetAreaCode(string code)
        {
            if (String.IsNullOrEmpty(code))
                return String.Empty;

            string result = "";
            string guess = code;

            XmlDocument cities = new XmlDocument();
            cities.LoadXml(Properties.Resources.Cities);
            XmlDocument mobiles = new XmlDocument();
            mobiles.LoadXml(Properties.Resources.Mobiles);

            while (guess.Length > 0)
            {
                XmlNode node = cities.SelectSingleNode(@"/cities/city[id='" + guess + "']");
                if (node == null)
                    node = mobiles.SelectSingleNode(@"/mobiles/mobile[id='" + guess + "']");
                if (node != null)
                {
                    try { result = node.SelectSingleNode("id").InnerText; }
                    catch { result = String.Empty; }
                    break;
                }

                guess = guess.Substring(0, guess.Length - 1);
            }

            return result;
        }

        public virtual string GetCountryNameFromXML(int countryID)
        {
            string result = "";

            XmlDocument countries = new XmlDocument();
            countries.LoadXml(Properties.Resources.Countries);
            XmlNode country = countries.SelectSingleNode(@"/countries/country[id=" + countryID + "]");
            if (country == null)
                return String.Empty;

            try { result = country.SelectSingleNode("name").InnerText; }
            catch { result = String.Empty; }

            return result;
        }

        public virtual string GetCityName(string areaCode)
        {
            if (String.IsNullOrEmpty(areaCode))
                return String.Empty;

            string result = "";

            XmlDocument cities = new XmlDocument();
            cities.LoadXml(Properties.Resources.Cities);
            XmlNode city = cities.SelectSingleNode(@"/cities/city[id='" + areaCode + "']");
            if (city == null)
                return String.Empty;

            try { result = city.SelectSingleNode("name").InnerText; }
            catch { result = String.Empty; }

            return result;
        }

        public virtual string GetCarrierName(string areaCode)
        {
            if (String.IsNullOrEmpty(areaCode))
                return String.Empty;

            string result = "";

            XmlDocument mobiles = new XmlDocument();
            mobiles.LoadXml(Properties.Resources.Mobiles);
            XmlNode mobile = mobiles.SelectSingleNode(@"/mobiles/mobile[id='" + areaCode + "']");
            if (mobile == null)
                return String.Empty;

            try { result = mobile.SelectSingleNode("name").InnerText; }
            catch { result = String.Empty; }

            return result;
        }

        #endregion


        #region Registry Data

        /// <summary>
        /// Gets the user's country ID from the Registry.
        /// </summary>
        /// <returns>A string containing the user's country ID or <value>String.Empty</value>, if an error occured.</returns>
        public virtual string GetUserCountryID()
        {
            RegistryKey hklmLocations = Registry.LocalMachine.OpenSubKey(REGISTRY_LOCATIONS);
            try
            {
                object currentID = hklmLocations.GetValue("CurrentID");
                if (currentID == null)
                    return String.Empty;
                RegistryKey currentLocation = hklmLocations.OpenSubKey("Location" + currentID);
                object country = currentLocation.GetValue("Country");
                if (country == null)
                    return String.Empty;

                return country.ToString();
            }
            catch { return String.Empty; }
        }

        /// <summary>
        /// Gets the user's area code from the Registry.
        /// </summary>
        /// <returns>A string containing the user's area code or <value>String.Empty</value>, if an error occured.</returns>
        public virtual string GetUserAreaCode()
        {
            RegistryKey hklmLocations = Registry.LocalMachine.OpenSubKey(REGISTRY_LOCATIONS);
            try
            {
                object currentID = hklmLocations.GetValue("CurrentID");
                if (currentID == null)
                    return String.Empty;
                RegistryKey currentLocation = hklmLocations.OpenSubKey("Location" + currentID);
                object areaCode = currentLocation.GetValue("AreaCode");
                if (areaCode == null)
                    return String.Empty;

                return areaCode.ToString();
            }
            catch { return String.Empty; }
        }

        /// <summary>
        /// Gets the phone format from registry.
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <param name="distanceRule">The distance rule.</param>
        /// <returns>The string representation of the country's selected phone number format or <value>String.Empty</value>, if an error occured.</returns>
        public virtual string GetPhoneFormatFromRegistry(int countryCode, DISTANCE_RULE distanceRule)
        {
            if (distanceRule == DISTANCE_RULE.CANONICAL)
                return PhoneNumbers.CANONICAL_FORMAT;

            RegistryKey hklmCountry = Registry.LocalMachine.OpenSubKey(REGISTRY_COUNTRYLIST_LEGACY);
            if (hklmCountry == null)
                hklmCountry = Registry.LocalMachine.OpenSubKey(REGISTRY_COUNTRYLIST_MODERN);

            try
            {
                hklmCountry = hklmCountry.OpenSubKey(countryCode.ToString());
                object curFormat = hklmCountry.GetValue(distanceRule.ToString());
                if (curFormat != null)
                    return curFormat.ToString();
            }
            catch { }

            return String.Empty;
        }

        public virtual string GetCountryNameFromRegistry(int countryID)
        {
            string result = "";

            RegistryKey hklmCountry = Registry.LocalMachine.OpenSubKey(REGISTRY_COUNTRYLIST_LEGACY);
            if (hklmCountry == null)
                hklmCountry = Registry.LocalMachine.OpenSubKey(REGISTRY_COUNTRYLIST_MODERN);
            if (hklmCountry == null)
                return String.Empty;

            try
            {
                hklmCountry = hklmCountry.OpenSubKey(countryID.ToString());
                result = hklmCountry.GetValue("Name").ToString();
            }
            catch { }

            return result;
        }

        #endregion
    }
}
