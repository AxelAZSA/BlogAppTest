using BlogApp.Core.Domain.DTOs;
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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BlogDbContext _context;

        public CategoryRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task Create(Category category)
        {
            await _context.Categories.AddAsync(category);

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
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return 0;

            _context.Entry(category).State = EntityState.Deleted;

            try
            {
                await Save();
                return category.Id;
            }
            catch (DbUpdateConcurrencyException)
            {
                return -1;
            }
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            List<Category> categories = new List<Category>();

            await CreateDefaultCategoriesAsync();

            categories = await _context.Categories.ToListAsync();

            return categories;
        }

        public async Task<Category> GetById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task Update(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        // Método para crear categorías por defecto si no existen
        public async Task CreateDefaultCategoriesAsync()
        {
            // Verifica si ya existen categorías en la base de datos
            if (!await _context.Categories.AnyAsync())
            {
                // Crea tres categorías por defecto
                var defaultCategories = new List<Category>
            {
                new Category { Name = "Technology", Description = "All about technology" },
                new Category { Name = "Lifestyle", Description = "Lifestyle and daily tips" },
                new Category { Name = "Education", Description = "Educational content" }
            };

                // Agrega las categorías a la base de datos
                await _context.Categories.AddRangeAsync(defaultCategories);
                await Save();
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
