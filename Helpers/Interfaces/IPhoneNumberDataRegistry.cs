
namespace HomeSen.Helpers.Interfaces
{
    public interface IPhoneNumberDataRegistry
    {
        string GetUserCountryID();
        string GetUserAreaCode();
        string GetPhoneFormat(int countryCode, HomeSen.Helpers.Types.DISTANCE_RULE distanceRule);
        string GetCountryName(int countryID);
    }
}
