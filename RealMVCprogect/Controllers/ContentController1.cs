using BusinessLayer.Maneger;
using DataAsseccLayer.Concreat;
using DataAsseccLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace RealMVCprogect.Controllers
{
    public class ContentController1 : Controller
    {
        private readonly ContentManager manager;
        private readonly AppDbContext _context;
        public ContentController1(AppDbContext context)
        {
            _context = context;
            manager = new ContentManager(new EfContentDl(_context));
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ContentByHeading(int id)
        {
            var con = manager.GetListByHeadingId(id);
            return View(con);
        }
    }
}
