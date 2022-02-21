using Microsoft.EntityFrameworkCore;
using PracticaCore2LuisEF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticaCore2LuisEF.Data
{
    public class LibreriaContext : DbContext
    {
        public LibreriaContext(DbContextOptions<LibreriaContext> options) : base(options) { }

        public DbSet<Libro> Libros { get; set; }

        public DbSet<Genero> Generos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }

        public DbSet<VistaPedido> VistaPedidos { get; set; }
    }
}
