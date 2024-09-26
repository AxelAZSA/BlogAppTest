using BlogApp.Core.Domain.IRepository;
using BlogApp.Core.Models.Tokens;
using BlogApp.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly BlogDbContext _context;
        public RefreshTokenRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task CreateRefreshToken(RefreshToken refresh)
        {
            await _context.RefreshTokens.AddAsync(refresh);
            try
            {
                await Save();
            }
            catch
            {
                throw;
            };
        }

        public async Task DeleteAll(int UserId)
        {
            var tokens = await _context.RefreshTokens.Where(u => u.idUser == UserId).ToListAsync();
            foreach (var token in tokens)
            {
                await DeleteRefreshToken(token.id);
            }
        }

        public async Task DeleteRefreshToken(int tokenId)
        {
            var token = await _context.RefreshTokens.FindAsync(tokenId);
            _context.Entry(token).State = EntityState.Deleted;
            try
            {
                await Save();
            }
            catch
            {
                throw;
            }
        }

        public async Task<RefreshToken> GetByToken(string token)
        {
            var tokenDTO = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.token == token);

            return tokenDTO;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
