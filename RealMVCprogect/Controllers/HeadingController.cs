using BusinessLayer;
using BusinessLayer.Maneger;
using DataAsseccLayer.Concreat;
using DataAsseccLayer.EntityFramework;
using EntityLayer.Concreat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RealMVCprogect.Controllers
{
    public class HeadingController : Controller
    {
       private readonly HeadingManager _headingManeger;
       private readonly CategoryManager _categoryManager;
       private readonly WriterMeneger _writerMeneger;
        private readonly AppDbContext _context;

        public HeadingController(AppDbContext context)
        {
            _context = context;
            _headingManeger = new HeadingManager(new EfHeadingDl(_context));
            _categoryManager = new CategoryManager(new EfCategoryDl(_context));
            _writerMeneger = new WriterMeneger(new EfWriterDl(_context));
        }
        public IActionResult Index()
        {
            var heading = _headingManeger.GetList();
            return View(heading);
        }

        [HttpGet]
        public IActionResult AddHeading()
        {
            var categoryvalue = (from x in _categoryManager.GetList()
                         select new SelectListItem
                         {
                             Text = x.CotegoryName,
                             Value = x.CotegoryId.ToString()
                         });
            List<SelectListItem> value = categoryvalue.ToList();

            var writervalue = (from x in _writerMeneger.GetList()
                               select new SelectListItem
                               {
                                   Text = x.WriterName,
                                   Value = x.WriterId.ToString()
                               }
                               );
            List<SelectListItem> val = writervalue.ToList();
            ViewBag.wv = val;
            ViewBag.vlc = value;
            return View();
        }

        [HttpPost]
        public IActionResult AddHeading(Heading heading)
        {

            DateTime data = DateTime.UtcNow;
            DateTime Utcdata = DateTime.SpecifyKind(data, DateTimeKind.Utc);    
            heading.HeadingDate = Utcdata;
            _headingManeger.HeadingAddBl(heading);
            return RedirectToAction("Index");
        }

        public IActionResult EditHeading(int id)
        {
            var categoryvalue = (from x in _categoryManager.GetList()
                                 select new SelectListItem
                                 {
                                     Text = x.CotegoryName,
                                     Value = x.CotegoryId.ToString()
                                 });
            List<SelectListItem> value = categoryvalue.ToList();

            ViewBag.vlc = value;
            

            var headingValue = _headingManeger.GetById(id);
            return View(headingValue);
        }

        [HttpPost]
        public IActionResult EditHeading(Heading p)
        {
            DateTime data = DateTime.UtcNow;
            DateTime Utcdata = DateTime.SpecifyKind(data, DateTimeKind.Utc);
            p.HeadingDate = Utcdata;
            _headingManeger.HeadingUpdateBl(p);
            return RedirectToAction("Index");

        }

        public IActionResult DeleteHeading(int id)
        {
            var delete = _headingManeger.GetById(id);
            delete.Headingstatus = false;
                _headingManeger.HeadingDeleteBl(delete);
            return RedirectToAction("Index");
        }
    }
}
