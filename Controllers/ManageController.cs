using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PracticaCore2LuisEF.Models;
using PracticaCore2LuisEF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PracticaCore2LuisEF.Controllers
{
    public class ManageController : Controller
    {
        private RepositoryLibreria repo;

        public ManageController(RepositoryLibreria repo)
        {
            this.repo = repo;
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            Usuario usuario = this.repo.LogInUsuario(email, password);
            if (usuario == null)
            {
                ViewData["MENSAJE"] = "No tienes credenciales correctas";
                return View();
            }
            else
            {
            
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                
                Claim claimUserName = new Claim(ClaimTypes.Name, usuario.Nombre);
                Claim claimIdUsuario = new Claim("IdUsuario", usuario.IdUsuario.ToString());
                Claim claimFoto = new Claim("Foto", usuario.Foto);
                Claim claimApellido = new Claim("Apellido", usuario.Apellidos);
                Claim claimEmail = new Claim("Email", usuario.Email);

                identity.AddClaim(claimUserName);
                identity.AddClaim(claimIdUsuario);
                identity.AddClaim(claimFoto);
                identity.AddClaim(claimApellido);
                identity.AddClaim(claimEmail);

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.Now.AddMinutes(45)
                });
                /*
                if(TempData["controller"].ToString() != null)
                {
                    string controller = TempData["controller"].ToString();
                    string action = TempData["action"].ToString();
                    return RedirectToAction(action, controller);
                }
                else
                {
                */
                    return RedirectToAction("Index", "Libreria");
                /*
                }
                */
                

                
            }
        }

        public IActionResult ErrorAcceso()
        {
            ViewData["MENSAJE"] = "Error de acceso";
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Libreria");
        }
    }
}
