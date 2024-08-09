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
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        // ============= In-Memory Implementation =============================
        private readonly IMemoryCache _cache;
        static readonly string CacheKey = "country";

        // ============= Log Implementation =============================
        private readonly ILogger _logger;

        public CountryController(ILogger<CountryController> logger, ICountryService countryService, IMemoryCache cache)
        {
            _countryService = countryService;
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            _logger.LogInformation("Get Country List ");
            if (!_cache.TryGetValue(CacheKey, out IEnumerable<Country> countries))
            {
                countries = await _countryService.GetCountries();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));

                _cache.Set(CacheKey, countries, cacheEntryOptions);
            }
            return Ok(countries);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetCountry(int id)
        {
            _logger.LogInformation("Get Country with Id : " + id.ToString());
            var country = await _countryService.GetCountry(id);
            
            return Ok(country);
        }

        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(Country country)
        {
            _logger.LogInformation("Post Country Data.  " );
            var resultCountry = await _countryService.PostCountry(country);
            return Ok(resultCountry);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, Country country)
        {
            _logger.LogInformation("Update Country with Id : " + id.ToString());
            var resultCountry = await _countryService.PutCountry(id,country);
            return Ok(resultCountry);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            _logger.LogInformation("Delete Country with Id : " + id.ToString());
            var resultCountry = await _countryService.DeleteCountry(id);
            return Ok(resultCountry);
        }
    }
}

