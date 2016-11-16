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

namespace GitGud.Controllers.Web
{
    public class AppController : Controller
    {
        private GitGudContext _context;
        private IConfigurationRoot _config;
        private IHostingEnvironment _environment;

<<<<<<< HEAD
        public AppController(IConfigurationRoot config, GitGudContext context)
=======
        public AppController(IConfigurationRoot config ,GitGudContext context, IHostingEnvironment environment)
>>>>>>> b31129709ba00469dbe555a22505ecbcee2100c4
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

        [HttpPost]
        public IActionResult Upload(UploadViewModel model)
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
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
