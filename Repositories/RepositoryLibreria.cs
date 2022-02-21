using PracticaCore2LuisEF.Data;
using PracticaCore2LuisEF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticaCore2LuisEF.Repositories
{
    public class RepositoryLibreria
    {
        private LibreriaContext context;

        public RepositoryLibreria(LibreriaContext context)
        {
            this.context = context;
        }

        public List<Libro> GetLibros()
        {
            var consulta = from datos in this.context.Libros
                           select datos;
            return consulta.ToList();
        }

        public Libro GetLibro(int idlibro)
        {
            var consulta = from datos in this.context.Libros
                           where datos.IdLibro == idlibro
                           select datos;
            return consulta.FirstOrDefault();
        }

        public List<Genero> GetGeneros()
        {
            var consulta = from datos in this.context.Generos
                           select datos;
            return consulta.ToList();
        }

        public List<Libro> GetLibrosGenero(int idgenero)
        {
            var consulta = from datos in this.context.Libros
                           where datos.IdGenero == idgenero
                           select datos;
            return consulta.ToList();
        }

        public List<Libro> GetLibrosSession(List<int> listIdLibros)
        {
            var consulta = from datos in this.context.Libros
                           where listIdLibros.Contains(datos.IdLibro)
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            return consulta.ToList();
        }

        public Usuario LogInUsuario(string email, string password)
        {
            Usuario usuario = this.context.Usuarios.SingleOrDefault(x => x.Email == email && x.Pass == password);
            if(usuario == null)
            {
                return null;
            }
            else
            {
                return usuario;
            }
        }

        private int GetMaxIdPedido()
        {
            if (this.context.Pedidos.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Pedidos.Max(z => z.IdPedido) + 1;
            }
        }

        private int GetMaxIdFactura()
        {
            if (this.context.Pedidos.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Pedidos.Max(z => z.IdFactura) + 1;
            }
        }

        public void CrearPedido(List<int> listIdLibros, int idusuario)
        {
            int idfactura = this.GetMaxIdFactura();
            for (var i =0; i < listIdLibros.Count(); i++)
            {
                Pedido pedido = new Pedido();
                pedido.IdPedido = this.GetMaxIdPedido();
                pedido.IdFactura = idfactura;
                pedido.Fecha = DateTime.Now;
                pedido.IdLibro = listIdLibros[i];
                pedido.Cantidad = 1;
                pedido.IdUsuario = idusuario;
                this.context.Pedidos.Add(pedido);
                this.context.SaveChanges();
            }
            
        }

        public List<VistaPedido> GetPedidosUsuario(int idusuario)
        {
            var consulta = from datos in this.context.VistaPedidos
                           where datos.IdUsuario == idusuario
                           select datos;
            
            if (consulta.Count() == 0)
            {
                return null;
            }
            
            return consulta.ToList();
        }





    }
}
