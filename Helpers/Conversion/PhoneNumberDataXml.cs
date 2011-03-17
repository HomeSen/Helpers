using System;
using System.Collections.Generic;
using System.Text;
using HomeSen.Helpers.Types;
using System.Xml;

namespace HomeSen.Helpers.Conversion
{
    class PhoneNumberDataXml : Interfaces.IPhoneNumberDataXml
    {
        #region Fields

        private Dictionary<DISTANCE_RULE, string> distanceRuleXmlNameMap = null;
        private bool initialized = false;

        #endregion


        #region Initialization

        private void Initialize()
        {
            if (initialized)
                return;

            distanceRuleXmlNameMap = new Dictionary<DISTANCE_RULE, string>(3);
            distanceRuleXmlNameMap.Add(DISTANCE_RULE.SameAreaRule, "sameAreaRule");
            distanceRuleXmlNameMap.Add(DISTANCE_RULE.LongDistanceRule, "longDistanceRule");
            distanceRuleXmlNameMap.Add(DISTANCE_RULE.InternationalRule, "internationalRule");

            initialized = true;
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneNumberDataXml"/> class.
        /// </summary>
        public PhoneNumberDataXml()
        {
            Initialize();
        }

        #endregion


        #region IDs / Codes

        /// <summary>
        /// Gets the country ID from the XML resource.
        /// </summary>
        /// <param name="areaCode">The area code.</param>
        /// <returns>A string containing the country ID for the given area code or <value>String.Empty</value>, if an error occured.</returns>
        public string GetCountryID(string areaCode)
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
        /// Gets the country code for a given phone number fragment by 
        /// iteratively matching the fragment against valid country codes.
        /// </summary>
        /// <param name="code">The phone number fragment.</param>
        /// <returns>A string containing the country code for the given phone number fragment or an empty string, if no valid country code matches the fragment or an error occured.</returns>
        public string GetCountryCode(string code)
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
        public string GetAreaCode(string code)
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

        #endregion


        #region Formats

        /// <summary>
        /// Gets the phone format from the XML resource.
        /// </summary>
        /// <param name="countryID">The country ID.</param>
        /// <param name="distanceRule">The distance rule.</param>
        /// <returns>The string representation of the country's selected phone number format or <value>String.Empty</value>, if an error occured.</returns>
        public string GetPhoneFormat(int countryID, DISTANCE_RULE distanceRule)
        {
            if (distanceRule == DISTANCE_RULE.CANONICAL)
                return PhoneNumberConstants.CANONICAL_FORMAT;

            if (initialized == false)
                Initialize();

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

        #endregion


        #region Names

        /// <summary>
        /// Gets the name of the country for the given ID.
        /// </summary>
        /// <param name="countryID">The country ID.</param>
        /// <returns>A string containing the name of the country for the given ID or <value>String.Empty</value>, if an error occured.</returns>
        public string GetCountryName(int countryID)
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

        /// <summary>
        /// Gets the name of the city for the given area code.
        /// </summary>
        /// <param name="areaCode">The area code.</param>
        /// <returns>A string containing the name of the city for the given area code or <value>String.Empty</value>, if an error occured.</returns>
        public string GetCityName(string areaCode)
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

        /// <summary>
        /// Gets the name of the carrier for the given area code.
        /// </summary>
        /// <param name="areaCode">The area code.</param>
        /// <returns>A string containing the name of the carrier for the given area code or <value>String.Empty</value>, if an error occured.</returns>
        public string GetCarrierName(string areaCode)
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
    }
}
