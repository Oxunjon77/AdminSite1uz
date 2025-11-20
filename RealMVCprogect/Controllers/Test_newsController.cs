using BusinessLayer.Maneger;
using DataAsseccLayer.Concreat;
using DataAsseccLayer.EntityFramework;
using EntityLayer.Concreat;
using Microsoft.AspNetCore.Mvc;

namespace RealMVCprogect.Controllers
{
    public class Test_newsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly NewsManager _newsManager;
        private static int _id;

        public Test_newsController(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            var isUser = _context.Users.FirstOrDefault(n => n.Name == CurrentUser.UserName);
            _id = isUser.id;
            _newsManager = new NewsManager(new EfNews(context));
        }


        // NewsManager manager = new NewsManager(new EfNews());
        public IActionResult Index()
        {
            var gettestNews = _newsManager.GetList();
            return View(gettestNews);
        }

        [HttpGet]
        public IActionResult TestGetByIdNew(int Id)
        {
            var getId = _newsManager.GetById(Id);
            return View(getId);
        }

        [HttpPost]
        public async Task<IActionResult> TestUploadNews(int newsId, string title, string titleRu, string content, string contentRu, int status, IFormFile[] files, IFormFile[] filesRu, DateTime? ScheduledDate)
        {
            var news = _context.News.FirstOrDefault(n => n.Id == newsId);


            if (news != null)
            {
                // Yangilikni yangilash
                news.Title = title;
                news.TitleRu = titleRu;
                news.Content = content;
                news.ContentRu = contentRu;
                news.PublishDate = DateTime.Now;
                news.status = status;
                news.ScheduledDate = ScheduledDate;
                if (files != null && files.Length > 0)
                {
                    // Eski rasmni o'chirish
                    if (!string.IsNullOrEmpty(news.photoNews))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", news.photoNews.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath); // Eski rasmni o'chirish
                        }
                    }

                    // Yangi rasmni saqlash
                    var file = files[0];  // Birinchi faylni olish
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);

                    // Faylni yuklash
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Rasm yo'lini yangilash
                    news.photoNews = $"/uploads/{file.FileName}";
                }

                if (filesRu != null && filesRu.Length > 0)
                {
                    // Eski rasmni o'chirish
                    if (!string.IsNullOrEmpty(news.PhotoNewsRu))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", news.PhotoNewsRu.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath); // Eski rasmni o'chirish
                        }
                    }

                    // Yangi rasmni saqlash
                    var file = filesRu[0];  // Birinchi faylni olish
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);

                    // Faylni yuklash
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Rasm yo'lini yangilash
                    news.PhotoNewsRu = $"/uploads/{file.FileName}";
                }

                // Yangilangan ma'lumotlarni saqlash
                _context.News.Update(news);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult TestDeleteNews(int Id)
        {
            var getdate = _newsManager.GetById(Id);
            return View(getdate);
        }



        [HttpPost]

        public IActionResult TestConfirmDeleteNews(int newsId)
        {
            var news = _newsManager.GetById(newsId);
            if (news == null)
            {
                TempData["Error"] = "Янгилик топилмади.";
                return RedirectToAction("Index");
            }

            // Rasming yo‘lini aniqlash
            if (!string.IsNullOrEmpty(news.photoNews))
            {
                string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                string imagePath = Path.Combine(rootPath, news.photoNews.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

                // Agar fayl mavjud bo‘lsa - o‘chiramiz
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            // Yangilikni bazadan o‘chirish
            _newsManager.NewsDeleteBl(news);

            TempData["Success"] = "Янгилик ва расм муваффақиятли ўчирилди.";
            return RedirectToAction("Index");


        }


    }
}