using BlogApp.Core.Domain.Models;
using BlogApp.Core.Models.Tokens;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Persistence.Data
{
    public class BlogDbContext : DbContext
    {

        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<BlogEntry> BlogEntries => Set<BlogEntry>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

     

    }
}
