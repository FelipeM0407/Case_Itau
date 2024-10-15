using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseItau.API.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaseItau.API.src.Infrastructure.Persistence.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Fundo> Fundos { get; set; }
        public DbSet<TipoFundo> TipoFundos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Fundo>()
                .ToTable("FUNDO")
                .HasKey(f => f.Codigo);

            modelBuilder.Entity<Fundo>()
                .HasOne(f => f.TipoFundo)
                .WithMany(tf => tf.Fundos)
                .HasForeignKey(f => f.Codigo_Tipo);

            modelBuilder.Entity<TipoFundo>()
                .HasKey(tf => tf.Codigo);

            modelBuilder.Entity<TipoFundo>()
                .ToTable("TIPO_FUNDO");
        }


    }
}