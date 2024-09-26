using BlogApp.Core.Domain.DTOs;
using BlogApp.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Domain.Interfaces
{
    public interface ICategoryRepository 
    {
        Task<IEnumerable<Category>> GetAll();
        Task Create(Category category);
        Task<Category> GetById(int id);
        Task Update(Category category);
        Task<int> Delete(int id);
    }
}
