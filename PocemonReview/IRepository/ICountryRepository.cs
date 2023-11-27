using PocemonReview.Models;

namespace PocemonReview.IRepository
{
    public interface ICountryRepository
    {
        ICollection<Country> GetAllCountries();
        Country GetCountry(int id);
        Country GetCountryByOwner(int ownerId);
        ICollection<Owner> GetOwnersFromCountry(int id);
        bool CountryExist(int id);
        bool CreateCountry(Country country);
        bool UpdateCountry(Country country);
        bool Save();
    }
    
}
