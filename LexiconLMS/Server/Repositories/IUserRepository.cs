﻿using LexiconLMS.Domain.Entities;
using LexiconLMS.Shared.Dtos;
using Microsoft.AspNetCore.Identity;

namespace LexiconLMS.Server.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetParticipantsAsync(Guid courseId);
        Task<IEnumerable<IdentityRole>> GetRolesAsync();
        Task<IdentityRole> GetRoleAsync(Guid id);
        Task<ApplicationUser> CreateUserAsync(ApplicationUser user, IdentityRole identityRole);
        Task<ApplicationUser> GetUserAsync(Guid id);
        Task<ApplicationUser> GetUserAsync(string UserName);
        Task UpdateUserAsync(ApplicationUser user);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<bool> CourseExistAsync(Guid courseId);

    }
}