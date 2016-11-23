using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitGud.Models;
using Microsoft.AspNetCore.Mvc;

namespace GitGud.Controllers.Web
{
    public class TagController : Controller
    {
        private IGitGudRepository _repository;

        public TagController(IGitGudRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Details(string tagName)
        {
            ViewData["tagName"] = tagName;
            var songsByTagName = _repository.GetSongsByTagName(tagName);
            return View(songsByTagName);
        }
    }
}
