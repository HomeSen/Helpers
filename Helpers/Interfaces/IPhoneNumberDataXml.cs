
namespace HomeSen.Helpers.Interfaces
{
    public interface IPhoneNumberDataXml
    {
        string GetCountryID(string areaCode);
        string GetPhoneFormat(int countryID, HomeSen.Helpers.Types.DISTANCE_RULE distanceRule);
        string GetCountryCode(string code);
        string GetAreaCode(string code);
        string GetCountryName(int countryID);
        string GetCityName(string areaCode);
        string GetCarrierName(string areaCode);
    }
}
