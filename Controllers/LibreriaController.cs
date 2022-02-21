using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticaCore2LuisEF.Models;
using PracticaCore2LuisEF.Extensions;
using PracticaCore2LuisEF.Repositories;
using PracticaCore2LuisEF.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticaCore2LuisEF.Controllers
{
    public class LibreriaController : Controller
    {

        private RepositoryLibreria repo;

        public LibreriaController(RepositoryLibreria repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Libro> libros = this.repo.GetLibros();
            return View(libros);
        }

        public IActionResult Genero(int idgenero)
        {
            List<Libro> libros = this.repo.GetLibrosGenero(idgenero);
            return View(libros);
        }

        public IActionResult Libro(int idlibro, int? idlibrocarrito)
        {
            if (idlibrocarrito != null)
            {
                List<int> listaIdLibros;
                if (HttpContext.Session.GetString("IDSLIBROS") == null)
                {
                    listaIdLibros = new List<int>();
                }
                else
                {
                    listaIdLibros = HttpContext.Session.GetObject<List<int>>("IDSLIBROS");
                }
                listaIdLibros.Add(idlibrocarrito.Value);

                HttpContext.Session.SetObject("IDSLIBROS", listaIdLibros);
            }

            Libro libro = this.repo.GetLibro(idlibro);
            return View(libro);
        }

        public IActionResult Carrito(int? ideliminar)
        {
            List<int> listIdLibros = HttpContext.Session.GetObject<List<int>>("IDSLIBROS");

            if (listIdLibros == null)
            {
                ViewData["MENSAJE"] = "No existen libros en el carrito";
                return View();
            }
            else
            {
                if (ideliminar != null)
                {
                    listIdLibros.Remove(ideliminar.Value);
                    if (listIdLibros.Count == 0)
                    {
                        HttpContext.Session.Remove("IDSLIBROS");
                    }
                    else
                    {
                        HttpContext.Session.SetObject("IDSLIBROS", listIdLibros);
                    }

                }

                List<Libro> libros = this.repo.GetLibrosSession(listIdLibros);
                return View(libros);
            }
        }

        [AuthorizeUsers]
        public IActionResult Comprar()
        {
            List<int> listIdLibros = HttpContext.Session.GetObject<List<int>>("IDSLIBROS");
            int idusuario = int.Parse(HttpContext.User.FindFirst("IdUsuario").Value);
            this.repo.CrearPedido(listIdLibros, idusuario);
            HttpContext.Session.Remove("IDSLIBROS");
            return RedirectToAction("ComprasRealizadas", "Libreria");
        }

        [AuthorizeUsers]
        public IActionResult ComprasRealizadas()
        {
            int idusuario = int.Parse(HttpContext.User.FindFirst("IdUsuario").Value);
            List<VistaPedido> vistapedidos = this.repo.GetPedidosUsuario(idusuario);
            return View(vistapedidos);
        }

        [AuthorizeUsers]
        public IActionResult Perfil()
        {
            return View();
        }


    }
}
