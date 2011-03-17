using System;
using NUnit.Framework;
using HomeSen.Helpers.Types;
using Rhino.Mocks;
using Microsoft.Win32;

namespace HomeSen.Helpers.Tests.Conversion
{
    [TestFixture]
    public class PhoneNumberDataRegistryTests
    {
        [SetUp]
        public void Init()
        {
        }

        [TearDown]
        public void Cleanup()
        {
        }

        #region GetPhoneFormat Tests
        [Test]
        public void GetPhoneFormatFromRegistry_GetCanonicalFormat_ReturnsCanonicalFormat()
        {
            int countryID = 49;
            string expected = PhoneNumberConstants.CANONICAL_FORMAT;
            string actual = PhoneNumberData.GetPhoneFormatFromRegistry(countryID, DISTANCE_RULE.CANONICAL);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPhoneFormatFromRegistry_GetInternationalFormatForGermany_ResturnsGermanInternationalFormat()
        {
            int countryID = 49;
            string expected = "00EFG";
            string actual = PhoneNumberData.GetPhoneFormatFromRegistry(countryID, DISTANCE_RULE.InternationalRule);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPhoneFormatFromRegistry_GetLongDistanceFormatForGermany_ReturnsGermanLongDistanceFormat()
        {
            int countryID = 49;
            string expected = "0FG";
            string actual = PhoneNumberData.GetPhoneFormatFromRegistry(countryID, DISTANCE_RULE.LongDistanceRule);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPhoneFormatFromRegistry_GetSameAreaFormatForGermany_ReturnsGermanSameAreaFormat()
        {
            int countryID = 49;
            string expected = "G";
            string actual = PhoneNumberData.GetPhoneFormatFromRegistry(countryID, DISTANCE_RULE.SameAreaRule);

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region GetCountryName Tests
        [Test]
        public void GetCountryNameFromRegistry_InvalidCountryID_ReturnsEmptyString()
        {
            int countryID = 0;
            string actual = PhoneNumberData.GetCountryNameFromRegistry(countryID);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetCountryNameFromRegistry_GermanCountryID_ReturnsGermany()
        {
            int countryID = 49;
            // TODO: The registry contains the localized names of the countries,
            // thus the expected result needs to be translated to the correct language.
            //string expected = "Germany";
            string expected = "Deutschland";
            string actual = PhoneNumberData.GetCountryNameFromRegistry(countryID);

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region GetUserAreaCode Tests
        [Test]
        public void GetUserAreaCode_LocationSetToDresdenGermany_ReturnsAreaCodeOfDresden()
        {
            // TODO: change to default location, if test fails (ControlPanel->Telephony->Location)
            string expected = "351";
            string actual = PhoneNumberData.GetUserAreaCode();

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region GetUserCountryID Tests
        [Test]
        public void GetUserCountryID_LocationSetToDresdenGermany_Returns49()
        {
            // TODO: change to default location, if test fails (ControlPanel->Telephony->Location)
            string expected = "49";
            string actual = PhoneNumberData.GetUserCountryID();

            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
