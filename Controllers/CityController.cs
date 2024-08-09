using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiPOC.DataBaseModel;
using WebApiPOC.Services.IServices;

namespace WebApiPOC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;
        // ==============In-Memory Caching implementation=================
        private readonly IMemoryCache _cache;
        static readonly string CacheKey = "city";
        // ============= Log Implementation =============================
        private readonly ILogger _logger;

        public CityController(ILogger<CityController> logger, ICityService cityService, IMemoryCache cache)
        {
            _cityService = cityService;
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCities()
        {
            if (!_cache.TryGetValue(CacheKey, out IEnumerable<City> cities))
            {
                cities = await _cityService.GetCities();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));

                _cache.Set(CacheKey, cities, cacheEntryOptions);
            }
            return Ok(cities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCity(int id)
        {
            try
            {
                _logger.LogInformation("To get City with Id :" + id.ToString());
                var city = await _cityService.GetCity(id);
                if (city == null) return NotFound();
                return Ok(city);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

        }

        [HttpPost]
        public async Task<ActionResult<City>> PostCity(City city)
        {
            try
            {
                _logger.LogInformation("To post City data :");
                var resultCity = await _cityService.PostCity(city);
                return Ok(resultCity);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(int id, City city)
        {
            try
            {
                _logger.LogInformation("To update City data Id :" + id.ToString());
                var result = await _cityService.PutCity(id, city);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            try
            {
                _logger.LogInformation("To delete City data Id :" + id.ToString());
                var result = await _cityService.DeleteCity(id);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }
    }
}
