using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetSizeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetSizeAPI.Data
{
    public class NetSizeDbContext : IdentityDbContext
    {
        public NetSizeDbContext(DbContextOptions<NetSizeDbContext> options) : base(options)
        {
         
        }

        public DbSet<AppTask> Tasks { get; set; }
    }
}
