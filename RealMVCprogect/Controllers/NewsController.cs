using BusinessLayer.Maneger;
using BusinessLayer.ValidationRele;
using DataAsseccLayer.Concreat;
using DataAsseccLayer.EntityFramework;
using EntityLayer.Concreat;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace RealMVCprogect.Controllers
{
    public class NewsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly NewsManager _newsManager;
        private static int _id;

        public NewsController(AppDbContext context)  // <-- Context konstruktor orqali keladi
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            var isUser = _context.Users.FirstOrDefault(n => n.Name == CurrentUser.UserName);
            _id = isUser.id;
            _newsManager = new NewsManager(new EfNews(_context));
        }
        // NewsManager manager = new NewsManager(new EfNews());
        NewsValidation validations = new NewsValidation();
        public IActionResult Index()
        {
            var newsList = _newsManager.GetList();
            return View(newsList);
        }

        [HttpPost]
        public async Task<IActionResult> UploadNews(int newsId, string title, string titleRu, string content, string contentRu, int status, IFormFile[] files, IFormFile[] filesRu)
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
        public IActionResult DeleteNews(int Id)
        {
            var getdate = _newsManager.GetById(Id);
            return View(getdate);
        }

        [HttpPost]

        public IActionResult ConfirmDeleteNews(int newsId)
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


        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile upload)
        {
            if (upload != null && upload.Length > 0)
            {

                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(upload.FileName)}";
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await upload.CopyToAsync(stream);
                }

                var url = Url.Content($"~/uploads/{fileName}");
                return Json(new { uploaded = true, url });
            }

            return Json(new { uploaded = false, error = new { message = "Fayl yuklanmadi." } });
        }

        private int GetLatestNewsIdFromUser()
        {
            // Masalan, hozirgi admin userning ID sini session yoki claim orqali oling
            int currentUserId = Convert.ToInt32(CurrentUser.id); // yoki claim

            //using (var db = new AppDbContext(_context))
            //{
            //    // Ushbu foydalanuvchiga tegishli oxirgi News ni topamiz
            //    var latestNews = db.News
            //                       .Where(n => n.UserId == currentUserId)
            //                       .OrderByDescending(n => n.Id)
            //                       .FirstOrDefault();

            //    return latestNews?.Id ?? 0;
            //}
            var latestNews = _context.News.Where(n => n.UserId == currentUserId).OrderByDescending(n => n.Id).FirstOrDefault();
            return latestNews?.Id ?? 0;
        }


        [HttpPost]
        public async Task<IActionResult> UploadImage_Reserve(IFormFile upload)
        {
            if (upload != null && upload.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(upload.FileName)}";

                // ✅ Rasmlar saqlanadigan papka
                var uploadsFolder = @"D:\Admin\img";

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await upload.CopyToAsync(stream);
                }

                // ✅ Bu yo‘l bazaga saqlanadi
                string savedPath = filePath;

                // 🔄 Agar siz bazaga yozmoqchi bo‘lsangiz (masalan, oxirgi NewsId bilan bog‘lasangiz)
                var photo = new Photos
                {
                    ImagePath = savedPath,
                    NewsId = GetLatestNewsIdFromUser() // bu metod sizda allaqachon bo‘lishi kerak
                };


                _context.Photos.Add(photo);
                await _context.SaveChangesAsync();

                return Json(new { uploaded = true, path = savedPath });
            }

            return Json(new { uploaded = false, error = new { message = "Fayl yuklanmadi." } });
        }



        [HttpGet]
        public IActionResult GetByIdNew(int Id)
        {
            var getId = _newsManager.GetById(Id);
            return View(getId);
        }


        [HttpGet]
        public IActionResult AddNews()
        {
            return View();
        }



        [HttpPost]
        public IActionResult AddNews(News news, IFormFile files, IFormFile filesRu)
        {
            if (news == null)
                return View();
            if (files != null && files.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(files.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", uniqueFileName);
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    files.CopyTo(stream);
                }
                news.photoNews = "/uploads/" + uniqueFileName;
            }

            if (filesRu != null && filesRu.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(filesRu.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", uniqueFileName);
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    filesRu.CopyTo(stream);
                }
                news.PhotoNewsRu = "/uploads/" + uniqueFileName;
            }
            news.PublishDate = DateTime.Now;
            news.PublishDate = DateTime.Now;
            news.UserId = _id;
            _newsManager.NewsAddBl(news);
            return RedirectToAction("Index");

        }


    }
}