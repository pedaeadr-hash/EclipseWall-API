using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gen;

namespace IconUser
{
    [ApiController]
    [Route("api/[Controller]")]
    public class Icon : ControllerBase
    {
        public BankDb Bank;

        public Icon (BankDb db)
        {
            Bank = db;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Teste()
        {
            return Ok("funcionando");
        }
    }
}