using GitGud.Models;
using GitGud.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GitGud.Controllers.Api
{
    [Route("api/favorite")]
    [Authorize]
    public class FavoritesController : Controller
    {
        private IGitGudRepository _repository;

        public FavoritesController(IGitGudRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            return BadRequest("Nothing to see here, move along.");
        }

        [HttpPost("")]
        public IActionResult AddRemoveFav([FromBody]FavoriteViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_repository.SongExists(model.Id))
                    {
                        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                        if (!_repository.UserFavExists(model.Id, userId))
                        {
                            _repository.AddFav(model.Id, userId);
                            return Ok("This song was added to your Favorites");
                        }
                        else
                        {
                            _repository.RemoveFav(model.Id, userId);
                            return Ok("This song was removed your Favorites");
                        }
                        
                    }
                    else
                    {
                        return BadRequest("Invalid Data: song does not exist in the database.");
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
