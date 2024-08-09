using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiPOC.DataBaseModel;

namespace WebApiPOC.Services.IServices
{
    public interface IAccountService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
        Task<User> PostUser(User user);
        Task<bool> PutUser(int id, User user);
        Task<bool> DeleteUser(int id);
        Task<User> GetUserByUserNamePassword(string userName, string password);
    }
}
