using KarmaWebMVC.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using KarmaWebMVC.Models;
using Microsoft.AspNetCore.Identity;
using KarmaWebMVC.Models.ViewModels;
using KarmaWebMVC.Repository;

using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity.UI.Services;
using NuGet.Common;

namespace KarmaWebMVC.Controllers
{

    public class AccountController : Controller
    {
        private UserManager<AppUserModel> _userManager;
        private SignInManager<AppUserModel> _signInManager;
        private readonly Repository.IEmailSender _emailSender;
        private readonly KarmaWebMVCContext _context;

        public AccountController(Repository.IEmailSender emailSender, UserManager<AppUserModel> userManager, SignInManager<AppUserModel> signInManager, KarmaWebMVCContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _context = context;
        }
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }
        public IActionResult Login(string returnUrl)
        {

            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        public async Task<IActionResult> NewPass(AppUserModel user, string token)
        {
            var checkuser = await _userManager.Users
                .Where(u => u.Email == user.Email && u.Token == user.Token)
                .FirstOrDefaultAsync();


            if (checkuser != null)
            {
                ViewBag.Email = checkuser.Email;
                ViewBag.Token = token;
            }
            else
            {
                TempData["error"] = "Email not found or token is not right";
                return RedirectToAction("ForgotPass", "Account");
            }

            return View();


        }
        public async Task<IActionResult> UpdateNewPassword(AppUserModel user, string token)
        {
            var checkuser = await _userManager.Users
                .Where(u => u.Email == user.Email)
                .Where(u => u.Token == user.Token)
                .FirstOrDefaultAsync();

            if (checkuser != null)
            {
                // Update user with new password and token
                string newtoken = Guid.NewGuid().ToString();

                // Hash the new password
                var passwordHasher = new PasswordHasher<AppUserModel>();
                var passwordHash = passwordHasher.HashPassword(checkuser, user.PasswordHash);
                checkuser.PasswordHash = passwordHash;

                // Update token
                checkuser.Token = newtoken;

                await _userManager.UpdateAsync(checkuser);
                TempData["success"] = "Password updated successfully.";
                return RedirectToAction("Login", "Account");
            }
            else
            {
                TempData["error"] = "Email not found or token is not right";
                return RedirectToAction("ForgetPass", "Account");
            }

            
        }
        public IActionResult ForgotPass()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendEmailForgotPass(AppUserModel user)
        {
            var checkMail = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (checkMail == null)
            {
                // Nếu không tìm thấy, thông báo lỗi
                TempData["error"] = "Email not found";
                return RedirectToAction("ForgotPass", "Account");
            }
            else
            {
                // Tạo token mới và cập nhật cho người dùng
                string token = Guid.NewGuid().ToString();
                checkMail.Token = token;
                _context.Update(checkMail);
                await _context.SaveChangesAsync();

                // Tạo email để gửi
                var receiver = checkMail.Email;
                var subject = "Change password for user " + checkMail.Email;
                var message = "Click on link to change password " +
              "<a href='" + $"{Request.Scheme}://{Request.Host}/Account/NewPass?email={checkMail.Email}&token={token}'>Click here</a>";

                try
                {
                    await _emailSender.SendEmailAsync(receiver, subject, message);
                    TempData["success"] = "An email has been sent to your registered email address with password reset link";
                }
                catch (Exception ex)
                {
                    TempData["error"] = "There was an error sending the email: " + ex.Message;
                    // Ghi log lỗi nếu cần
                }
                return RedirectToAction("ForgotPass", "Account");
            }
        }

        //Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginView)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra thông tin đăng nhập bằng SignInManager
                var result = await _signInManager.PasswordSignInAsync(loginView.UserName, loginView.Password, false, false);

                if (result.Succeeded)
                {

                    return Redirect(loginView.ReturnUrl ?? "/");
                }

                // Thêm lỗi nếu đăng nhập thất bại
                ModelState.AddModelError("", "Invalid username or password. Please try again.");
            }

            // Trả về View với thông tin model và các lỗi (không dùng RedirectToAction)
            return View(loginView);
        }
        public IActionResult Create(string email = null)
        {
            var model = new UserModel
            {
                Email = email // Điền email vào model
            };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                // Check if the email already exists
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email already exists."); // Add error if email is taken
                    return View(user); // Return view with the model containing error
                }
                AppUserModel newModel = new AppUserModel { UserName = user.UserName, Email = user.Email };
                IdentityResult result = await _userManager.CreateAsync(newModel, user.Password);
                if (result.Succeeded)
                {
                    ViewBag.RegisterStatus = "Registration successful! Please log in.";

                    return View(user);

                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            return View(user);

        }
        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
        // Phương thức xử lý callback sau khi đăng nhập bên ngoài
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            // Lấy thông tin người dùng từ claims
            var claimsIdentity = result.Principal.Identity as ClaimsIdentity;

            // Kiểm tra xem người dùng đã tồn tại trong cơ sở dữ liệu chưa
            var emailClaim = claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(emailClaim);

            if (user == null)
            {
                // Lưu thông báo vào TempData
                TempData["Message"] = "You have successfully authenticated with Google, please register an account.";
                // Chuyển hướng đến trang đăng ký và truyền email
                return RedirectToAction("Create", "Account", new { email = emailClaim });
            }
            else
            {
                // Nếu người dùng đã tồn tại, đăng nhập
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            return LocalRedirect(returnUrl ?? "/"); // Chuyển hướng đến URL đã định
        }
    }

}





