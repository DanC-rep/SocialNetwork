using Logic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Logic.ViewModels;
using Application.Services;
using Microsoft.AspNetCore.Identity;
using Application.Options;
using Microsoft.IdentityModel.Tokens;
using Logic.Enums;

namespace API.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserService userService;
        private readonly FileService fileService;
        private readonly NotificationsService notificationsService;
        private readonly FriendsService friendsService;

        public ProfileController(UserService _userService, FileService _fileService, NotificationsService _notificationsService, FriendsService _friendsService)
        {
            userService = _userService;
            fileService = _fileService;
            notificationsService = _notificationsService;
            friendsService = _friendsService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ProfileInfo(string? id = null)
        {
            bool isOwnPage = id.IsNullOrEmpty();

            User user = await (isOwnPage ? userService.Get(User) : userService.GetById(id));

            if (user != null)
            {
                ProfileInfoViewModel profile = userService.GetProfileInfo(user);
                await SetProfilePhotos(user, profile);

                if ((User.Identity.IsAuthenticated && user.Email == User.Identity.Name) || isOwnPage)
                {
                    return View("MyProfile", profile);
                }

                if (User.Identity.IsAuthenticated)
                {
                    User ownUser = await userService.Get(User);
                    var relationType = friendsService.GetRelationType(user, ownUser);
                    profile.RelationType = relationType;
                }

                return View("UserProfile", profile);
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
        public async Task<IActionResult> Notifications()
        {
            User user = await userService.Get(User);
            
            if (user != null)
            {
                return View(notificationsService.GetUserNotifications(user.Id));
            }
            
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNotification(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await userService.Get(User);
            var notification = notificationsService.GetById((int)id);

            if (user.Id != notification.ReceiverId)
            {
                return NotFound();
            }

            notificationsService.Delete((int)id);
            return RedirectToAction("Notifications");
        }

        [HttpPost]
        public async Task<IActionResult> SendFriendRequest(string id)
        {
            User receiver = await userService.GetById(id);

            if (receiver != null)
            {
                User sender = await userService.Get(User);

                notificationsService.SendFriendRequest(sender, receiver);
                friendsService.SendFriendRequest(sender, receiver);

                return RedirectToAction("ProfileInfo", "Profile", new { id = id });
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CancelSubscription(string id)
        {
            User receiver = await userService.GetById(id);

            if (receiver != null)
            {
                User sender = await userService.Get(User);

                friendsService.CancelSubscribtion(sender, receiver);
                notificationsService.Delete(sender, receiver, NotificationType.FriendRequest);

                return RedirectToAction("ProfileInfo", "Profile", new { id = id });
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromFriends(string id, string? returnUrl)
        {
            User receiver = await userService.GetById(id);

            if (receiver != null)
            {
                User sender = await userService.Get(User);

                friendsService.RemoveFromFriends(sender, receiver);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("RelationList", new { id = sender.Id, relationType = RelationType.Friend });
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddToFriends(string id, string? returnUrl)
        {
            var receiver = await userService.GetById(id);

            if (receiver != null)
            {
                User sender = await userService.Get(User);

                friendsService.AddToFriends(sender, receiver);

                notificationsService.Delete(receiver, sender, NotificationType.FriendRequest);
                notificationsService.ApproveFriendRequest(sender, receiver);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("RelationList", new { id = sender.Id, relationType = RelationType.Friend });
            }

            return NotFound();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RelationList(string id, RelationType relationType)
        {
            var relations = friendsService.GetFriendsByRelation(id, relationType);
            var users = new List<User>();

            foreach (var relation in relations)
            {
                users.Add(await userService.GetById(relation.FriendId));
            }

            var profiles = users.Select(u => userService.GetProfileInfo(u)).ToList();

            if (User.Identity.IsAuthenticated)
            {
                var ownUser = await userService.Get(User);

                if (ownUser.Id == id)
                {
                    profiles.ForEach(p => p.RelationType = relationType);
                }
            }

            return View(profiles);
        }


        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        private async Task SetProfilePhotos(User user, ProfileInfoViewModel profile)
        {
            var photoData = fileService.GetUserAvatar(user).Data;

            if (!photoData.IsNullOrEmpty())
            {
                profile.Avatar = Convert.ToBase64String(photoData);
            }
            else
            {
                var memoryStream = fileService.OpenDefaultImage();
                var file = new FormFile(memoryStream, 0, memoryStream.Length, FileSettings.DefaultImageSettings["Name"], FileSettings.DefaultImageSettings["ContentType"]);
                profile.Avatar = Convert.ToBase64String(fileService.ConvertToByteArray(file.OpenReadStream(), file.Length));
            }

            profile.Photos = await fileService.GetUserPhotosInfo(user);
        }
    }
}