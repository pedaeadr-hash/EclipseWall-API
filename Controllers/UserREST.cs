using Gen;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace Controles
{
    [ApiController]
    [Route("api/[controller]")]
    public class Controll : ControllerBase
    {
        public BankDb bank;

        public Controll (BankDb banco){
            bank=banco;
        }

        private string GerarTokenJwt( string email, int role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Gj10ksao924kalju2399merda82ikdii2ne");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2), // Token expira em 2 horas
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        


        [HttpPost("Create")]
        public async Task<IActionResult> CreateUser ([FromBody]Usuario us)

        {
            if (us == null)
            {
                return BadRequest("Dados inválidos.");
            }
           
            var caixaaltae =us.Email.Trim().ToLower();
            us.Nome = us.Nome.Trim();
            us.Senha=us.Senha.Trim();
            
            us.Email=caixaaltae;
            
            if (us.Nome.Length > 8)
            {
                return BadRequest("nome com muitas caracteres max: 8 caracteres");
            }
            if (us.Nome.Length < 3)
            {
                return BadRequest("nome com poucas caracteres");
            }
            if (us.Email.Length > 60)
            {
                return BadRequest("Email muito grande");
            }
            if (us.Email.Length < 14)
            {
                return BadRequest("email minimo de 6 caracteres");
            }
            if (us.Email.Length > 60)
            {
                return BadRequest("Email muito grande");
            }
            if (!caixaaltae.EndsWith("@gmail.com"))
            {
                return BadRequest("O email deve conter @gmail.com");
            }
            if (us.Senha.Length < 8)
            {
                return BadRequest("Senha fraca, a senha deve conter mais de 8 caracteres");
            }
            if (us.Senha.Length > 25)
            {
                return BadRequest("Senha muito grande");
            }
            // Letra maiúscula
            if (!Regex.IsMatch(us.Senha, "[A-Z]"))
            {
                return BadRequest("A senha deve conter pelo menos uma letra maiúscula.");
            }

            // Letra minúscula
            if (!Regex.IsMatch(us.Senha, "[a-z]"))
            {
                return BadRequest("A senha deve conter pelo menos uma letra minúscula.");
            }

            // Número
            if (!Regex.IsMatch(us.Senha, "[0-9]"))
            {
                return BadRequest("A senha deve conter pelo menos um número.");
            }

            // Caractere especial
            if (!Regex.IsMatch(us.Senha, @"[!@#$%^&*(),.?""':{}|<>_\-+=/\\\[\]]"))
            {
                return BadRequest("A senha deve conter pelo menos um caractere especial.");
            }
            var verificar = await bank.Usuarios.FirstOrDefaultAsync(x=>x.Email==us.Email);
            if (verificar != null)
            {
                return BadRequest("email ja cadastrado");
            }



            //tratamentos acima


            string Crip = BCrypt.Net.BCrypt.HashPassword(us.Senha);
            us.Senha=Crip;
            us.Role=1;
            try
            {
            await bank.Usuarios.AddAsync(us);
            await bank.SaveChangesAsync();
            } catch (Exception x)
            {
                return StatusCode(500,$"Erro interno: {x.Message}");
            }
            
            return Ok("CADASTRO REALIZADO COM SUCESSO");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]Usuario us)
        {
            us.Email=us.Email.ToLower();
            var encontrado = await bank.Usuarios.FirstOrDefaultAsync(x=>x.Email==us.Email);
            if (encontrado == null)
            {
                return BadRequest ("Email ou senha incorretos");
            }
            bool senhahash = BCrypt.Net.BCrypt.Verify(us.Senha, encontrado.Senha);
            if (!senhahash)
            {
                return BadRequest ("Email ou senha incorretos");
            }
            var Token = GerarTokenJwt(us.Email,us.Role);
            string mensagem = "Login Realizado com Sucesso ...";
            return Ok (new {Token,mensagem});
        }

        [HttpGet("verificartt")]
        [Authorize]
        public async Task<IActionResult> hitmanverification()
        {
            // O .NET extrai automaticamente as informações de dentro do Token recebido no Header
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
            return Ok(new {email,role});
        }
    }
        
}