using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiPOC.DataBaseModel;

namespace WebApiPOC.Services.IServices
{
    public interface ICityService
    {
        Task<IEnumerable<City>> GetCities();
        Task<City> GetCity(int id);
        Task<City> PostCity(City city);
        Task<bool> PutCity(int id, City city);
        Task<bool> DeleteCity(int id);
    }
}
