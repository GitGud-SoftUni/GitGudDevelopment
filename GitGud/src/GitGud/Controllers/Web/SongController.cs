using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitGud.Models;
using Microsoft.AspNetCore.Mvc;

namespace GitGud.Controllers.Web
{
    public class SongController : Controller
    {
        private IGitGudRepository _repository;

        public SongController(IGitGudRepository repository)
        {
            _repository = repository;
        }

        public IActionResult DeleteSong(Song song)
        {
            _repository.DeleteSong(song);
            return RedirectToAction("Browse", "App");
        }
    }
}
