using Client.Application.Features.User.Dtos;
using Client_WebApp.Models.Master;
using Client_WebApp.Services;
using Client_WebApp.Services.Config;
using Client_WebApp.Services.Master;
using Microsoft.AspNetCore.Mvc;

namespace Client_WebApp.Controllers.Master
{
    public class UserController : BaseController
    {
        private readonly UserService _service;
        private readonly RoleService _roleService; 

        public UserController(UserService service, RoleService roleService)
        {
            _service = service;
            _roleService = roleService;
        }

        public async Task<IActionResult> Index(string? searchText = null)
        {
            try
            {
                var companyId = CurrentCompanyId;
                var users = await _service.GetAllUsersAsync(companyId, null, searchText);
                var roles = await _roleService.GetRoleAsync();

                var viewModel = new UserViewModel
                {
                    CompanyId = companyId,
                    Users = users,
                    Roles = roles
                };

                ViewData["searchText"] = searchText;
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to load users. " + ex.Message;
                return View(new UserViewModel());
            }
        }

        //Updated sp_sbs_userMaster_insert sp in insert query replace this "SHA2(p_password, 512)"
        //to "UNHEX(SHA2(p_password, 512))" and also update input password parameter to "IN p_password varbinary(64)"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(User model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Validation failed.";
                return RedirectToAction("Index");
            }

            try
            {
                if (model.Id > 0)
                {
                    var updateDto = new UpdateUserDto
                    {
                        Id = model.Id,
                        RoleMasterId = model.RoleMasterId,
                        Username = model.Username,
                        Email = model.Email,
                        CompanyID = model.CompanyId,
                        UpdatedBy = CurrentUserId
                    };
                    await _service.UpdateUserAsync(updateDto);
                    TempData["SuccessMessage"] = "User updated successfully!";
                }
                else
                {
                    var createDto = new CreateUserDto
                    {
                        RoleMasterId = model.RoleMasterId,
                        Username = model.Username,
                        Email = model.Email,
                        Password = model.Password,
                        CompanyID = model.CompanyId,
                        CreatedBy = CurrentUserId
                    };
                    await _service.CreateUserAsync(createDto);
                    TempData["SuccessMessage"] = "User added successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Operation failed. " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteUserAsync(id, CurrentUserId, CurrentCompanyId);
                TempData["SuccessMessage"] = "User deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to delete user. " + ex.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var users = await _service.GetAllUsersAsync(CurrentCompanyId, id);
                var user = users.FirstOrDefault();
                if (user == null) return NotFound(new { message = "User not found." });

                return Json(new
                {
                    id = user.Id,
                    username = user.Username,
                    email = user.Email,
                    roleMasterId = user.RoleMasterId,
                    companyId = user.CompanyID,
                    isActive = user.isActive
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to fetch user data. " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleUserStatus(int id, int isActive)
        {
            try
            {
                var dto = new ToggleUserActiveDto { Id = id, IsActive = isActive };
                await _service.ToggleActiveAsync(dto, CurrentCompanyId);
                TempData["SuccessMessage"] = isActive == 1 ? "User activated successfully!" : "User deactivated successfully!";

                return Json(new { success = true, message = isActive == 1 ? "User activated successfully!" : "User deactivated successfully!" });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to update user status: " + ex.Message;
                return Json(new { success = false, message = "Failed to update user status: " + ex.Message });
            }
        }
    }
}
