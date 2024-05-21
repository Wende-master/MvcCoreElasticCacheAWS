using Microsoft.AspNetCore.Mvc;
using MvcCoreElasticCacheAWS.Models;
using MvcCoreElasticCacheAWS.Repositories;

namespace MvcCoreElasticCacheAWS.Controllers
{
    public class CochesController : Controller
    {
        private RepositoryCoches repo;
        public CochesController(RepositoryCoches repo)
        {
            this.repo = repo;
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
    }
}
