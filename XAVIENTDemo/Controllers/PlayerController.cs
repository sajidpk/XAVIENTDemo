using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XAVIENTDemo.Model;
using XAVIENTDemo.Repository;

namespace XAVIENTDemo.Controllers
{
 
    [Route("api/[controller]")]
    public class PlayersController : Controller
    {
        private readonly IPlayersRepository _playerRepository;

        public PlayersController(IPlayersRepository bookRepository)
        {
            _playerRepository = bookRepository;
        }

      
        [HttpGet]
        public async Task<IActionResult> GetPlayersAsync()
        {        

            var players = await _playerRepository.GetPlayersAsync();
            return  Ok(players);          
        }

        
        [HttpGet("{id}", Name = "GetPlayer")]
        public async Task<IActionResult> GetPlayerAsync(int id)
        {
          var player = await _playerRepository.GetPlayerByIDAsync(id);

          if ( player == null)
            {
                return NotFound();
            }

            return  Ok(player);
        }
        
        
        [HttpPost]
        public async Task<IActionResult> CreatePlayerAsync([FromBody]Player player)
        {
            if ( player==null)
            {
                return BadRequest();
            }

            if( ! await _playerRepository.SavePlayersAsync(player))
            {
               throw new Exception("Problem while saving..");
            }

            return CreatedAtRoute("GetPlayer", new { id= player.Id } , player);

        }
        
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayerAsync(int id, [FromBody]Player player)
        {
            if (player == null)
            {
                return BadRequest();
            }

            var orginalPlayer = await _playerRepository.GetPlayerByIDAsync(id);

            if (orginalPlayer == null)
            {
                return NotFound();
            }


            if (!await _playerRepository.UpdatePlayersAsync(player))
            {
                throw new Exception("Problem while updating..");
            }

            return NoContent();
        }
        
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayerAsync(int id)
        {

            var player = await _playerRepository.GetPlayerByIDAsync(id);

            if (player == null)
            {
                return NotFound();
            }


            if (!await _playerRepository.DeletePlayersAsync(player))
            {
                throw new Exception("Problem while saving..");
            }

            return NoContent();
        }
    }
}
