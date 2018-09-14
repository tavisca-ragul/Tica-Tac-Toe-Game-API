using Microsoft.AspNetCore.Mvc;
using TicTacToe.Attributes;
using TicTacToe.Data_Access;
using TicTacToe.Model;

namespace TicTacToe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Logging]
    [Exception]
    [UnAuthourizedExceptionResponseMessage]
    public class IdentityController : Controller
    {
        [HttpPost]
        [Route("user")]
        public ActionResult RegisterUser([FromBody]User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            ITicTacToeServices services = new TicTacToeSQLServices();
            if (services.CheckUserName(user.UserName))
                return BadRequest("Username is already exist");
            return Ok($"User ID : {services.RegisterUser(user)}");
        }

        [HttpPut]
        [Route("user/{id}/apikey")]
        public ActionResult GenerateAPIKey(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            ITicTacToeServices services = new TicTacToeSQLServices();
            string userName = services.GetUserNameByID(id);
            if (string.IsNullOrEmpty(userName))
                return BadRequest("User doesn't exist");
            string apiKey = services.GenerateAPIKey(userName);
            if (string.IsNullOrEmpty(apiKey))
                return StatusCode(500);
            return Ok($"Api Key = {apiKey}");
        }
    }
}