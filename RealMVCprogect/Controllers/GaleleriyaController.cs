using BusinessLayer.Maneger;
using DataAsseccLayer.Concreat;
using DataAsseccLayer.EntityFramework;
using EntityLayer.Concreat;
using Microsoft.AspNetCore.Mvc;

namespace RealMVCprogect.Controllers
{
	public class GaleleriyaController : Controller
	{
		private readonly ImageFileManeger image;
		private readonly AppDbContext _context;
        public GaleleriyaController(AppDbContext context)
        {
			_context = context;
            image = new ImageFileManeger(new EfImageFileDal(_context));
        }
        public IActionResult Index()
		{
			var files = image.GetList();
			return View(files);
		}
	}
}
