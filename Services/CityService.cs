using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiPOC.DataBaseModel;
using WebApiPOC.Services.IServices;

namespace WebApiPOC.Services
{
    public class CityService : ICityService
    {
        private readonly IRepository<City> _cityRepository;

        public CityService(IRepository<City> cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<IEnumerable<City>> GetCities()
        {
            return await _cityRepository.GetAllAsync();
        }

        public async Task<City> GetCity(int id)
        {
            var city = await _cityRepository.GetByIdAsync(id);
            if (city == null)
            {
                return null;
            }
            return city;
        }

        public async Task<City> PostCity(City city)
        {
            return await _cityRepository.AddAsync(city);
        }


        public async Task<bool> PutCity(int id, City city)
        {
            if (id != city.Id) return false;

            var entity = await _cityRepository.UpdateAsync(city);
            return (entity!=null) ? true : false;
        }

        public async Task<bool> DeleteCity(int id)
        {
            var entity = await _cityRepository.DeleteAsync(id);
            return entity != null ? true : false;
        }
    }
}


