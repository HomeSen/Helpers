using HomeSen.Helpers.Types;
using NUnit.Framework;

namespace HomeSen.Helpers.Conversion.Tests
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
        public void GetPhoneFormat_GetCanonicalFormat_ReturnsCanonicalFormat()
        {
            int countryID = 49;
            string expected = PhoneNumberConstants.CANONICAL_FORMAT;

            PhoneNumberDataRegistry registryDataProvider = new PhoneNumberDataRegistry();
            string actual = registryDataProvider.GetPhoneFormat(countryID, DISTANCE_RULE.CANONICAL);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPhoneFormat_GetInternationalFormatForGermany_ResturnsGermanInternationalFormat()
        {
            int countryID = 49;
            string expected = "00EFG";

            PhoneNumberDataRegistry registryDataProvider = new PhoneNumberDataRegistry();
            string actual = registryDataProvider.GetPhoneFormat(countryID, DISTANCE_RULE.InternationalRule);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPhoneFormat_GetLongDistanceFormatForGermany_ReturnsGermanLongDistanceFormat()
        {
            int countryID = 49;
            string expected = "0FG";

            PhoneNumberDataRegistry registryDataProvider = new PhoneNumberDataRegistry();
            string actual = registryDataProvider.GetPhoneFormat(countryID, DISTANCE_RULE.LongDistanceRule);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPhoneFormat_GetSameAreaFormatForGermany_ReturnsGermanSameAreaFormat()
        {
            int countryID = 49;
            string expected = "G";

            PhoneNumberDataRegistry registryDataProvider = new PhoneNumberDataRegistry();
            string actual = registryDataProvider.GetPhoneFormat(countryID, DISTANCE_RULE.SameAreaRule);

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region GetCountryName Tests
        [Test]
        public void GetCountryName_InvalidCountryID_ReturnsEmptyString()
        {
            int countryID = 0;

            PhoneNumberDataRegistry registryDataProvider = new PhoneNumberDataRegistry();
            string actual = registryDataProvider.GetCountryName(countryID);

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
            
            PhoneNumberDataRegistry registryDataProvider = new PhoneNumberDataRegistry();
            string actual = registryDataProvider.GetCountryName(countryID);

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region GetUserAreaCode Tests
        [Test]
        public void GetUserAreaCode_LocationSetToDresdenGermany_ReturnsAreaCodeOfDresden()
        {
            // TODO: change to default location, if test fails (ControlPanel->Telephony->Location)
            string expected = "351";

            PhoneNumberDataRegistry registryDataProvider = new PhoneNumberDataRegistry();
            string actual = registryDataProvider.GetUserAreaCode();

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region GetUserCountryID Tests
        [Test]
        public void GetUserCountryID_LocationSetToDresdenGermany_Returns49()
        {
            // TODO: change to default location, if test fails (ControlPanel->Telephony->Location)
            string expected = "49";

            PhoneNumberDataRegistry registryDataProvider = new PhoneNumberDataRegistry();
            string actual = registryDataProvider.GetUserCountryID();

            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
