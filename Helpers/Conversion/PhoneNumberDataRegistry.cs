using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using HomeSen.Helpers.Types;

namespace HomeSen.Helpers.Conversion
{
    class PhoneNumberDataRegistry : Interfaces.IPhoneNumberDataRegistry
    {
        #region User Data

        /// <summary>
        /// Gets the user's country ID from the Registry.
        /// </summary>
        /// <returns>A string containing the user's country ID or <value>String.Empty</value>, if an error occured.</returns>
        public string GetUserCountryID()
        {
            RegistryKey hklmLocations = Registry.LocalMachine.OpenSubKey(PhoneNumberConstants.REGISTRY_LOCATIONS);
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
        public string GetUserAreaCode()
        {
            RegistryKey hklmLocations = Registry.LocalMachine.OpenSubKey(PhoneNumberConstants.REGISTRY_LOCATIONS);
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

        #endregion


        #region Format

        /// <summary>
        /// Gets the phone format from registry.
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <param name="distanceRule">The distance rule.</param>
        /// <returns>The string representation of the country's selected phone number format or <value>String.Empty</value>, if an error occured.</returns>
        public string GetPhoneFormat(int countryCode, DISTANCE_RULE distanceRule)
        {
            if (distanceRule == DISTANCE_RULE.CANONICAL)
                return PhoneNumberConstants.CANONICAL_FORMAT;

            RegistryKey hklmCountry = Registry.LocalMachine.OpenSubKey(PhoneNumberConstants.REGISTRY_COUNTRYLIST_LEGACY);
            if (hklmCountry == null)
                hklmCountry = Registry.LocalMachine.OpenSubKey(PhoneNumberConstants.REGISTRY_COUNTRYLIST_MODERN);

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

        #endregion


        #region Names

        public string GetCountryName(int countryID)
        {
            string result = "";

            RegistryKey hklmCountry = Registry.LocalMachine.OpenSubKey(PhoneNumberConstants.REGISTRY_COUNTRYLIST_LEGACY);
            if (hklmCountry == null)
                hklmCountry = Registry.LocalMachine.OpenSubKey(PhoneNumberConstants.REGISTRY_COUNTRYLIST_MODERN);
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
