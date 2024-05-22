using Microsoft.AspNetCore.Mvc;
using MvcCoreElasticCacheAWS.Models;
using MvcCoreElasticCacheAWS.Repositories;
using MvcCoreElasticCacheAWS.Services;

namespace MvcCoreElasticCacheAWS.Controllers
{
    public class CochesController : Controller
    {
        private RepositoryCoches repo;
        private ServiceAWSCache cache;
        public CochesController(RepositoryCoches repo, ServiceAWSCache cache)
        {
            this.repo = repo;
            this.cache = cache;
        }
        public IActionResult Index()
        {
            List<Coche> coches =
                this.repo.GetCoches();
            return View(coches);
        }

        public IActionResult Details(int id)
        {
            Coche coche = this.repo.FindCoche(id);
            return View(coche);
        }

        public async Task<IActionResult> SeleccionarFavorito(int id)
        {
            //BUSCAMOS EL FAVORITO
            Coche coche =
                this.repo.FindCoche(id);
            await this.cache.AddCocheFavoritoAsync(coche);
            return RedirectToAction("Favoritos");
        }

        public async Task<IActionResult> Favoritos()
        {
            List<Coche> favoritos = 
                await this.cache.GetCochesFavoritosAsync();
            return View(favoritos);
        }

        public async Task<IActionResult> DeleteFavorito(int id)
        {
            await this.cache.DeleteCocheFavoritoAsync(id);
            return RedirectToAction("Favoritos");
        }
    }
}
