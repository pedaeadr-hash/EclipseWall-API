using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gen;
using Microsoft.IdentityModel.Tokens;

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
        public async Task<IActionResult> Teste([FromBody] IconsUser Iu)
        {
    
                bool Verificar = await Bank.IconsUsers.AnyAsync(x=>x.UserId==Iu.UserId);
                if (Verificar==true)
                {
                    var objecto = await Bank.IconsUsers.FirstOrDefaultAsync(x=>x.UserId==Iu.UserId);
                    if (objecto == null)
                {
                    return BadRequest();
                }
                    objecto.UrlIcon=Iu.UrlIcon;
                
                await Bank.SaveChangesAsync();
                return Ok("Tudo Certo icon salva no banco de dados");
                }
                await Bank.IconsUsers.AddAsync(Iu);
                await Bank.SaveChangesAsync();
                return Ok("Tudo Certo icon salva no banco de dados");
        }

        [HttpPost]
        public async Task<IActionResult> Effects ([FromBody] IconDto ID)
        {
            bool Verificacao = await Bank.IconsUsers.AnyAsync(x=>x.UserId==ID.Id);
            if (!Verificacao)
            {
                string UrlF = "https://i.pinimg.com/736x/d7/b9/48/d7b948ff970f7d92ee265072da06fd07.jpg";
                return Ok (UrlF);
            }
            var transfer = await Bank.IconsUsers.FirstOrDefaultAsync(x=>x.UserId==ID.Id);
            if (transfer == null)
            {
                return BadRequest("Null");
            }
            string Url = transfer.UrlIcon;
            return Ok(Url);
        }
    }
}