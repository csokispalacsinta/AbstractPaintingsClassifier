using ClassifierWebApplication.Models;
using Emgu.CV;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace ClassifierWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [RequestSizeLimit(5000000)]
        public IActionResult Result(IFormFile picturedata)
        {
            Painting painting = new Painting();
            using (var stream = picturedata.OpenReadStream())
            {
                byte[] buffer = new byte[picturedata.Length];
                stream.Read(buffer, 0, (int)picturedata.Length);
                painting.FileName = Guid.NewGuid().ToString() + "_" + picturedata.FileName;
                System.IO.File.WriteAllBytes(Path.Combine("wwwroot", "Images", painting.FileName), buffer);
              
                Classifier classifier = new Classifier();
                painting.Style = classifier.GetStyle(Path.Combine("wwwroot", "Images", painting.FileName));
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            return View(painting);
        }


        [HttpGet]
        public IActionResult Classifier()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}