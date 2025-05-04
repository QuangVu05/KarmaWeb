using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KarmaWebMVC.Data;
using KarmaWebMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using KarmaWebMVC.Models.ViewModels;
using System.Data;
using KarmaWebMVC.Migrations;

namespace KarmaWebMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly KarmaWebMVCContext _context;
       

        public UsersController(KarmaWebMVCContext context, UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;

        }

        // GET: Admin/Users
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserRoleViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    SelectedRole = roles.FirstOrDefault() // Lưu tất cả vai trò vào danh sách
                });
            }

            return View(userRoles); // Trả về danh sách người dùng cùng với vai trò của họ
        }

        //Get Create
        // GET: Admin/Users/Create
        public IActionResult Create()
        {
            var userViewModel = new UserRoleViewModel();
            userViewModel.AvailableRoles = _roleManager.Roles.Select(r => r.Name).ToList(); // Lấy danh sách vai trò có sẵn
            return View(userViewModel); // Trả về view để tạo người dùng mới
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserRoleViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tính duy nhất của email
                var existingUser = await _userManager.FindByEmailAsync(userViewModel.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại."); // Thông báo lỗi nếu email đã có người dùng khác
                    userViewModel.AvailableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                    return View(userViewModel);
                }

                // Tạo người dùng mới
                var user = new AppUserModel
                {
                    UserName = userViewModel.UserName,
                    Email = userViewModel.Email
                };

                var result = await _userManager.CreateAsync(user, "DefaultPassword123!"); // Đặt mật khẩu mặc định

                if (result.Succeeded)
                {
                    // Thêm vai trò cho người dùng mới
                    if (!string.IsNullOrEmpty(userViewModel.SelectedRole))
                    {
                        await _userManager.AddToRoleAsync(user, userViewModel.SelectedRole);
                    }
                    return RedirectToAction(nameof(Index)); // Chuyển hướng về danh sách người dùng
                }

                // Nếu có lỗi trong quá trình tạo người dùng, thêm vào ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Nếu có lỗi, giữ lại các giá trị nhập vào
            userViewModel.AvailableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return View(userViewModel); // Trả về view với userViewModel
        }



        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy người dùng
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var userViewModel = new UserRoleViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                SelectedRole = userRoles.FirstOrDefault() // Lưu tất cả vai trò đã gán
            };

            // Lấy tất cả vai trò từ cơ sở dữ liệu
            var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            userViewModel.AvailableRoles = allRoles; // Lưu các vai trò có sẵn cho dropdown

            return View(userViewModel); // Trả về view với userViewModel
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserRoleViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(userViewModel.Id);
                if (user == null)
                {
                    return NotFound(); // Trả về 404 nếu không tìm thấy người dùng
                }

                // Kiểm tra tính duy nhất của email
                var existingUser = await _userManager.FindByEmailAsync(userViewModel.Email);
                if (existingUser != null && existingUser.Id != userViewModel.Id)
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại."); // Thông báo lỗi nếu email đã có người dùng khác
                    userViewModel.AvailableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                    return View(userViewModel);
                }

                // Cập nhật thông tin người dùng
                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email; // Cập nhật email nếu cần thiết

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    // Cập nhật các vai trò của người dùng
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, currentRoles); // Xóa các vai trò hiện tại

                    // Thêm vai trò đã chọn
                    if (!string.IsNullOrEmpty(userViewModel.SelectedRole))
                    {
                        await _userManager.AddToRoleAsync(user, userViewModel.SelectedRole); // Thêm vai trò mới
                    }

                    return RedirectToAction(nameof(Index)); // Chuyển hướng về danh sách người dùng
                }

                // Nếu có lỗi trong quá trình cập nhật, thêm vào ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Nếu có lỗi, giữ lại các giá trị nhập vào
            userViewModel.AvailableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return View(userViewModel); // Trả về view với userViewModel
        }
        public async Task<IActionResult> Delete(string id)
        {
            // Tìm người dùng theo Id
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy người dùng
            }

            // Thực hiện xóa người dùng
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                // Nếu xóa thành công, chuyển hướng về trang danh sách người dùng
                TempData["SuccessMessage"] = "Xóa người dùng thành công.";
                return RedirectToAction("Index");
            }

            // Nếu xóa không thành công, thêm lỗi vào TempData để hiển thị trên giao diện
            TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa người dùng.";
            foreach (var error in result.Errors)
            {
                TempData["ErrorMessage"] += $" {error.Description}";
            }

            // Chuyển hướng về trang danh sách người dùng và hiển thị thông báo lỗi
            return RedirectToAction("Index");
        }
    }
}


