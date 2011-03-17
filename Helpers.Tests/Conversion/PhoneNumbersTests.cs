using System;
using NUnit.Framework;
using HomeSen.Helpers.Types;
using HomeSen.Helpers.Interfaces;
using Rhino.Mocks;

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
            PhoneNumbers phoneNumberConverter = new PhoneNumbers();
            string actual = phoneNumberConverter.GetCountryID(number);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetCountryID_InvalidNumber_ReturnsEmptyString()
        {
            string number = "abcd";
            PhoneNumbers phoneNumberConverter = new PhoneNumbers();
            string actual = phoneNumberConverter.GetCountryID(number);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetCountryID_PhoneNumberInDresdenGermany_Returns49()
        {
            MockRepository mocks = new MockRepository();
            IPhoneNumberDataXml xmlDataProvider = mocks.Stub<IPhoneNumberDataXml>();
            IPhoneNumberDataRegistry registryDataProvider = mocks.Stub<IPhoneNumberDataRegistry>();

            string number = "+493511234567";
            string expected = "49";

            using (mocks.Record())
            {
                xmlDataProvider.GetCountryCode("4935");
                LastCall.Return("49");
                xmlDataProvider.GetAreaCode("+493511234567");
                LastCall.Return("+49351");
                xmlDataProvider.GetCountryID("+49351");
                LastCall.Return(expected);
            }

            PhoneNumbers phoneNumberConverter = new PhoneNumbers(xmlDataProvider, registryDataProvider);
            string actual = phoneNumberConverter.GetCountryID(number);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetCountryID_PhoneNumberOfTMobileGermany_Returns49()
        {
            MockRepository mocks = new MockRepository();
            IPhoneNumberDataXml xmlDataProvider = mocks.Stub<IPhoneNumberDataXml>();
            IPhoneNumberDataRegistry registryDataProvider = mocks.Stub<IPhoneNumberDataRegistry>();

            string number = "+491511123456";
            string expected = "49";

            using (mocks.Record())
            {
                xmlDataProvider.GetCountryCode("4915");
                LastCall.Return("49");
                xmlDataProvider.GetAreaCode("+491511123456");
                LastCall.Return("+491511");
                xmlDataProvider.GetCountryID("+491511");
                LastCall.Return(expected);
            }

            PhoneNumbers phoneNumberConverter = new PhoneNumbers(xmlDataProvider, registryDataProvider);
            string actual = phoneNumberConverter.GetCountryID(number);

            Assert.AreEqual(expected, actual);
        } 
        #endregion

        #region FormatPhoneNumber Tests
        [Test]
        public void FormatPhoneNumber_EmptyNumber_ReturnsEmptyString()
        {
            string number = String.Empty;
            PhoneNumbers phoneNumberConverter = new PhoneNumbers();
            string actual = phoneNumberConverter.FormatPhoneNumber(number, DISTANCE_RULE.CANONICAL);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void FormatPhoneNumber_InvalidNumber_ReturnsEmptyString()
        {
            string number = "abcd";
            PhoneNumbers phoneNumberConverter = new PhoneNumbers();
            string actual = phoneNumberConverter.FormatPhoneNumber(number, DISTANCE_RULE.CANONICAL);

            Assert.IsEmpty(actual);
        }

        [Test, Sequential]
        public void FormatPhoneNumber_PhoneNumberInDresdenGermany_ReturnsFormattedNumber(
            [Values(DISTANCE_RULE.CANONICAL, DISTANCE_RULE.InternationalRule, DISTANCE_RULE.LongDistanceRule,
                DISTANCE_RULE.SameAreaRule)] DISTANCE_RULE distanceRule,
            [Values("+49 (351) 1234567", "00493511234567", "03511234567", "1234567")] string expected)
        {
            MockRepository mocks = new MockRepository();
            IPhoneNumberDataXml xmlDataProvider = mocks.Stub<IPhoneNumberDataXml>();
            IPhoneNumberDataRegistry registryDataProvider = mocks.Stub<IPhoneNumberDataRegistry>();

            string number = "+493511234567";

            using (mocks.Record())
            {
                registryDataProvider.GetUserCountryID();
                LastCall.Return("49");
                registryDataProvider.GetUserAreaCode();
                LastCall.Return("351");
                registryDataProvider.GetPhoneFormat(49, DISTANCE_RULE.InternationalRule);
                LastCall.Return("00EFG");
                registryDataProvider.GetPhoneFormat(49, DISTANCE_RULE.LongDistanceRule);
                LastCall.Return("0FG");
                registryDataProvider.GetPhoneFormat(49, DISTANCE_RULE.SameAreaRule);
                LastCall.Return("G");

                xmlDataProvider.GetCountryCode("4935");
                LastCall.Return("49");
                xmlDataProvider.GetAreaCode("+493511234567");
                LastCall.Return("+49351");
            }

            PhoneNumbers phoneNumberConverter = new PhoneNumbers(xmlDataProvider, registryDataProvider);
            string actual = phoneNumberConverter.FormatPhoneNumber(number, distanceRule);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormatPhoneNumber_PhoneNumberInGermanInternationalFormatToCanonical_ReturnsCanonicalNumber()
        {
            MockRepository mocks = new MockRepository();
            IPhoneNumberDataXml xmlDataProvider = mocks.Stub<IPhoneNumberDataXml>();
            IPhoneNumberDataRegistry registryDataProvider = mocks.Stub<IPhoneNumberDataRegistry>();

            string number = "00493511234567";
            string expected = "+49 (351) 1234567";

            using (mocks.Record())
            {
                xmlDataProvider.GetCountryCode("4935");
                LastCall.Return("49");
                xmlDataProvider.GetAreaCode("+493511234567");
                LastCall.Return("+49351");
            }

            PhoneNumbers phoneNumberConverter = new PhoneNumbers(xmlDataProvider, registryDataProvider);
            string actual = phoneNumberConverter.FormatPhoneNumber(number, DISTANCE_RULE.CANONICAL);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormatPhoneNumber_PhoneNumberInGermanLongDistanceFormatToCanonical_ReturnsCanonicalNumber()
        {
            MockRepository mocks = new MockRepository();
            IPhoneNumberDataXml xmlDataProvider = mocks.Stub<IPhoneNumberDataXml>();
            IPhoneNumberDataRegistry registryDataProvider = mocks.Stub<IPhoneNumberDataRegistry>();

            string number = "03511234567";
            string expected = "+49 (351) 1234567";

            using (mocks.Record())
            {
                registryDataProvider.GetUserCountryID();
                LastCall.Return("49");
                xmlDataProvider.GetAreaCode("+493511234567");
                LastCall.Return("+49351");
            }

            PhoneNumbers phoneNumberConverter = new PhoneNumbers(xmlDataProvider, registryDataProvider);
            string actual = phoneNumberConverter.FormatPhoneNumber(number, DISTANCE_RULE.CANONICAL);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormatPhoneNumber_PhoneNumberInGermanSameAreaFormatToCanonical_ReturnsCanonicalNumber()
        {
            MockRepository mocks = new MockRepository();
            IPhoneNumberDataXml xmlDataProvider = mocks.Stub<IPhoneNumberDataXml>();
            IPhoneNumberDataRegistry registryDataProvider = mocks.Stub<IPhoneNumberDataRegistry>();

            string number = "1234567";
            string expected = "+49 (351) 1234567";

            using (mocks.Record())
            {
                registryDataProvider.GetUserCountryID();
                LastCall.Return("49");
                registryDataProvider.GetUserAreaCode();
                LastCall.Return("351");
                xmlDataProvider.GetAreaCode("+493511234567");
                LastCall.Return("+49351");
            }

            PhoneNumbers phoneNumberConverter = new PhoneNumbers(xmlDataProvider, registryDataProvider);
            string actual = phoneNumberConverter.FormatPhoneNumber(number, DISTANCE_RULE.CANONICAL);

            Assert.AreEqual(expected, actual);
        } 
        #endregion

        #region GetCountryName Tests
        [Test]
        public void GetCountryName_InvalidCountryID_ReturnsEmptyString()
        {
            int countryID = 0;
            PhoneNumbers phoneNumberConverter = new PhoneNumbers();
            string actual = phoneNumberConverter.GetCountryName(countryID);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetCountryName_GermanCountryID_ReturnsGermany()
        {
            MockRepository mocks = new MockRepository();
            IPhoneNumberDataRegistry registryDataProvider = mocks.Stub<IPhoneNumberDataRegistry>();

            int countryID = 49;
            string expected = "Deutschland";

            using (mocks.Record())
            {
                registryDataProvider.GetCountryName(countryID);
                LastCall.Return(expected);
            }

            PhoneNumbers phoneNumberConverter = new PhoneNumbers(registryDataProvider);
            string actual = phoneNumberConverter.GetCountryName(countryID);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetCountryName_GermanCountryIDWithoutRegistryAccess_GetsCountryNameFromXML()
        {
            MockRepository mocks = new MockRepository();
            IPhoneNumberDataXml xmlDataProvider = mocks.StrictMock<IPhoneNumberDataXml>();
            IPhoneNumberDataRegistry registryDataProvider = mocks.Stub<IPhoneNumberDataRegistry>();

            int countryID = 49;

            using (mocks.Record())
            {
                // expect that GetCountryName is called
                xmlDataProvider.GetCountryName(countryID);
                LastCall.Return("Germany");
            }

            PhoneNumbers phoneNumberConverter = new PhoneNumbers(xmlDataProvider, registryDataProvider);
            phoneNumberConverter.GetCountryName(countryID);

            mocks.VerifyAll();
        }
        #endregion

        #region GetCountryNameByNumber Tests
        [Test]
        public void GetCountryNameByNumber_EmptyNumber_ReturnsEmptyString()
        {
            string number = String.Empty;
            PhoneNumbers phoneNumberConverter = new PhoneNumbers();
            string actual = phoneNumberConverter.GetCountryNameByNumber(number);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetCountryNameByNumber_InvalidNumber_ReturnsEmptyString()
        {
            string number = "abcd";
            PhoneNumbers phoneNumberConverter = new PhoneNumbers();
            string actual = phoneNumberConverter.GetCountryNameByNumber(number);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetCountryNameByNumber_PhoneNumberInDresdenGermany_ReturnsGermany()
        {
            MockRepository mocks = new MockRepository();
            IPhoneNumberDataXml xmlDataProvider = mocks.Stub<IPhoneNumberDataXml>();
            IPhoneNumberDataRegistry registryDataProvider = mocks.Stub<IPhoneNumberDataRegistry>();

            string number = "+493511234567";
            string expected = "Deutschland";

            using (mocks.Record())
            {
                xmlDataProvider.GetCountryCode("4935");
                LastCall.Return("49");
                xmlDataProvider.GetAreaCode("+493511234567");
                LastCall.Return("+49351");
                xmlDataProvider.GetCountryID("+49351");
                LastCall.Return("49");

                registryDataProvider.GetCountryName(49);
                LastCall.Return(expected);
            }

            PhoneNumbers phoneNumberConverter = new PhoneNumbers(xmlDataProvider, registryDataProvider);
            string actual = phoneNumberConverter.GetCountryNameByNumber(number);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetCountryNameByNumber_PhoneNumberInDresdenGermanyWithoutRegistryAccess_GetsCountryNameFromXML()
        {
            MockRepository mocks = new MockRepository();
            IPhoneNumberDataXml xmlDataProvider = mocks.StrictMock<IPhoneNumberDataXml>();
            IPhoneNumberDataRegistry registryDataProvider = mocks.Stub<IPhoneNumberDataRegistry>();

            string number = "+493511234567";

            using (mocks.Record())
            {
                xmlDataProvider.GetCountryCode("4935");
                LastCall.Return("49");
                xmlDataProvider.GetAreaCode("+493511234567");
                LastCall.Return("+49351");
                xmlDataProvider.GetCountryID("+49351");
                LastCall.Return("49");
                xmlDataProvider.GetCountryName(49);
                LastCall.Return("Germany");
            }

            PhoneNumbers phoneNumberConverter = new PhoneNumbers(xmlDataProvider, registryDataProvider);
            phoneNumberConverter.GetCountryNameByNumber(number);

            mocks.VerifyAll();
        }
        #endregion

        #region GetAreaName Tests
        [Test]
        public void GetAreaName_EmptyAreaCode_ReturnsEmptyString()
        {
            string areaCode = String.Empty;
            PhoneNumbers phoneNumberConverter = new PhoneNumbers();
            string actual = phoneNumberConverter.GetAreaName(areaCode);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetAreaName_InvalidAreaCode_ReturnsEmptyString()
        {
            string areaCode = "123456";
            PhoneNumbers phoneNumberConverter = new PhoneNumbers();
            string actual = phoneNumberConverter.GetAreaName(areaCode);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetAreaName_AreaCodeOfDresdenGermany_ReturnsDresden()
        {
            MockRepository mocks = new MockRepository();
            IPhoneNumberDataXml xmlDataProvider = mocks.Stub<IPhoneNumberDataXml>();
            IPhoneNumberDataRegistry registryDataProvider = mocks.Stub<IPhoneNumberDataRegistry>();

            string areaCode = "+49351";
            string expected = "Dresden";

            using (mocks.Record())
            {
                xmlDataProvider.GetCityName("+49351");
                LastCall.Return("Dresden");
            }

            PhoneNumbers phoneNumberConverter = new PhoneNumbers(xmlDataProvider, registryDataProvider);
            string actual = phoneNumberConverter.GetAreaName(areaCode);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetAreaName_AreaCodeOfTMobileGermany_ReturnsTMobile()
        {
            MockRepository mocks = new MockRepository();
            IPhoneNumberDataXml xmlDataProvider = mocks.Stub<IPhoneNumberDataXml>();
            IPhoneNumberDataRegistry registryDataProvider = mocks.Stub<IPhoneNumberDataRegistry>();

            string areaCode = "+491511";
            string expected = "T-Mobile (D1)";
            
            using (mocks.Record())
            {
                xmlDataProvider.GetCarrierName("+491511");
                LastCall.Return("T-Mobile (D1)");
            }

            PhoneNumbers phoneNumberConverter = new PhoneNumbers(xmlDataProvider, registryDataProvider);
            string actual = phoneNumberConverter.GetAreaName(areaCode);

            Assert.AreEqual(expected, actual);
        } 
        #endregion

        #region GetAreaNameByNumber Tests
        [Test]
        public void GetAreaNameByNumber_EmptyNumber_ReturnsEmptyString()
        {
            string number = String.Empty;
            PhoneNumbers phoneNumberConverter = new PhoneNumbers();
            string actual = phoneNumberConverter.GetAreaNameByNumber(number);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetAreaNameByNumber_InvalidNumber_ReturnsEmptyString()
        {
            string number = "abcd";
            PhoneNumbers phoneNumberConverter = new PhoneNumbers();
            string actual = phoneNumberConverter.GetAreaNameByNumber(number);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetAreaNameByNumber_PhoneNumberInDresdenGermany_ReturnsDresden()
        {
            MockRepository mocks = new MockRepository();
            IPhoneNumberDataXml xmlDataProvider = mocks.Stub<IPhoneNumberDataXml>();
            IPhoneNumberDataRegistry registryDataProvider = mocks.Stub<IPhoneNumberDataRegistry>();

            string number = "+493511234567";
            string expected = "Dresden";

            using (mocks.Record())
            {
                xmlDataProvider.GetCountryCode("4935");
                LastCall.Return("49");
                xmlDataProvider.GetAreaCode("+493511234567");
                LastCall.Return("+49351");
                xmlDataProvider.GetCityName("+49351");
                LastCall.Return("Dresden");
            }

            PhoneNumbers phoneNumberConverter = new PhoneNumbers(xmlDataProvider, registryDataProvider);
            string actual = phoneNumberConverter.GetAreaNameByNumber(number);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetAreaNameByNumber_PhoneNumberOfTMobileGermany_ReturnsTMobile()
        {
            MockRepository mocks = new MockRepository();
            IPhoneNumberDataXml xmlDataProvider = mocks.Stub<IPhoneNumberDataXml>();
            IPhoneNumberDataRegistry registryDataProvider = mocks.Stub<IPhoneNumberDataRegistry>();

            string number = "+491511123456";
            string expected = "T-Mobile (D1)";

            using (mocks.Record())
            {
                xmlDataProvider.GetCountryCode("4915");
                LastCall.Return("49");
                xmlDataProvider.GetAreaCode("+491511123456");
                LastCall.Return("+491511");
                xmlDataProvider.GetCityName("+491511");
                LastCall.Return("T-Mobile (D1)");
            }

            PhoneNumbers phoneNumberConverter = new PhoneNumbers(xmlDataProvider, registryDataProvider);
            string actual = phoneNumberConverter.GetAreaNameByNumber(number);

            Assert.AreEqual(expected, actual);
        } 
        #endregion

        #region GetUserAreaCode Tests
        [Test]
        public void GetUserAreaCode_LocationSetToDresdenGermany_ReturnsAreaCodeOfDresden()
        {
            MockRepository mocks = new MockRepository();
            IPhoneNumberDataXml xmlDataProvider = mocks.Stub<IPhoneNumberDataXml>();
            IPhoneNumberDataRegistry registryDataProvider = mocks.Stub<IPhoneNumberDataRegistry>();

            string expected = "351";

            using (mocks.Record())
            {
                registryDataProvider.GetUserAreaCode();
                LastCall.Return("351");
            }

            PhoneNumbers phoneNumberConverter = new PhoneNumbers(xmlDataProvider, registryDataProvider);
            string actual = phoneNumberConverter.GetUserAreaCode();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetUserAreaCode_WithoutRegistryAccess_ReturnsEmptyString()
        {
            MockRepository mocks = new MockRepository();
            IPhoneNumberDataXml xmlDataProvider = mocks.Stub<IPhoneNumberDataXml>();
            IPhoneNumberDataRegistry registryDataProvider = mocks.Stub<IPhoneNumberDataRegistry>();

            PhoneNumbers phoneNumberConverter = new PhoneNumbers(xmlDataProvider, registryDataProvider);
            string actual = phoneNumberConverter.GetUserAreaCode();

            Assert.IsNullOrEmpty(actual); ;
        }
        #endregion

        #region GetUserCountryID Tests
        [Test]
        public void GetUserCountryID_LocationSetToDresdenGermany_Returns49()
        {
            MockRepository mocks = new MockRepository();
            IPhoneNumberDataXml xmlDataProvider = mocks.Stub<IPhoneNumberDataXml>();
            IPhoneNumberDataRegistry registryDataProvider = mocks.Stub<IPhoneNumberDataRegistry>();

            string expected = "49";

            using (mocks.Record())
            {
                registryDataProvider.GetUserCountryID();
                LastCall.Return("49");
            }

            PhoneNumbers phoneNumberConverter = new PhoneNumbers(xmlDataProvider, registryDataProvider);
            string actual = phoneNumberConverter.GetUserCountryID();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetUserCountryID_WithoutRegistryAccess_ReturnsEmptyString()
        {
            MockRepository mocks = new MockRepository();
            IPhoneNumberDataXml xmlDataProvider = mocks.Stub<IPhoneNumberDataXml>();
            IPhoneNumberDataRegistry registryDataProvider = mocks.Stub<IPhoneNumberDataRegistry>();

            PhoneNumbers phoneNumberConverter = new PhoneNumbers(xmlDataProvider, registryDataProvider);
            string actual = phoneNumberConverter.GetUserCountryID();

            Assert.IsNullOrEmpty(actual); ;
        }
        #endregion
    }
}
