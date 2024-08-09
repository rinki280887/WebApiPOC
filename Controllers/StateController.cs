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
    public class StateController : ControllerBase
    {
        private readonly IStateService _stateService;
        // ============= In-Memory Caching Implementation =============================
        private readonly IMemoryCache _cache;
        static readonly string CacheKey = "state";

        // ============= Log Implementation =============================
        private readonly ILogger _logger;

        public StateController(ILogger<StateController> logger, IStateService stateService, IMemoryCache cache)
        {
            _stateService = stateService;
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<State>>> GetStates()
        {
            _logger.LogInformation("Get State List ");
            if (!_cache.TryGetValue(CacheKey, out IEnumerable<State> stateList))
            {
                stateList = await _stateService.GetStates();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));

                _cache.Set(CacheKey, stateList, cacheEntryOptions);
            }
            return Ok(stateList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<State>> GetState(int id)
        {
            try
            {
                _logger.LogInformation("Get State with Id :" + id.ToString());
                var state = await _stateService.GetState(id);

                return Ok(state);
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
        public async Task<ActionResult<State>> PostState(State state)
        {
            try
            {
                _logger.LogInformation("POST State data.");
                var resultState = await _stateService.PostState(state);
                return Ok(resultState);
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
        public async Task<IActionResult> PutState(int id, State state)
        {
            try
            {
                _logger.LogInformation("Update State with Id :" + id.ToString());
                var resultState = await _stateService.PutState(id, state);
                return Ok(resultState);
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
        public async Task<IActionResult> DeleteState(int id)
        {
            try
            {
                _logger.LogInformation("Delete State with Id :" + id.ToString());
                var resultState = await _stateService.DeleteState(id);
                return Ok(resultState);
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
