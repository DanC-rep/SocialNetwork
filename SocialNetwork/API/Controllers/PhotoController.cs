using System.Web;
using Application.Options;
using Application.Services;
using Logic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Authorize]
    public class PhotoController : Controller
    {
        private readonly FileService fileService;
        private readonly ReactionsService reactionsService;
        private readonly UserService userService;
        private readonly NotificationsService notificationService;
        private readonly CommentsService commentsService;

        public PhotoController(FileService _fileService, UserService usrService, ReactionsService _reactionService,
        NotificationsService _notificationService, CommentsService _commentsService)
        {
            fileService = _fileService;
            userService = usrService;
            reactionsService = _reactionService;
            notificationService = _notificationService;
            commentsService = _commentsService;
        }

        [HttpGet]
        public async Task<IActionResult> PhotoInfo(int? photoId, string? userId)
        {
            if (photoId == null || userId == null)
            {
                return NotFound();
            }

            var user = await userService.GetById(userId);
            var ownUser = await userService.Get(User);

            var photo = fileService.GetById((int)photoId);

            if (user.Id != photo.UserId)
            {
                return RedirectToAction("Index", "Home");
            }

            var photoInfo = await fileService.CreatePhotoInfo(photo);

            if (user.Id == ownUser.Id)
            {
                return View("MyPhotoInfo", photoInfo);
            }

            return View("UserPhotoInfo", photoInfo);
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

        [HttpPost]
        public async Task<IActionResult> MakeReaction(int? photoId, int? reactionId, string? userId)
        {
            if (photoId == null || reactionId == null || userId.IsNullOrEmpty())
            {
                return NotFound();
            }

            var ownUser = await userService.Get(User);
            var user = await userService.GetById(userId);

            var photo = fileService.GetById((int)photoId);
            var reaction = reactionsService.GetReactionById((int)reactionId);

            bool sendNotification = reactionsService.MakeReaction(photo, reaction, ownUser);

            if (sendNotification && user.Id != ownUser.Id)
            {
                notificationService.MakeReactionToPhoto(ownUser, user, photo, reactionsService.GetLocalizedReactionType(reaction.ReactionType));
            }

            return RedirectToAction("PhotoInfo", new { photoId = photoId, userId = userId });
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(string? commentText, int photoId, string userId)
        {
            if (commentText.IsNullOrEmpty())
            {
                return BadRequest();
            }

            var ownUser = await userService.Get(User);
            var photo = fileService.GetById(photoId);
            commentsService.AddCommentToPhoto(commentText, ownUser, photo);

            return RedirectToAction("PhotoInfo", new { userId = userId, photoId = photoId });
        }
    }
}