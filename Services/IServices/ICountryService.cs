using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiPOC.DataBaseModel;

namespace WebApiPOC.Services.IServices
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetCountries();
        Task<Country> GetCountry(int id);
        Task<Country> PostCountry(Country country);
        Task<bool> PutCountry(int id, Country country);
        Task<bool> DeleteCountry(int id);
    }
}
