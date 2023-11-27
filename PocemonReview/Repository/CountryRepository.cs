using PocemonReview.Data;
using PocemonReview.IRepository;
using PocemonReview.Models;

namespace PocemonReview.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;
        public CountryRepository(DataContext context)
        {
            _context = context;
        }

        public bool CountryExist(int id)
        {
            return _context.Countries.Any(c => c.Id == id);
        }

        public bool CreateCountry(Country country)
        {
            _context.Add(country);
            return Save();
        }

        public ICollection<Country> GetAllCountries()
        {
           return _context.Countries.ToList();
        }

        public Country GetCountry(int id)
        {
            return _context.Countries.Where(c => c.Id == id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromCountry(int id)
        {
            return _context.Owners.Where(c => c.Country.Id == id).ToList();
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0;
        }

        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return Save();
        }
    }
}
