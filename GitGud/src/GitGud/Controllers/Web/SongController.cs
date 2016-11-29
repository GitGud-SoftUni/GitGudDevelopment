using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitGud.Models;
using Microsoft.AspNetCore.Mvc;
using GitGud.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace GitGud.Controllers.Web
{
    public class SongController : Controller
    {
        private IGitGudRepository _repository;

        public SongController(IGitGudRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult DeleteSong(string strSongId)
        {
            int songId = int.Parse(strSongId);
            Song song = _repository.GetSongById(songId);

            if (song == null)
            {
                return RedirectToAction("Browse", "App");
            }

            return View(song);
        }

        [HttpPost]
        public IActionResult DeleteSong(int songId)
        {

            _repository.DeleteSong(songId);
            return RedirectToAction("Browse", "App");
        }

        public IActionResult DownloadSong(string strSongId)
        {
            int songId = int.Parse(strSongId);
            Song song = _repository.GetSongById(songId);

            string fullSongFileAddress = Path.GetFullPath("..\\GitGud\\wwwroot\\uploads\\")
                                     + song.fileAdress;
            string songTitle = song.fileAdress;
            return PhysicalFile(fullSongFileAddress, "application/mpeg", songTitle);
        }

        [HttpGet]
        public IActionResult Details(int songId)
        {
            if (_repository.SongExists(songId))
            {
                ViewData["songId"] = songId;
                Song featuredSong = _repository.GetSongById(songId);
                return View(featuredSong);
            }
            else
            {
                return RedirectToAction("Browse", "App");
            }


        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                string url = Request.GetDisplayUrl();
                int songId = int.Parse(url.Split('=').ToList().Last().ToString());
                _repository.AddComment(this.User.Identity.Name, model.Content,songId);
                

                ModelState.Clear();
            }
            else
            {
                ViewBag.InputFields = "Comment min length is 3 and max is 255";
            }
            return Redirect(Request.GetDisplayUrl());
        }
    }
}
