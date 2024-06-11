using Logic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Logic.ViewModels;
using Application.Services;
using Microsoft.AspNetCore.Identity;
using Application.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserService userService;
        private readonly FileService fileService;

        public ProfileController(UserService _userService, FileService _fileService)
        {
            userService = _userService;
            fileService = _fileService;
        }

        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            User user = await userService.Get(User);

            if (user != null)
            {
                ProfileInfoViewModel profile = userService.GetProfileInfo(user);
                var photoData = fileService.GetUserAvatar(user).Data;

                if (!photoData.IsNullOrEmpty())
                {
                    profile.Avatar = Convert.ToBase64String(photoData);
                }
                else
                {
                    var memoryStream = fileService.OpenDefaultImage();
                    var file = MakeFormFileFromStream(memoryStream);
                    profile.Avatar = Convert.ToBase64String(fileService.ConvertToByteArray(file.OpenReadStream(), file.Length));
                }

                profile.Photos = fileService.GetUserPhotosInfo(user);

                return View(profile);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(string id, string? url = null)
        {
            var user = await userService.GetById(id);

            if (user != null)
            {
                ViewData["returnUrl"] = url;
                return View(userService.CreateEditProfileDTO(user));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.EditProfile(model);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("MyProfile");
                }
                AddErrorsFromResult(result);
            }

            return View(model);
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

            var mimeType = file.ContentType;
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
                    var fileModel = fileService.CreateFileModel(file.FileName, file.Length, mimeType, stream, user, isAvatar);

                    await fileService.SaveFile(fileModel);

                    return RedirectToAction("MyProfile");
                }
            }
            return View();
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        private IFormFile MakeFormFileFromStream(MemoryStream stream)
        {
            return new FormFile(stream, 0, stream.Length, FileSettings.DefaultImageSettings["Name"], FileSettings.DefaultImageSettings["ContentType"]);
        }
    }
}