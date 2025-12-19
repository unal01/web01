using CoreBuilder.Data;
using CoreBuilder.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreBuilder.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUserAsync(string username, string email, string password, string? fullName = null, bool isSuperAdmin = false)
        {
            var passwordHash = HashPassword(password);

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                FullName = fullName,
                IsSuperAdmin = isSuperAdmin,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> AuthenticateAsync(string usernameOrEmail, string password)
        {
            var user = await _context.Users
                .Include(u => u.TenantRoles)
                    .ThenInclude(tr => tr.Tenant)
                .FirstOrDefaultAsync(u => 
                    (u.Username == usernameOrEmail || u.Email == usernameOrEmail) 
                    && u.IsActive);

            if (user == null)
                return null;

            if (!VerifyPassword(password, user.PasswordHash))
                return null;

            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task AssignTenantRoleAsync(Guid userId, Guid tenantId, string role)
        {
            var existingRole = await _context.UserTenantRoles
                .FirstOrDefaultAsync(utr => utr.UserId == userId && utr.TenantId == tenantId);

            if (existingRole != null)
            {
                existingRole.Role = role;
            }
            else
            {
                _context.UserTenantRoles.Add(new UserTenantRole
                {
                    UserId = userId,
                    TenantId = tenantId,
                    Role = role
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveTenantRoleAsync(Guid userId, Guid tenantId)
        {
            var role = await _context.UserTenantRoles
                .FirstOrDefaultAsync(utr => utr.UserId == userId && utr.TenantId == tenantId);

            if (role != null)
            {
                _context.UserTenantRoles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.TenantRoles)
                    .ThenInclude(tr => tr.Tenant)
                .OrderBy(u => u.Username)
                .ToListAsync();
        }

        public async Task<List<UserTenantRole>> GetUserTenantRolesAsync(Guid userId)
        {
            return await _context.UserTenantRoles
                .Include(utr => utr.Tenant)
                .Where(utr => utr.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> HasPermissionAsync(Guid userId, Guid tenantId, string requiredRole)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || !user.IsActive)
                return false;

            if (user.IsSuperAdmin)
                return true;

            var role = await _context.UserTenantRoles
                .FirstOrDefaultAsync(utr => utr.UserId == userId && utr.TenantId == tenantId);

            if (role == null)
                return false;

            return requiredRole switch
            {
                Roles.Viewer => true,
                Roles.Editor => role.Role == Roles.Editor || role.Role == Roles.TenantAdmin,
                Roles.TenantAdmin => role.Role == Roles.TenantAdmin,
                _ => false
            };
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.PasswordHash = HashPassword(newPassword);
            await _context.SaveChangesAsync();
            return true;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            var passwordHash = HashPassword(password);
            return passwordHash == hash;
        }
    }
}
