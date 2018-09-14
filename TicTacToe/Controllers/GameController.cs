using Microsoft.AspNetCore.Mvc;
using TicTacToe.Attributes;
using TicTacToe.Data_Access;

namespace TicTacToe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Logging]
    [Exception]
    [UnAuthourizedExceptionResponseMessage]
    public class GameController : Controller
    {
        [HttpPost]
        [Route("new")]
        [AuthorizeGame]
        public ActionResult NewGame([FromHeader]string playerOneAPIKey, [FromHeader]string playerTwoAPIKey)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (playerOneAPIKey.Equals(playerTwoAPIKey))
                return BadRequest("PlayerOneAPIKey and PlayerTwoAPIKey is same");
            ITicTacToeServices services = new TicTacToeSQLServices();
            return Ok($"Game ID: {services.GameStart(playerOneAPIKey, playerTwoAPIKey)}");
        }

        [HttpPut]
        [Route("{id}/move/{move}")]
        [AuthorizeMove]
        public ActionResult MakeAMove([FromHeader]string apiKey, int id, int move)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            ITicTacToeServices services = new TicTacToeSQLServices();
            if (!services.IsGameID(id))
                return BadRequest("Game is not available");
            if (!services.CheckStatus(id))
                return BadRequest("Game is finished already");
            if (!services.CheckPlayerInGameByID(apiKey, id))
                return BadRequest("Player is not involved in game");
            if (!services.ChechPlayerMoveInGameByID(apiKey, id))
                return BadRequest("Please wait for opponents move");
            if (!services.CheckMoveIsAvailableInGame(id, move))
                return BadRequest("Player has made this move already");
            services.MakeAMove(id, move, apiKey);
            return Ok("Moved");
        }

        [HttpGet]
        [Route("{id}/status")]
        [AuthorizeGame]
        public ActionResult Status([FromHeader]string playerOneAPIKey, [FromHeader]string playerTwoAPIKey, int id)
        {
            ITicTacToeServices services = new TicTacToeSQLServices();
            if (!services.IsGameID(id))
                return BadRequest("Game is not available");
            if (!services.CheckStatus(id))
                return BadRequest("Game is finished already");
            if (!services.CheckPlayerInGameByID(playerOneAPIKey, id))
                return BadRequest("Player one is not involved in game");
            if (!services.CheckPlayerInGameByID(playerTwoAPIKey, id))
                return BadRequest("Player two is not involved in game");
            return Ok(services.GetStatus(id));
        }
    }
}