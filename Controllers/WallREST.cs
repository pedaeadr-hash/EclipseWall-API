using Gen;
using Microsoft.AspNetCore.Mvc;
namespace ControllersWall
{
    [ApiController]
    [Route("api/[Controller]")]
    public class WallEndPoints : ControllerBase
    {
        public BankDb Bank;
        public WallEndPoints (BankDb db)
        {
            Bank = db;
        }

        [HttpGet]
        public async Task<IActionResult> Eyes()
        {
            return Ok ("funcionando");
        }

    }
}