using Courses_HW_5_6_Part_2.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courses_HW_5_6_Part_2.Database
{
    public class HomeworkDbContext : DbContext
    {
        public HomeworkDbContext(DbContextOptions<HomeworkDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<Analysis> Analysis { get; set; }
        public DbSet<Orders> Orders { get; set; }
    }
}
