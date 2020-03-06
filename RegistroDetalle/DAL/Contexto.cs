using System;
using System.Collections.Generic;
using System.Text;
using RegistroDetalle.Entidades;
using Microsoft.EntityFrameworkCore;

namespace RegistroDetalle.DAL
{
    public class Contexto : DbContext
    {
        public DbSet<Persona> Personas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging().UseSqlite(@"Data Source = DetallePersonas.db");
        }
    }
}
