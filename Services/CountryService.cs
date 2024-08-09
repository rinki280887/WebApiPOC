using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiPOC.DataBaseModel;
using WebApiPOC.Services.IServices;

namespace WebApiPOC.Services
{
    public class CountryService : ICountryService
    {
        private readonly IRepository<Country> _countryRepository;
        public CountryService(IRepository<Country> countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<IEnumerable<Country>> GetCountries()
        {
            return await _countryRepository.GetAllAsync();
        }

        public async Task<Country> GetCountry(int id)
        {
            var country = await _countryRepository.GetByIdAsync(id);
            if (country == null)
            {
                return null;
            }
            return country;
        }

        public async Task<Country> PostCountry(Country country)
        {
            return await _countryRepository.AddAsync(country);
        }

        
        public async Task<bool> PutCountry(int id, Country country)
        {
            if (id != country.Id) return false;
            var entity = await _countryRepository.UpdateAsync(country);
            return (entity != null) ? true : false;
        }

        public async Task<bool> DeleteCountry(int id)
        {
            var country = await _countryRepository.DeleteAsync(id);
            if (country == null)
            {
                return false;
            }
            
            return true;
        }
    }
}

