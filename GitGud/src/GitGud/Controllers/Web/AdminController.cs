using System.Collections.Generic;
using System.Linq;
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
            return View(allUsers);
        }

        public IActionResult DeleteUser(string userId)
        {
            _repository.DeleteUser(userId);
            return RedirectToAction("AllUsers");
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
