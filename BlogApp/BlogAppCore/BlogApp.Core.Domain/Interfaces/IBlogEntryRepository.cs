using BlogApp.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Domain.Interfaces
{
    public interface IBlogEntryRepository 
    {
        Task<IEnumerable<BlogEntry>> GetAll();
        Task Create(BlogEntry entry);
        Task<BlogEntry> GetById(int id);
        Task Update(BlogEntry entry);
        Task<int> Delete(int id);
    }
}
