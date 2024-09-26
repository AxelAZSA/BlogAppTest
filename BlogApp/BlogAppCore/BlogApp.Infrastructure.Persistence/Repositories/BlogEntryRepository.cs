using BlogApp.Core.Domain.Interfaces;
using BlogApp.Core.Domain.Models;
using BlogApp.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Persistence.Repositories
{
    public class BlogEntryRepository : IBlogEntryRepository
    {

        private readonly BlogDbContext _context;

        public BlogEntryRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task Create(BlogEntry entry)
        {
            await _context.BlogEntries.AddAsync(entry);

            try
            {
                await Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task<int> Delete(int id)
        {
            var entry = await _context.BlogEntries.FindAsync(id);

            if (entry == null)
                return 0;

            _context.Entry(entry).State = EntityState.Deleted;

            try
            {
                await Save();
                return entry.IdEntry;
            }
            catch (DbUpdateConcurrencyException)
            {
                return -1;
            }
        }

        public async Task<IEnumerable<BlogEntry>> GetAll()
        {
            List<BlogEntry> entries = new List<BlogEntry>();

            entries = await _context.BlogEntries.ToListAsync();

            return entries;
        }

        public async Task<BlogEntry> GetById(int id)
        {
            return await _context.BlogEntries.FindAsync(id);
        }

        public async Task Update(BlogEntry entry)
        {
            _context.Entry(entry).State = EntityState.Modified;

            try
            {
                await Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
