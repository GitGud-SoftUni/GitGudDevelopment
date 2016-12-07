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
    [Route("api/comment")]
    [Authorize(Roles = "Admin")]
    public class CommentController : Controller
    {
        private IGitGudRepository _repository;

        public CommentController(IGitGudRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            return BadRequest("Nothing to see here, move along.");
        }


        [HttpPost("")]
        public IActionResult RemoveComment([FromBody]DeleteCommentViewModel comIdToDel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_repository.CommentExists(comIdToDel.Id))
                    {
                        _repository.DeleteCommentById(comIdToDel.Id);

                        return Ok($"Comment: {comIdToDel.Id} was deleted");
                    }
                    else
                    {
                        return BadRequest("Invalid Data: comment does not exist in the database.");
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
