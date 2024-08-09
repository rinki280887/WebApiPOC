using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiPOC.DataBaseModel;
using WebApiPOC.Services.IServices;

namespace WebApiPOC.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<User> _userRepository;
        public AccountService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;
            
            return user;
        }

        public async Task<User> PostUser(User user)
        {
            return await _userRepository.AddAsync(user);
        }

        public async Task<bool> PutUser(int id, User user)
        {
            if (id != Convert.ToInt32(user.Id)) return false;

            var entity = await _userRepository.UpdateAsync(user);
            
            return (entity!=null) ? true : false;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _userRepository.DeleteAsync(id);
            if (user == null) return false;
            
            return true;
        }

        public async Task<User> GetUserByUserNamePassword(string userName, string password)
        {
            var users = await _userRepository.GetAllAsync();
             var user = users.Where(x=>x.UserName == userName && x.Password == password).FirstOrDefault();
            if (user == null) return null;

            return user;
        }
    }
}



