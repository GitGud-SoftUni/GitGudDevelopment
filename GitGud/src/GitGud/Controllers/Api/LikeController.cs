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
            string user = this.User.Identity.Name;


            if (ModelState.IsValid)
            {
                try
                {
                    if (!_repository.UserLikeExists(liked.CommentId, user))
                    {
                        _repository.AddLike(liked.CommentId, user);

                        return Created($"api/like/{user}", liked);
                    }
                    else
                    {
                        _repository.RemoveLike(liked.CommentId, user);
                        return Ok($"api/like/{user} like removed");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"Something went wrong: {ex.Message}");
                }
            }

            return BadRequest("Invalid Data");
        }
    }
}
