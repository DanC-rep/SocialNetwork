using Application.Options;
using Application.Services;
using Logic.Models;
using Logic.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Authorize]
    public class PhotoController : Controller
    {
        private readonly FileService fileService;

        private readonly UserService userService;

        public PhotoController(FileService _fileService, UserService usrService)
        {
            fileService = _fileService;
            userService = usrService;
        }

        [HttpGet]
        public async Task<IActionResult> PhotoInfo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await userService.Get(User);
            var photo = fileService.GetById((int)id);

            if (user.Id != photo.UserId)
            {
                return RedirectToAction("Index", "Home");
            }
            
            var photoInfo = fileService.CreatePhotoInfo(photo);
            return View(photoInfo);
        }

        [HttpPost]
        public IActionResult DeletePhoto(int id)
        {
            fileService.DeletePhoto(id);

            return RedirectToAction("MyProfile", "Profile");
        }

        [HttpPost]
        public IActionResult MakeNewAvatar(int id)
        {
            fileService.ChangeUserAvatar(id);

            return RedirectToAction("MyProfile", "Profile");
        }

        [HttpGet]
        public IActionResult FileUpload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FileUpload(IFormFile file, string? _isAvatar = null)
        {
            if (file != null && file.Length > FileSettings.MaxFileUploadSizeMb * 1000000)
            {
                ModelState.AddModelError("", $"Размер файла не может превышать {FileSettings.MaxFileUploadSizeMb} МБ");
            }

            var extension = fileService.GetFileExtension(file?.FileName ?? "");

            if (extension.IsNullOrEmpty() || !FileSettings.PermittedExtensions.Contains(extension))
            {
                ModelState.AddModelError("", "Неправильный тип файла");
            }

            var mimeType = file?.ContentType;
            if (!FileSettings.PermittedMimeTypes.Contains(mimeType))
            {
                ModelState.AddModelError("", "Неверный тип MIME");
            }

            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    bool isAvatar = _isAvatar.IsNullOrEmpty() ? false : true;
                    var stream = file.OpenReadStream();
                    User user = await userService.Get(User);
                    var fileModel = fileService.CreateFileModel(file.FileName, file.Length, file.ContentType, stream, user, isAvatar);

                    fileService.SaveFile(fileModel);

                    return RedirectToAction("MyProfile");
                }
            }
            return View();
        }
    }
}