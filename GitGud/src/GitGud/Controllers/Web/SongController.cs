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
using Microsoft.AspNetCore.Hosting;

namespace GitGud.Controllers.Web
{
    public class SongController : Controller
    {
        private IGitGudRepository _repository;
        private IHostingEnvironment _environment;

        public SongController(IGitGudRepository repository, IHostingEnvironment environment)
        {
            _repository = repository;
            _environment = environment;
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeleteSong(string strSongId)
        {
            int songId = int.Parse(strSongId);
            Song song = _repository.GetSongById(songId);

            if (song == null)
            {
                return RedirectToAction("Browse", "App");
            }

            if (!User.IsInRole("Admin") && !song.UploaderName.Equals(User.Identity.Name))
            {
                return this.StatusCode(401);
            }

            return View(song);
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteSong(int songId)
        {

            Song song = _repository.GetSongById(songId);

            if (song == null)
            {
                return RedirectToAction("Browse", "App");
            }

            if (!User.IsInRole("Admin") && !song.UploaderName.Equals(User.Identity.Name))
            {
                return this.StatusCode(401);
            }

            _repository.DeleteSong(songId);
            return RedirectToAction("Browse", "App");
        }

        public FileContentResult DownloadSong(string strSongId)
        {
            int songId = int.Parse(strSongId);
            Song song = _repository.GetSongById(songId);

            string fullSongFileAddress = Path.GetFullPath(_environment.WebRootPath) + @"\uploads\"
                                     + song.fileAdress;
            string songTitle = song.fileAdress;
            song.Downloads++;
            _repository.SaveChangesInDb();
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullSongFileAddress);
            return File(fileBytes, "application/mpeg", songTitle);
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
                _repository.AddComment(this.User.Identity.Name, model.Content, songId);


                ModelState.Clear();
            }
            else
            {
                ViewBag.InputFields = "Comment min length is 3 and max is 255";
            }
            return Redirect(Request.GetDisplayUrl());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditSong(int id)
        {
            try
            {
                if (_repository.SongExists(id))
                {
                    var categories = _repository.GetAllCategories();
                    ViewData["categories"] = categories;
                    ViewData["songId"] = id;

                    var song = _repository.GetSongById(id);

                    ViewData["artist"] = song.ArtistName;
                    ViewData["songName"] = song.Name;
                    ViewData["category"] = song.Category.Name;
                    ViewData["tags"] = string.Join(", ", song.Tags.Select(x => x.Name.ToString()));

                    return View();
                }
                else
                {
                    return RedirectToAction("Browse", "App");
                }
            }
            catch (Exception ex)
            {

                return BadRequest($"Something went wrong: {ex.Message}");
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditSong(int songId, EditSongViewModel model)
        {
            try
            {
                if (_repository.SongExists(songId))
                {
                    if (ModelState.IsValid)
                    {
                        if (_repository.SongDuplicateExists(model.SongName, model.Artist) && _repository.DuplicateIsThisSong(songId, model.SongName, model.Artist))
                        {
                            _repository.EditSong(songId, model.Artist, model.SongName, model.Category, model.Tags);
                            return RedirectToAction("Details", new { songId = songId });
                        }
                        else if (!_repository.SongDuplicateExists(model.SongName, model.Artist))
                        {
                            _repository.EditSong(songId, model.Artist, model.SongName, model.Category, model.Tags);
                            return RedirectToAction("Details", new { songId = songId });
                        }
                        else
                        {
                            ModelState.Clear();
                            var categories = _repository.GetAllCategories();
                            ViewData["categories"] = categories;
                            ViewData["songId"] = songId;

                            var song = _repository.GetSongById(songId);

                            ViewData["artist"] = song.ArtistName;
                            ViewData["songName"] = song.Name;
                            ViewData["category"] = song.Category.Name;
                            ViewData["tags"] = string.Join(", ", song.Tags.Select(x => x.Name.ToString()));

                            ViewBag.InputFields = "Such a song already exists in the Database";
                            return View();
                        }
                    }
                    return RedirectToAction("Browse", "App");
                }
                else
                {
                    return RedirectToAction("Browse", "App");
                }
            }
            catch (Exception ex)
            {

                return BadRequest($"Something went wrong: {ex.Message}");
            }
        }
    }
}
