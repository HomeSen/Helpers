using System;
using NUnit.Framework;
using HomeSen.Helpers.Types;

namespace HomeSen.Helpers.Conversion.Tests
{
    [TestFixture]
    public class PhoneNumberDataXmlTests
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
        public void GetPhoneFormatFromXML_GetCanonicalFormat_ReturnsCanonicalFormat()
        {
            int countryID = 49;
            string expected = PhoneNumberConstants.CANONICAL_FORMAT;

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetPhoneFormat(countryID, DISTANCE_RULE.CANONICAL);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPhoneFormatFromXML_GetInternationalFormatForGermany_ResturnsGermanInternationalFormat()
        {
            int countryID = 49;
            string expected = "00EFG";

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetPhoneFormat(countryID, DISTANCE_RULE.InternationalRule);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPhoneFormatFromXML_GetLongDistanceFormatForGermany_ReturnsGermanLongDistanceFormat()
        {
            int countryID = 49;
            string expected = "0FG";

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetPhoneFormat(countryID, DISTANCE_RULE.LongDistanceRule);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPhoneFormatFromXML_GetSameAreaFormatForGermany_ReturnsGermanSameAreaFormat()
        {
            int countryID = 49;
            string expected = "G";

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetPhoneFormat(countryID, DISTANCE_RULE.SameAreaRule);

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region GetCountryID Tests
        [Test]
        public void GetCountryIDFromXML_EmptyAreaCode_ResturnsEmptyString()
        {
            string areaCode = "";

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetCountryID(areaCode);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetCountryIDFromXML_AreaCodeOfDresdenGermany_Returns49()
        {
            string areaCode = "+49351";
            string expected = "49";

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetCountryID(areaCode);

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region GetCountryCode Tests
        [Test]
        public void GetCountryCode_EmptyString_ReturnsEmptyString()
        {
            string code = "";

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetCountryCode(code);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetCountryCode_InvalidCode_ReturnsEmptyString()
        {
            string code = "000";

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetCountryCode(code);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetCountryCode_FragmentOfGermanNumber_Returns49()
        {
            string code = "49351";
            string expected = "49";

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetCountryCode(code);

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region GetCountryName Tests
        [Test]
        public void GetCountryNameFromXML_InvalidCountryID_ResturnsEmptyString()
        {
            int countryID = 0;

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetCountryName(countryID);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetCountryNameFromXML_GermanCountryID_ReturnsGermany()
        {
            int countryID = 49;
            string expected = "Germany";

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetCountryName(countryID);

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region GetCityName Tests
        [Test]
        public void GetCityName_EmptyAreaCode_ReturnsEmptyString()
        {
            string areaCode = String.Empty;

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetCityName(areaCode);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetCityName_InvalidAreaCode_ReturnsEmptyString()
        {
            string areaCode = "123456";

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetCityName(areaCode);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetCityName_AreaCodeOfDresdenGermany_ReturnsDresden()
        {
            string areaCode = "+49351";
            string expected = "Dresden";

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetCityName(areaCode);

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region GetCarrierName Tests
        [Test]
        public void GetCarrierName_EmptyAreaCode_ReturnsEmptyString()
        {
            string areaCode = String.Empty;

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetCarrierName(areaCode);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetCarrierName_InvalidAreaCode_ReturnsEmptyString()
        {
            string areaCode = "123456";

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetCarrierName(areaCode);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetCarrierName_AreaCodeOfTMobileGermany_ReturnsTMobileGermany()
        {
            string areaCode = "+491511";
            string expected = "T-Mobile (D1)";

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetCarrierName(areaCode);

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region GetAreaCode Tests
        [Test]
        public void GetAreaCode_EmptyCode_ReturnsEmptyString()
        {
            string code = String.Empty;

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetAreaCode(code);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetAreaCode_InvalidCode_ReturnsEmptyString()
        {
            string code = "abcd";

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetAreaCode(code);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetAreaCode_FragmentOfGermanPhoneNumberFromDresden_ResturnsAreaCodeForDresdenGermany()
        {
            string code = "+4935126";
            string expected = "+49351";

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetAreaCode(code);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetAreaCode_FragmentOfGermanMobileNumberFromTMobie_ReturnsAreaCodeForTMobileGermany()
        {
            string code = "+49151124";
            string expected = "+491511";

            PhoneNumberDataXml xmlDataProvider = new PhoneNumberDataXml();
            string actual = xmlDataProvider.GetAreaCode(code);

            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
