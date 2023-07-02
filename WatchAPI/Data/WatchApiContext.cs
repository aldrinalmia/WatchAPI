using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WatchAPI.Model;

namespace WatchAPI.Data
{
    public class WatchApiContext : DbContext
    {
        public WatchApiContext()
        {
        }

        public WatchApiContext(DbContextOptions<WatchApiContext> options)
            : base(options)
        {
        }

        public DbSet<WatchAPI.Model.Watch> Watches { get; set; }
    }
}
