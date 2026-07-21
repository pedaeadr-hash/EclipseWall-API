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
    }
}