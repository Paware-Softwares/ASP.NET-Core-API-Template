using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Data
{
    public class APIContext : DbContext
    {
        public APIContext (DbContextOptions<APIContext> options)
            : base(options)
        {
        }

        public DbSet<Costumer> Costumers { get; set; }

        public DbSet<Apointment> Apointments { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
