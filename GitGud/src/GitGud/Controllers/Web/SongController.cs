using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitGud.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace GitGud.Controllers.Web
{
    public class SongController : Controller
    {
        private IGitGudRepository _repository;

        public SongController(IGitGudRepository repository)
        {
            _repository = repository;
        }

        public IActionResult DeleteSong(string strSongId)
        {
            int songId = int.Parse(strSongId);

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
    }
}
