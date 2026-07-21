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
    
            try
            {
                await Bank.IconsUsers.AddAsync(Iu);
                await Bank.SaveChangesAsync();
                return Ok("Tudo Certo icon salva no banco de dados");
            }
            catch (Exception ex)
            {
                // Retorna a mensagem real do erro para você ver no console do navegador/Postman
                return StatusCode(500, new { erro = ex.Message, detalhe = ex.InnerException?.Message });
            }
        }
        }
        }