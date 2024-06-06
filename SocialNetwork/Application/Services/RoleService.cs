using Logic.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleService(RoleManager<IdentityRole> roleMgr)
        {
            roleManager = roleMgr;
        }

        public async Task<bool> RoleExists(string role) => await roleManager.RoleExistsAsync(role);

        public async Task<IdentityResult> AddRole(CreateRoleViewModel model)
        {
            IdentityRole role = new IdentityRole
            {
                Name = model.Role
            };

            return await roleManager.CreateAsync(role);
        }

        public IEnumerable<IdentityRole> GetAllRoles()
        {
            return roleManager.Roles.ToList();
        }

        public async Task<IdentityResult> DeleteRole(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            return await roleManager.DeleteAsync(role);
        }
    }
}
