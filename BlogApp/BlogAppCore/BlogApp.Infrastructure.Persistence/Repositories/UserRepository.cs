
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
    public class UserRepository : IUserRepository
    {
        private readonly BlogDbContext _context;

        public UserRepository(BlogDbContext context)
        {
            _context = context;
        }
        public async Task Create(RegisterDTO userTemp)
        {
            User user = new User()
            {
                Email = userTemp.email,
                FirstName = userTemp.firstName,
                LastName = userTemp.lastName,
                PasswordHash = userTemp.password,
                RegisteredAt = DateTime.Now
            };

            await _context.Users.AddAsync(user);

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
            var Users = await _context.Users.FindAsync(id);

            if (Users == null)
                return 0;

            _context.Entry(Users).State = EntityState.Deleted;

            try
            {
                await Save();
                return Users.Id;
            }
            catch (DbUpdateConcurrencyException)
            {
                return -1;
            }
        }

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            List<UserDto> userDtos = new List<UserDto>();

            var Users = await _context.Users.ToListAsync();
            foreach (var user in Users)
            {
                UserDto dto = new UserDto()
                {
                    id = user.Id,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    email = user.Email
                };

                userDtos.Add(dto);
            }

            return userDtos;
        }

        public async Task<User> GetByCorreo(string correo)
        {
            return await _context.Users.FirstOrDefaultAsync(s => s.Email == correo);
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;

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
