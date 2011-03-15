using System;
using NUnit.Framework;

namespace HomeSen.Helpers.Conversion.Tests
{
    [TestFixture]
    public class PhoneNumbersTests
    {

        [SetUp]
        public void Init()
        {
        }

        [TearDown]
        public void Cleanup()
        {
        }

        #region GetCountryID Tests
        [Test]
        public void GetCountryID_EmptyNumber_ReturnsEmptyString()
        {
            string number = String.Empty;
            string actual = PhoneNumbers.GetCountryID(number);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetCountryID_InvalidNumber_ReturnsEmptyString()
        {
            string number = "abcd";
            string actual = PhoneNumbers.GetCountryID(number);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetCountryID_PhoneNumberInDresdenGermany_Returns49()
        {
            string number = "+493511234567";
            string expected = "49";
            string actual = PhoneNumbers.GetCountryID(number);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetCountryID_PhoneNumberOfTMobileGermany_Returns49()
        {
            string number = "+491511123456";
            string expected = "49";
            string actual = PhoneNumbers.GetCountryID(number);

            Assert.AreEqual(expected, actual);
        } 
        #endregion

        #region FormatPhoneNumber Tests
        [Test]
        public void FormatPhoneNumber_EmptyNumber_ReturnsEmptyString()
        {
            string number = String.Empty;
            string actual = PhoneNumbers.FormatPhoneNumber(number, DISTANCE_RULE.CANONICAL);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void FormatPhoneNumber_InvalidNumber_ReturnsEmptyString()
        {
            string number = "abcd";
            string actual = PhoneNumbers.FormatPhoneNumber(number, DISTANCE_RULE.CANONICAL);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void FormatPhoneNumber_PhoneNumberInDresdenGermanyCanonical_ReturnsCanonicalNumber()
        {
            string number = "+493511234567";
            string expected = "+49 (351) 1234567";
            string actual = PhoneNumbers.FormatPhoneNumber(number, DISTANCE_RULE.CANONICAL);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormatPhoneNumber_PhoneNumberInGermanInternationalFormatToCanonical_ReturnsCanonicalNumber()
        {
            string number = "00493511234567";
            string expected = "+49 (351) 1234567";
            string actual = PhoneNumbers.FormatPhoneNumber(number, DISTANCE_RULE.CANONICAL);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormatPhoneNumber_PhoneNumberInGermanLongDistanceFormatToCanonical_ReturnsCanonicalNumber()
        {
            // TODO: adjust country code or default location, if test fails (ControlPanel->Telephony->Location)
            string number = "03511234567";
            string expected = "+49 (351) 1234567";
            string actual = PhoneNumbers.FormatPhoneNumber(number, DISTANCE_RULE.CANONICAL);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormatPhoneNumber_PhoneNumberInGermanSameAreaFormatToCanonical_ReturnsCanonicalNumber()
        {
            // TODO: adjust country and area code or default location, if test fails (ControlPanel->Telephony->Location)
            string number = "1234567";
            string expected = "+49 (351) 1234567";
            string actual = PhoneNumbers.FormatPhoneNumber(number, DISTANCE_RULE.CANONICAL);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormatPhoneNumber_PhoneNumberInDresdenGermanySameArea_ReturnsSameAreaNumber()
        {
            string number = "+493511234567";
            string expected = "1234567";
            string actual = PhoneNumbers.FormatPhoneNumber(number, DISTANCE_RULE.SameAreaRule);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormatPhoneNumber_PhoneNumberInDresdenGermanyLongDistance_ReturnsLongDistanceNumber()
        {
            string number = "+493511234567";
            string expected = "03511234567";
            string actual = PhoneNumbers.FormatPhoneNumber(number, DISTANCE_RULE.LongDistanceRule);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormatPhoneNumber_PhoneNumberInDresdenGermanyInternational_ReturnsInternationalNumber()
        {
            string number = "+493511234567";
            string expected = "00493511234567";
            string actual = PhoneNumbers.FormatPhoneNumber(number, DISTANCE_RULE.InternationalRule);

            Assert.AreEqual(expected, actual);
        } 
        #endregion

        #region GetCountryName Tests
        [Test]
        public void GetCountryName_InvalidCountryID_ReturnsEmptyString()
        {
            int countryID = 0;
            string actual = PhoneNumbers.GetCountryName(countryID);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetCountryName_GermanCountryID_ReturnsGermany()
        {
            int countryID = 49;
            // TODO: The registry contains the localized names of the countries,
            // thus the expected result needs to be translated to the correct language.
            //string expected = "Germany";
            string expected = "Deutschland";
            string actual = PhoneNumbers.GetCountryName(countryID);

            Assert.AreEqual(expected, actual);
        } 
        #endregion

        #region GetCountryNameByNumber Tests
        [Test]
        public void GetCountryNameByNumber_EmptyNumber_ReturnsEmptyString()
        {
            string number = String.Empty;
            string actual = PhoneNumbers.GetCountryNameByNumber(number);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetCountryNameByNumber_InvalidNumber_ReturnsEmptyString()
        {
            string number = "abcd";
            string actual = PhoneNumbers.GetCountryNameByNumber(number);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetCountryNameByNumber_PhoneNumberInDresdenGermany_ReturnsGermany()
        {
            string number = "+493511234567";
            // TODO: The registry contains the localized names of the countries,
            // thus the expected result needs to be translated to the correct language.
            //string expected = "Germany";
            string expected = "Deutschland";
            string actual = PhoneNumbers.GetCountryNameByNumber(number);

            Assert.AreEqual(expected, actual);
        } 
        #endregion

        #region GetAreaName Tests
        [Test]
        public void GetAreaName_EmptyAreaCode_ReturnsEmptyString()
        {
            string areaCode = String.Empty;
            string actual = PhoneNumbers.GetAreaName(areaCode);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetAreaName_InvalidAreaCode_ReturnsEmptyString()
        {
            string areaCode = "123456";
            string actual = PhoneNumbers.GetAreaName(areaCode);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetAreaName_AreaCodeOfDresdenGermany_ReturnsDresden()
        {
            string areaCode = "+49351";
            string expected = "Dresden";
            string actual = PhoneNumbers.GetAreaName(areaCode);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetAreaName_AreaCodeOfTMobileGermany_ReturnsTMobile()
        {
            string areaCode = "+491511";
            string expected = "T-Mobile (D1)";
            string actual = PhoneNumbers.GetAreaName(areaCode);

            Assert.AreEqual(expected, actual);
        } 
        #endregion

        #region GetAreaNameByNumber Tests
        [Test]
        public void GetAreaNameByNumber_EmptyNumber_ReturnsEmptyString()
        {
            string number = String.Empty;
            string actual = PhoneNumbers.GetAreaNameByNumber(number);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetAreaNameByNumber_InvalidNumber_ReturnsEmptyString()
        {
            string number = "abcd";
            string actual = PhoneNumbers.GetAreaNameByNumber(number);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetAreaNameByNumber_PhoneNumberInDresdenGermany_ReturnsDresden()
        {
            string number = "+493511234567";
            string expected = "Dresden";
            string actual = PhoneNumbers.GetAreaNameByNumber(number);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetAreaNameByNumber_PhoneNumberOfTMobileGermany_ReturnsTMobile()
        {
            string number = "+491511123456";
            string expected = "T-Mobile (D1)";
            string actual = PhoneNumbers.GetAreaNameByNumber(number);

            Assert.AreEqual(expected, actual);
        } 
        #endregion

        #region GetUserAreaCode Tests
        [Test]
        public void GetUserAreaCode_LocationSetToDresdenGermany_ReturnsAreaCodeOfDresden()
        {
            // TODO: change to default location, if test fails (ControlPanel->Telephony->Location)
            string expected = "351";
            string actual = PhoneNumbers.GetUserAreaCode();

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region GetUserCountryID Tests
        [Test]
        public void GetUserCountryID_LocationSetToDresdenGermany_Returns49()
        {
            // TODO: change to default location, if test fails (ControlPanel->Telephony->Location)
            string expected = "49";
            string actual = PhoneNumbers.GetUserCountryID();

            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
