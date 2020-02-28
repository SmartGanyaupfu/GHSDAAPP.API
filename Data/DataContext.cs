using GHSDAAPP.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GHSDAAPP.Data
{
    public class DataContext : DbContext
    {
        //specify the datatype in the constructor in this case its DataContext// or the class we re using for our dbcontext
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
