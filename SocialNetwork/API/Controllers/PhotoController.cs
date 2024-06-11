using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class PhotoController : Controller
    {
        private readonly FileService fileService;

        public PhotoController(FileService _fileService)
        {
            fileService = _fileService;
        }

        [HttpGet]
        public IActionResult PhotoInfo(int id)
        {
            var photoInfo = fileService.CreatePhotoInfo(fileService.GetById(id));
            return View(photoInfo);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            fileService.DeletePhoto(id);

            return RedirectToAction("MyProfile", "Profile");
        }

        [HttpPost]
        public async Task<IActionResult> MakeNewAvatar(int id)
        {
            fileService.ChangeUserAvatar(id);

            return RedirectToAction("MyProfile", "Profile");
        }
    }
}