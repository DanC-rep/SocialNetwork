using System.Text.Encodings.Web;
using Logic.Interfaces;
using Logic.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class PasswordService
    {
        private readonly ISendEmail emailSender;
        private readonly UserManager<User> userManager;

        public PasswordService(ISendEmail _emailSender, UserManager<User> usrMgr)
        {
            emailSender = _emailSender;
            userManager = usrMgr;
        }

        public async Task<string> CreatePasswordRestoreToken(User user)
        {
            return await userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task SendEmail(string email, string passResetLink)
        {
            await emailSender.SendEmailAsync(email, "Обновите ваш пароль", $"Обновите ваш пароль <a href='{HtmlEncoder.Default.Encode(passResetLink)}'>перейдя по ссылке</a>", true);
        }

        public async Task<IdentityResult> ResetPassword(User user, string token , string password)
        {
            return await userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<IdentityResult> ChangePassword(User user, string oldPassword, string newPassword)
        {
            return await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }
    }
}