using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GitGud.Models;
using GitGud.ViewModels;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace GitGud.Controllers.Web
{
    public class UserController: Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private RoleManager<Role> _roleManager;
        private IGitGudRepository _repository;


        //user manager - for creating user, sign in manager - for log in and log out user
        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IGitGudRepository repository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User {UserName = model.Username, Email = model.Email};

                var createResult = await _userManager.CreateAsync(user, model.Password);

                if (createResult.Succeeded)
                {
                   _userManager.AddToRoleAsync(user,"User").Wait();

                   await _signInManager.SignInAsync(user, false); //signinasync - false means user won't stay logged in after closes browser
                   return RedirectToAction("Index", "App");
                   
                }

                foreach (var error in createResult.Errors)
                {
                        //error will appear also in <span asp-validation-summary="ModelOnly"></span> field
                        ModelState.AddModelError("",error.Description);
                }

            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "App");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginResult = await _signInManager.PasswordSignInAsync(model.Username, 
                                                                            model.Password, 
                                                                            model.RememberMe, 
                                                                            false);
                //if login succeeds user will be returned to the address he was 
                //trying to reach if it is local url and to index if it's not
                if (loginResult.Succeeded)
                {
                    
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return RedirectToAction("Index", "App");
                }
            }

            ModelState.AddModelError("","Could not log in. Please try again!");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Show()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var songs = _repository.GetAllSongsFromUser(user.UserName).ToList();
            user.Songs = songs;
            return View(user);

        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

       
    }
}
