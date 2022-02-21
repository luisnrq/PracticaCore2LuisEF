using Microsoft.AspNetCore.Mvc;
using PracticaCore2LuisEF.Models;
using PracticaCore2LuisEF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticaCore2LuisEF.ViewComponents
{
    public class MenuLibrosViewComponent : ViewComponent
    {
        private RepositoryLibreria repo;

        public MenuLibrosViewComponent(RepositoryLibreria repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genero> generos = this.repo.GetGeneros();
            return View(generos);
        }
    }
}
