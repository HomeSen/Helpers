using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSen.Helpers.Types
{
    internal class PhoneNumberConstants
    {
        internal const string REGISTRY_LOCATIONS = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Telephony\Locations";
        internal const string REGISTRY_COUNTRYLIST_LEGACY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Telephony\CountryList";
        internal const string REGISTRY_COUNTRYLIST_MODERN = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Telephony\Country List";

        /// <summary>
        /// String representation of the canonical phone number format.
        /// </summary>
        public static string CANONICAL_FORMAT = "+E (F) G";

        /// <summary>
        /// String representation of the RegEx-pattern for valid phone numbers.
        /// </summary>
        public static string PHONE_NUMBER_PATTERN = @"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$";

        internal const int MAX_COUNTRYCODE_LENGTH = 4;
        internal const int MAX_AREACODE_LENGTH = 8;
    }
}
