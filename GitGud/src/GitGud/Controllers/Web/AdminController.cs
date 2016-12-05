using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using GitGud.Models;
using GitGud.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace GitGud.Controllers.Web
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IGitGudRepository _repository;
        //here to add everything that an admin can do
        //create categories, manage users etc.
        //first to find out how a user can take role=admin???

        public AdminController(IGitGudRepository repository)
        {
            _repository = repository;
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


    }
}
