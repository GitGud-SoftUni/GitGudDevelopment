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

namespace GitGud.Controllers.Web
{
    public class AppController : Controller
    {
        private GitGudContext _context;
        private IConfigurationRoot _config;
        private IHostingEnvironment _environment;

        public AppController(IConfigurationRoot config, GitGudContext context, IHostingEnvironment environment)
        {
            _config = config;
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Browse()
        {
            var data = _context.Songs.ToList();

            return View(data);
        }

        public IActionResult Charts()
        {
            return View();
        }

        public IActionResult HotTracks()
        {
            return View();
        }

        public IActionResult Upload()
        {
            return View();
        }

        //[Authorize]//only registered users to be able to upload files
        [HttpPost, Authorize]
        public IActionResult Upload(UploadViewModel model)
        {
           
            return View();
        }

        public IActionResult Search()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
