using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiPOC.DataBaseModel;
using WebApiPOC.Services.IServices;

namespace WebApiPOC.Services
{
    public class StateService : IStateService
    {
        private readonly IRepository<State> _stateRepository;
        public StateService(IRepository<State> stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public async Task<IEnumerable<State>> GetStates()
        {
            return await _stateRepository.GetAllAsync();
        }

        public async Task<State> GetState(int id)
        {
            var state = await _stateRepository.GetByIdAsync(id);
            if (state == null)
            {
                return null;
            }
            return state;
        }

        public async Task<State> PostState(State state)
        {
            return await _stateRepository.AddAsync(state);
        }

        public async Task<bool> PutState(int id, State state)
        {
            if (id != state.Id) return false;

            var entity = await _stateRepository.UpdateAsync(state);

            return (entity != null) ?true: false;
        }

        public async Task<bool> DeleteState(int id)
        {
            var state = await _stateRepository.DeleteAsync(id);
            if (state == null)
            {
                return false;
            }
            return true;
        }
    }
}


