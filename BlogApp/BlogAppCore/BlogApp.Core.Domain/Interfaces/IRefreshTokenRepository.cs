
using BlogApp.Core.Models.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Domain.IRepository
{
    public interface IRefreshTokenRepository
    {
        Task CreateRefreshToken(RefreshToken refresh);
        Task<RefreshToken> GetByToken(string token);
        Task DeleteRefreshToken(int tokenId);
        Task DeleteAll(int UserId);
    }
}
