using BusinessLayer.Maneger;
using DataAsseccLayer.Concreat;
using DataAsseccLayer.EntityFramework;
using EntityLayer.Concreat;
using Microsoft.AspNetCore.Mvc;


namespace RealMVCprogect.Controllers
{
    public class AboutController : Controller
    {

        private readonly AboutManager manager;
        private readonly AppDbContext _context;
        public AboutController(AppDbContext context)
        {
            _context = context;
            manager = new AboutManager(new EfAboutDl(_context));
        }
        public IActionResult Index()
        {
            var getvalue = manager.GetList();
            return View(getvalue);
        }

        [HttpPost]
        public IActionResult AddAbout(About about)
        {
            manager.AboutAdd(about);
            return RedirectToAction("Index");
        }

        public PartialViewResult AboutPartial()
        {
            Console.WriteLine(  );
            return PartialView();
        }
    }
}
