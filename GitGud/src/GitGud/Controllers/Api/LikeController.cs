using GitGud.Models;
using GitGud.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitGud.Controllers.Api
{
    [Route("api/like")]
    [Authorize]
    public class LikeController : Controller
    {
        private IGitGudRepository _repository;

        public LikeController(IGitGudRepository repository)
        {
            _repository = repository;
        }


        [HttpGet("")]
        public IActionResult Get()
        {
            return BadRequest("Nothing to see here, move along.");
        }

        [HttpPost("")]
        public IActionResult Post([FromBody]LikeViewModel liked)
        {

            
            if (ModelState.IsValid)
            {
                _repository.AddLike(liked.CommentId, this.User.Identity.Name);

                return Created($"api/like/{this.User.Identity.Name}", liked);
            }

            return BadRequest("Invalid Data");
        }
    }
}
