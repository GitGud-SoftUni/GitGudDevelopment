using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GitGud.Models
{
    public class GitGudContext : IdentityDbContext<User>
    {
        private IConfigurationRoot _config;

        public GitGudContext(IConfigurationRoot config, DbContextOptions options) : base(options)
        {
            _config = config;
           
        }

        public DbSet<Song> Songs { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config["ConnectionStrings:GitGudContextConnection"]);
        }
    }
}
