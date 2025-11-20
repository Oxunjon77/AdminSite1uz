using BusinessLayer.Maneger;
using BusinessLayer.ValidationRele;
using DataAsseccLayer.Concreat;
using DataAsseccLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace RealMVCprogect.Controllers
{
    public class ContectController : Controller
    {
        private readonly ContactMenager menager;
        private readonly AppDbContext _context;
        private readonly ContectValidator validator = new ContectValidator();

        public ContectController(AppDbContext context)
        {
            _context = context;
            menager = new ContactMenager(new EfContactDl(_context));
        }

        public IActionResult Index()
        {
            var contectval = menager.GetList();
            return View(contectval);
        }

        public IActionResult GetContectDetails(int Id)
        {
            var contectvalue = menager.ContactById(Id);
            return View(contectvalue);

        }

        public PartialViewResult MessageListMenyu()
        {
            return PartialView();
        }
    }
}
