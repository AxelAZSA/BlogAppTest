
using BlogApp.Core.Domain.DTOs;
using BlogApp.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Domain.Interfaces
{
  
    public interface IUserRepository 
    {
        Task<IEnumerable<UserDto>> GetAll();
        Task Create(RegisterDTO userTemp);
        Task<User> GetByCorreo(string correo);
        Task<User> GetById(int id);
        Task Update(User user);
        Task<int> Delete(int id);
    }
}
