using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using GitGud.Models;
using GitGud.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace GitGud.Controllers.Web
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IGitGudRepository _repository;
        private UserManager<User> _userManager;
        //here to add everything that an admin can do
        //create categories, manage users etc.
        //first to find out how a user can take role=admin???

        public AdminController(IGitGudRepository repository, UserManager<User> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public IActionResult AllUsers()
        {
            List<User> allUsers = _repository.GetAllUsers().ToList();

            ViewBag.Admins = _repository.GetAllAdmins();

            return View(allUsers);
        }

        public IActionResult DeleteUserConfirmation(string userId)
        {
            User user = _repository.GetUserById(userId);

            if (user == null)
            {
                return RedirectToAction("AllUsers");
            }

            return View(user);
        }

        public IActionResult DeleteUser(string userId)
        {
            var currentLoggedInUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //Check if current logged in user is same as user to be deleted
            if (userId != currentLoggedInUserId)
            {
                _repository.DeleteUser(userId);
            }
            else
            {
                return RedirectToAction("DeleteUserError");
            }

            return RedirectToAction("AllUsers");
        }

        public IActionResult DeleteUserError()
        {
            return View();
        }

        public IActionResult EditUser(string userId)
        {
            var user = _repository.GetUserById(userId);

            var editUserViewModel = new EditUserViewModel();
            editUserViewModel.User = user;
            editUserViewModel.Roles = GetUserRoles(user);

            return View(editUserViewModel);
        }

        [HttpPost]
        public IActionResult EditUser(string userId, EditUserViewModel editUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _repository.GetUserById(userId);

                if (!string.IsNullOrEmpty(editUserViewModel.Password))
                {
                    var passwordHasher = new PasswordHasher<User>();
                    var newPasswordHash = passwordHasher.HashPassword(user, editUserViewModel.Password);
                    user.PasswordHash = newPasswordHash;
                }

                SetUserRoles(editUserViewModel, user);
                _repository.SaveChangesInDb();
            }

            return RedirectToAction("AllUsers");
        }

        private void SetUserRoles(EditUserViewModel editUserViewModel, User user)
        {
            foreach (var role in editUserViewModel.Roles)
            {
                var isUserInRole = _userManager.IsInRoleAsync(user, role.Name).Result;

                if (role.IsSelected && !isUserInRole)
                {
                    var isUserAddedToRole = _userManager.AddToRoleAsync(user, role.Name).Result;
                }
                else if (!role.IsSelected && isUserInRole)
                {
                    var isUserRemovedFromRole = _userManager.RemoveFromRoleAsync(user, role.Name).Result;
                }
            }
        }

        private List<CheckUserRolesViewModel> GetUserRoles(User user)
        {
            var allRoles = _repository.GetAllRoles();

            var userRoles = new List<CheckUserRolesViewModel>();

            foreach (var role in allRoles)
            {
                CheckUserRolesViewModel checkUserRolesViewModel = new CheckUserRolesViewModel();
                checkUserRolesViewModel.Name = role.Name;

                if (role.Users.Any(u => u.UserId == user.Id))
                {
                    checkUserRolesViewModel.IsSelected = true;
                }

                userRoles.Add(checkUserRolesViewModel);
            }

            return userRoles;
        }

        public IActionResult AllCategories()
        {
            var allCategories = _repository.GetAllCategories();
            return View(allCategories);
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        public IActionResult AddCreatedCategory(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                _repository.AddCategory(categoryViewModel.CategoryName);
            }

            return RedirectToAction("AllCategories");
        }

        [HttpGet]
        public IActionResult DeleteCategory(string strCategoryId)
        {
            int categoryId = int.Parse(strCategoryId);
            Category category = _repository.SearchCategoryById(categoryId);

            if (category == null)
            {
                return RedirectToAction("AllCategories", "Admin");
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult DeleteCategory(int categoryId)
        {
            _repository.DeleteCategory(categoryId);

            return RedirectToAction("AllCategories");
        }

        [HttpGet]
        public IActionResult EditCategory(string strCategoryId)
        {
            int categoryId = int.Parse(strCategoryId);
            Category category = _repository.SearchCategoryById(categoryId);

            if (category != null)
            {
                ViewData["categoryName"] = category.Name;
                ViewData["categoryId"] = category.Id;
            }

            return View();
        }

        [HttpPost]
        public IActionResult EditCategory(int categoryId, CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                string newCategoryName = categoryViewModel.CategoryName;
                Category category = _repository.SearchCategoryById(categoryId);

                if (category != null)
                {
                    _repository.EditCategory(newCategoryName, category);
                }
            }

            return RedirectToAction("AllCategories");
        }

        public IActionResult ShowUser(string userId)
        {
            var user = _repository.GetUserById(userId);

            var model = new ShowUserViewModel();
            model.User = user;
            return View(model);
        }
    }
}
