using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiPOC.DataBaseModel;

namespace WebApiPOC.Services.IServices
{
    public interface IStateService
    {
        Task<IEnumerable<State>> GetStates();
        Task<State> GetState(int id);
        Task<State> PostState(State country);
        Task<bool> PutState(int id, State country);
        Task<bool> DeleteState(int id);
    }
}
