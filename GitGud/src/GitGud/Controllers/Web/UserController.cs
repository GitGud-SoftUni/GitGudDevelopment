using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GitGud.Models;
using GitGud.ViewModels;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using GitGud.Services;
using System.IO;
using Microsoft.AspNetCore.Server.Kestrel.Internal.Http;

namespace GitGud.Controllers.Web
{
    public class UserController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private RoleManager<Role> _roleManager;
        private IGitGudRepository _repository;
        private IUploadService _uploadService;


        //user manager - for creating user, sign in manager - for log in and log out user
        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IGitGudRepository repository, IUploadService uploadService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _repository = repository;
            _uploadService = uploadService;
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
                var user = new User { UserName = model.Username, Email = model.Email };

                var createResult = await _userManager.CreateAsync(user, model.Password);

                if (createResult.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "User").Wait();

                    await _signInManager.SignInAsync(user, false); //signinasync - false means user won't stay logged in after closes browser
                    return RedirectToAction("Index", "App");

                }

                foreach (var error in createResult.Errors)
                {
                    //error will appear also in <span asp-validation-summary="ModelOnly"></span> field
                    ModelState.AddModelError("", error.Description);
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

            ModelState.AddModelError("", "Could not log in. Please try again!");

            return View(model);
        }

        [HttpGet]
        public IActionResult Show(string userName)
        {
            var user = _repository.GetUserByUsername(userName) ?? _repository.GetUserByUsername(this.User.Identity.Name);

            //if (!User.Identity.IsAuthenticated)
            //{
            //    //ToDo display some sort of message???
            //    return RedirectToAction("Login");
            //}

            var songs = _repository.GetAllSongsFromUser(user.UserName).ToList();
            var comments = _repository.GetCommentsFromUser(user.UserName).ToList();
            var favs = _repository.GetUserFavs(user.Id).ToList();

            user.Songs = songs;
            user.Comments = comments;
            user.FavSongs = favs;
            return View(user);

        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var profileViewModel = new ProfileViewModel();
            profileViewModel.FirstName = user.FirstName;
            profileViewModel.LastName = user.LastName;
            profileViewModel.Town = user.Town;
            profileViewModel.Birthday = user.Birthday;

            return View(profileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {

                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Town = model.Town;
                user.Birthday = model.Birthday;
                user.Age = DateTime.Now.Year - model.Birthday.Year;
                await _userManager.UpdateAsync(user);

                return RedirectToAction("Show","User", new { userName = user.UserName});
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeAvatar()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeAvatar(ChangeAvatarViewModel model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var songExtension = Path.GetExtension(model.ImageFile.FileName);
            if (model.ImageFile.Length > 2097152 || (songExtension != ".jpg" && songExtension != ".PNG" && songExtension != ".gif"))
            {
                ModelState.Clear();
                ViewBag.InputFields = "Sorry but the maximum size allowed for an avatar is 2mb and the format must be .jpg .PNG .gif";
                ViewBag.AlertType = "danger";
                return View();
            }


            //delete old avatar
            _repository.DeleteAvatar(user.Id);
            //upload new avatar fileAddress
            _uploadService.UploadAvatar(model.ImageFile, this.User.Identity.Name);

            return RedirectToAction("Show", "User", new { userName = user.UserName });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAvatar(DeleteAvatarViewModel model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            _repository.DeleteAvatar(user.Id);
            return RedirectToAction("Show", "User", new { userName = user.UserName });
        }

    }
}
