using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GitGud.Models;
using Microsoft.Extensions.Configuration;
using GitGud.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using GitGud.Controllers.Web;
using GitGud.Services;

namespace GitGud.Controllers.Web
{
    public class AppController : Controller
    {

        private IConfigurationRoot _config;
        private IHostingEnvironment _environment;
        private IUploadService _uploadService;
        private IGitGudRepository _repository;

        public AppController(IConfigurationRoot config, IGitGudRepository repository, IHostingEnvironment environment, IUploadService uploadService)
        {
            _config = config;
            _repository = repository;
            _environment = environment;
            _uploadService = uploadService;
        }

        public IActionResult Index()
        {
            var tags = _repository.GetTopTags();
            var categories = _repository.GetAllCategories();
            ViewData["categories"] = categories;

            return View(tags);
        }

        public IActionResult Browse(int? page)
        {
            var songs = _repository.GetAllSongs().OrderByDescending(x => x.DateUploaded).ThenBy(x => x.Name);

            int pageSize = 10;//NUMBER OF RESULTS PER PAGE
            ViewData["PagerIsNeeded"] = songs.Count() > pageSize;
            //If the songs we have in the DB are less than the results per page(pageSize) we dont show the navigation in the view.
            return View(PaginatedList<Song>.Create(songs.AsQueryable(), page ?? 1, pageSize));
        }

        public IActionResult Charts()
        {
            return View();
        }

        public IActionResult GetDownloadsChartData()
        {
            var allSongs = _repository.GetAllSongs()
               .OrderByDescending(s => s.Downloads)
               .Take(10)
               .ToList();

            return Json(allSongs);
        }

        public IActionResult HotTracks()
        {
            var hotTracks = _repository.GetHotTracks();

            return View(hotTracks);
        }

        //[Authorize]//ToDo - find out why redirect path is not working 
        [HttpGet]
        public IActionResult Upload()
        {
            //until then - instead of annotations we just check for user login
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }

            var categories = _repository.GetAllCategories();
            ViewData["categories"] = categories;

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Upload(UploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                var categories = _repository.GetAllCategories();
                ViewData["categories"] = categories;

                if (!_repository.SongDuplicateExists(model.SongName, model.Artist))
                {
                    var songExtension = Path.GetExtension(model.MusicFile.FileName);
                    if (model.MusicFile.Length > 10485760 || songExtension != ".mp3")
                    {
                        ModelState.Clear();
                        ViewBag.InputFields = "Sorry but the maximum size allowed for a song is 10mb and the format must be .mp3";
                        ViewBag.AlertType = "danger";
                        return View();
                    }
                    _uploadService.UploadSong(
                                        model.MusicFile,
                                        model.SongName,
                                        model.Artist,
                                        model.Category,
                                        model.Tags.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList(),
                                        this.User.Identity.Name);


                    ModelState.Clear();
                    ViewBag.InputFields = $"Your song \"{model.SongName}\" by \"{model.Artist}\" has been uploaded successfully.";
                    ViewBag.AlertType = "success";
                    return View();
                }
                else
                {
                    ModelState.Clear();
                    ViewBag.InputFields = "Such a song already exists in the Database";
                    ViewBag.AlertType = "danger";
                    return View();
                }
            }
            return RedirectToAction("Upload");
        }

        public IActionResult Search(string id)
        {
            var songs = _repository.GetAllSongs();

            if (!String.IsNullOrEmpty(id))
            {
                id = id.ToLower();
                songs = songs.Where(s => s.ArtistName.ToLower().Contains(id) || s.Name.ToLower().Contains(id));
            }
            else
            {
                return RedirectToAction("Index");
            }
            ViewData["SearchString"] = id;

            return View(songs.ToList());
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult SearchByCategory(string strCategoryId)
        {
            int categoryId = int.Parse(strCategoryId);
            var songsByCategory = _repository.SearchSongsByCategory(categoryId);

            string categoryName = _repository.SearchCategoryById(categoryId).Name;
            ViewData["categoryName"] = categoryName;

            return View(songsByCategory);
        }
    }
}
