using System.Text.Encodings.Web;
using Logic.Interfaces;
using Logic.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class EmailService
    {
        private readonly ISendEmail emailSender;
        private readonly UserManager<User> userManager;

        public EmailService(ISendEmail _emailSender, UserManager<User> usrMgr)
        {
            emailSender = _emailSender;
            userManager = usrMgr;
        }

        public async Task<string> CreateConfirmToken(User user)
        {
            return await userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task SendEmail(string email, string confirmLink)
        {
            await emailSender.SendEmailAsync(email, "Подтвердите ваш Email", $"Подтвердите ваш Email <a href='{HtmlEncoder.Default.Encode(confirmLink)}'>перейля по ссылке</a>", true);
        }

        public async Task<IdentityResult> ConfirmEmail(User user, string token)
        {
            return await userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<bool> CheckConfirmedEmail(User user)
        {
            return await userManager.IsEmailConfirmedAsync(user);
        }

        public async Task SendAccountLockedEmail(string email)
        {
            await emailSender.SendEmailAsync(email, "Аккаунт заблокирован", "Ваш аккаунт был заблокирован из-за многочисленных попыток входа", false);
        }
    }
}