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

        [HttpPost("SaveIcon")]
        public async Task<IActionResult> Teste(IconsUser Iu)
        {
            await Bank.AddAsync(Iu);
            await Bank.SaveChangesAsync();
            return Ok("Tudo Certo icon salva no banco de dados");
        }
    }
}