﻿using LexiconLMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LexiconLMS.Server.Data
{
    public class SeedData
    {
        private static UserManager<ApplicationUser> _userManager = default!;
        private static RoleManager<IdentityRole> _roleManager = default!;

        public static async Task InitAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            if (await context.Course.AnyAsync()) return;

            var activityTypes = AddActivityType();
            await context.AddRangeAsync(activityTypes);
            var courses = AddCourses();
            await context.AddRangeAsync(courses);
            var modules = AddModules(courses);
            await context.AddRangeAsync(modules);
            var activities = AddActivities(activityTypes, modules);
            await context.AddRangeAsync(activities);

            await context.SaveChangesAsync();

            if (context.Roles.Any()) return;

            _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "Admin", "Student" };
            await AddRolesAsync(roles);

            var users = AddUsers(courses).ToList();

            var adminPassword = "P@ssw0rd";
            //var adminCourseId = new Guid();

            var studentPassword = "P@ssw0rd";
            // var studentCourseId = new Guid();

            var admin = await AddAccountAsync(users[0], adminPassword);
            var user = await AddAccountAsync(users[1], studentPassword);

            await AddUserToRoleAsync(admin, roles[0]);
            await AddUserToRoleAsync(user, roles[1]);
        }

        private static IEnumerable<ActivityType> AddActivityType()
        {
            var activityTypes = new List<ActivityType>
            {
                new ActivityType
                {
                    Name = "Assignment"
                },
                new ActivityType
                {
                    Name = "E-learning"
                },
                new ActivityType
                {
                    Name = "Lecture"

                },
                new ActivityType
                {
                    Name = "Practice"
                },
            };
            return activityTypes;
        }

        private static IEnumerable<Course> AddCourses()
        {
            var courses = new List<Course>
            {
                new Course
                {
                    Name = ".Net",
                    StartDate = new DateTime(2023,12,30, 14,30,0),
                    EndDate = new DateTime(2024,01,15, 14,30,0),
                    Description = "C# Object Oriented"
                },
                new Course
                {
                    Name = "Java",
                    StartDate = new DateTime(2024,01,30, 14,30,0),
                    EndDate = new DateTime(2024,02,20, 14,30,0),
                    Description = "Spring Boot"
                },
            };

            return courses;
        }

        private static IEnumerable<Activity> AddActivities(IEnumerable<ActivityType> activityTypes, IEnumerable<Module> modules)
        {
            var activities = new List<Activity>
            {
                new Activity
                {
                    Name = "Assignment OOD",
                    Description = "Learn about inheritence and polymorphism",
                    StartTime = new DateTime(2023, 11, 11, 13, 0, 0),
                    EndTime =new DateTime(2023, 11, 18, 13, 0, 0)
                },
                new Activity
                {
                    Name = "Project Blazor",
                    Description = "Group project using Blazor",
                    StartTime = new DateTime(2023, 12, 20, 13, 0, 0),
                    EndTime =new DateTime(2024, 01, 12, 1, 0, 0)
                }

            };

            var activityTypesList = activityTypes.ToList();
            var moduleList = modules.ToList();

            for (int i = 0; i < activities.Count; i++)
            {
                activities[i].ActivityTypeId = activityTypesList[i].Id;
                activities[i].ModuleId = moduleList[i].Id;
            }

            return activities;
        }

        private static IEnumerable<Module> AddModules(IEnumerable<Course> courses)
        {
            var modules = new List<Module>
            {
                new Module
                {
                    Name = "C# Basic",
                    StartDate = new DateTime(2023, 12, 30, 14, 30, 0),
                    EndDate = new DateTime(2024, 01, 04, 14, 30, 0),
                    Description = "C# For Beginners"
                },
                new Module
                {
                    Name = "Java Basic",
                    StartDate = new DateTime(2024, 01, 30, 14, 30, 0),
                    EndDate = new DateTime(2024, 01, 15, 14, 30, 0),
                    Description = "Java For Beginners"
                },
            };

            var coursesList = courses.ToList();

            for (int i = 0; i < modules.Count; i++)
            {
                modules[i].CourseId = coursesList[i].Id;
            }

            return modules;
        }

        private static IEnumerable<ApplicationUser> AddUsers(IEnumerable<Course> courses)
        {
            var roles = new[] { "Admin", "Student" };

            var users = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Email = "admin@lms.com",
                    FirstName = "Jane",
                    LastName = "Doe"
                },
                new ApplicationUser()
                {
                    Email = "student@lms.com",
                    FirstName = "John",
                    LastName = "Smith"
                }
            };

            var coursesList = courses.ToList();

            for (int i = 0; i < users.Count; i++)
            {
                users[i].CourseId = coursesList[i].Id;
            }

            return users;
        }

        private static async Task AddRolesAsync(string[] roles)
        {
            foreach (var roleName in roles)
            {
                if (await _roleManager.RoleExistsAsync(roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = await _roleManager.CreateAsync(role);

                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        private static async Task<ApplicationUser?> AddAccountAsync(ApplicationUser userToAdd, string password)
        {
            var user = await _userManager.FindByEmailAsync(userToAdd.Email);
            if (user != null) return null;

            var newUser = new ApplicationUser
            {
                UserName = userToAdd.Email,
                FirstName = userToAdd.FirstName,
                LastName = userToAdd.LastName,
                Email = userToAdd.Email,
                CourseId = userToAdd.CourseId,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newUser, password);

            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

            return newUser;
        }

        //private static async Task AddUserToRoleAsync(ApplicationUser user, string roleName)
        //{
        //    if (!await _userManager.IsInRoleAsync(user, roleName))
        //    {
        //        var result = await _userManager.AddToRoleAsync(user, roleName);

        //        if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
        //    }

        //}

        private static async Task AddUserToRoleAsync(ApplicationUser user, string roleName)
        {
            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);

                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
            // Add a role claim for the assigned role
            var roleClaim = new Claim(ClaimTypes.Role, roleName);
             var addClaimResult = await _userManager.AddClaimAsync(user, roleClaim);
             if (!addClaimResult.Succeeded) throw new Exception(string.Join("\n", addClaimResult.Errors));     
            
             // If the assigned role is "Admin," you may want to perform additional logic here.    
            // For example, assigning specific claims, setting up additional permissions, etc.
             if (roleName == "Admin")     
            {         
             //Perform additional operations for the "Admin" role assignment if needed.   
                       
            } 
        }
        }
    }

