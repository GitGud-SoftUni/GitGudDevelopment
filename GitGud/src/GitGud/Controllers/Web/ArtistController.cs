using System;
using System.Collections.Generic;
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
    public class ArtistController : Controller
    {
        private IGitGudRepository _repository;

        public ArtistController(IGitGudRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Artist(string artistName)
        {
            List<Song> songs = _repository.GetSongByArtist(artistName).ToList();

            ViewData["Artist"] = artistName;

            return View(songs);
        }
    }
}
