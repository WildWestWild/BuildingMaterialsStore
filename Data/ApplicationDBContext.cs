using System.Linq;
using BuildingMaterialsStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace BuildingMaterialsStore.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Category { get; set; } 
        
        public DbSet<ApplicationType> ApplicationTypes { get; set; }
        
        public DbSet<Product> Product { get; set; }
        
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
} 