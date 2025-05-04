using KarmaWebMVC.Data;
using KarmaWebMVC.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace KarmaWebMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {

        private readonly KarmaWebMVCContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(KarmaWebMVCContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Roles.OrderByDescending(p => p.Id).ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        // POST: Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var existingRole = await _roleManager.FindByNameAsync(roleViewModel.Name);
                if (existingRole != null)
                {
                    ModelState.AddModelError("Name", "Role already exists.");
                    return View(roleViewModel);
                }

                // Tạo vai trò mới
                var role = new IdentityRole
                {
                    Name = roleViewModel.Name // Gán tên cho vai trò
                };

                // Gọi phương thức tạo vai trò từ RoleManager
                var result = await _roleManager.CreateAsync(role);

                
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index)); // Chuyển hướng về danh sách vai trò
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description); // Thêm lỗi vào ModelState
                }
            }

            return View(roleViewModel); // Trả về view với thông tin lỗi
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound(); // Không tìm thấy ID
            }

            var existingRole = await _roleManager.FindByIdAsync(id);
            if (existingRole == null)
            {
                return NotFound(); // Không tìm thấy vai trò
            }

            var roleViewModel = new RoleViewModel
            {
                Id = existingRole.Id, // Thiết lập Id cho ViewModel
                Name = existingRole.Name // Gán tên vai trò vào ViewModel
            };

            return View(roleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var existingRole = await _roleManager.FindByIdAsync(roleViewModel.Id);
                if (existingRole == null)
                {
                    return NotFound(); // Không tìm thấy vai trò
                }

                existingRole.Name = roleViewModel.Name; // Cập nhật tên vai trò

                var result = await _roleManager.UpdateAsync(existingRole);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index)); // Quay lại danh sách vai trò
                }

                // Hiển thị lỗi nếu có
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Nếu ModelState không hợp lệ, quay lại view với thông tin lỗi
            return View(roleViewModel);
        }
      


        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Tìm vai trò theo Id
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            try
            {
                // Xóa vai trò
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Nếu xóa thất bại, hiển thị lỗi
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }

            // Nếu có lỗi, quay trở lại trang danh sách vai trò
            return RedirectToAction(nameof(Index));
        }

    }
}
