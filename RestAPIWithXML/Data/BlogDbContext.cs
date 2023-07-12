using Microsoft.EntityFrameworkCore;
using RestAPIWithXML.Models;
using System.Reflection.Metadata;

namespace RestAPIWithXML.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        public DbSet<BlogModel> Blogs { get; set; }
    }
}
