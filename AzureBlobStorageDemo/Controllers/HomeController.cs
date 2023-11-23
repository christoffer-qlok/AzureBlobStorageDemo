using AzureBlobStorageDemo.Models;
using AzureBlobStorageDemo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AzureBlobStorageDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlobStorageService _blobStorageService;
        public HomeController(BlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        public async Task<IActionResult> Index()
        {
            var files = await _blobStorageService.ListFiles();
            return View(files);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if(file != null && file.Length > 0)
            {
                await _blobStorageService.UploadFileAsync(file);
                TempData["message"] = $"Uploaded {file.FileName}!";
            }
            else
            {
                throw new Exception("Bad file!");
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}