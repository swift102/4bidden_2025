using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public ProfileController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        [Route("GetProfile")]
        public async Task<ActionResult<ApiResponse<UserProfileDto>>> GetProfile()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse<UserProfileDto>
                    {
                        Success = false,
                        Message = "User not found in token",
                        Errors = new List<string> { "Invalid authentication token" }
                    });
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new ApiResponse<UserProfileDto>
                    {
                        Success = false,
                        Message = "User not found",
                        Errors = new List<string> { "User does not exist" }
                    });
                }

                var profileDto = new UserProfileDto
                {
                    Id = user.Id,
                    FullName = user.Name,
                    Email = user.Email!,
                    UserName = user.UserName!,
                    NotificationsEnabled = user.NotificationsEnabled,
                    Theme = user.Theme,
                    NotificationThreshold = user.NotificationThreshold,
                    TimeZone = user.TimeZone,
                    EmailNotifications = user.EmailNotifications,
                    SmsNotifications = user.SmsNotifications,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                };

                return Ok(new ApiResponse<UserProfileDto>
                {
                    Success = true,
                    Message = "Profile retrieved successfully",
                    Data = profileDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<UserProfileDto>
                {
                    Success = false,
                    Message = "An error occurred while retrieving profile",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut]
        [Route("UpdateProfile")]
        public async Task<ActionResult<ApiResponse<UserProfileDto>>> UpdateProfile(UpdateProfileDto model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse<UserProfileDto>
                    {
                        Success = false,
                        Message = "User not found in token"
                    });
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new ApiResponse<UserProfileDto>
                    {
                        Success = false,
                        Message = "User not found"
                    });
                }

                // Check if email is being changed and if it's already taken
                if (user.Email != model.Email)
                {
                    var existingUser = await _userManager.FindByEmailAsync(model.Email);
                    if (existingUser != null && existingUser.Id != userId)
                    {
                        return BadRequest(new ApiResponse<UserProfileDto>
                        {
                            Success = false,
                            Message = "Email is already taken",
                            Errors = new List<string> { "This email is already registered to another user" }
                        });
                    }
                }

                // Update user properties
                user.Name = model.Name;
                user.Email = model.Email;
                user.UserName = model.Email; 
                user.NotificationsEnabled = model.NotificationsEnabled;
                user.Theme = model.Theme;
                user.NotificationThreshold = model.NotificationThreshold;
                user.TimeZone = model.TimeZone;
                user.EmailNotifications = model.EmailNotifications;
                user.SmsNotifications = model.SmsNotifications;
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(new ApiResponse<UserProfileDto>
                    {
                        Success = false,
                        Message = "Failed to update profile",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    });
                }

                var updatedProfileDto = new UserProfileDto
                {
                    Id = user.Id,
                    FullName = user.Name,
                    Email = user.Email!,
                    UserName = user.UserName!,
                    NotificationsEnabled = user.NotificationsEnabled,
                    Theme = user.Theme,
                    NotificationThreshold = user.NotificationThreshold,
                    TimeZone = user.TimeZone,
                    EmailNotifications = user.EmailNotifications,
                    SmsNotifications = user.SmsNotifications,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                };

                return Ok(new ApiResponse<UserProfileDto>
                {
                    Success = true,
                    Message = "Profile updated successfully",
                    Data = updatedProfileDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<UserProfileDto>
                {
                    Success = false,
                    Message = "An error occurred while updating profile",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost("change-password")]
        public async Task<ActionResult<ApiResponse<object>>> ChangePassword(ChangePasswordDto model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "User not found in token"
                    });
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "User not found"
                    });
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Failed to change password",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    });
                }

                // Update the UpdatedAt timestamp
                user.UpdatedAt = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Password changed successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while changing password",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete]
        [Route("DeleteAccount")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteAccount()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "User not found in token"
                    });
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "User not found"
                    });
                }

                // You might want to soft delete or clean up related data first
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Failed to delete account",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Account deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while deleting account",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}