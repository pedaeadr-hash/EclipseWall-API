using Gen;
using DtoUser;
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

        private string GerarTokenJwt( string email, int role, string name)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Gj10ksao924kalju2399merda82ikdii2ne");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Role, role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2), // Token expira em 2 horas
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        //endpoint in dto

        [HttpPost("Create")]
        public async Task<IActionResult> CreateUser ([FromBody]CreateUser cudto)

        {
            if (cudto == null || string.IsNullOrWhiteSpace(cudto.Name) || string.IsNullOrWhiteSpace(cudto.Email) || string.IsNullOrWhiteSpace(cudto.Senha))
            {
                return BadRequest("Todos os campos (Nome, Email e Senha) são obrigatórios.");
            }

            cudto.Name=cudto.Name.Trim().ToLower();
            cudto.Email=cudto.Email.Trim().ToLower();
            cudto.Senha=cudto.Senha.Trim();

            //TRATAMENTOS
            
            //NameTRATAMENTOS
            if (cudto.Name.Length > 50)
            {
                return BadRequest("O nome deve conter entre 3 e 50 caracteres.");
            }
            if (cudto.Name.Length < 3)
            {
                return BadRequest("O nome deve conter entre 3 e 50 caracteres.");
            }




            //EmailTRATAMENTOS
            if (cudto.Email.Length > 100)
            {
                return BadRequest("Por favor, insira um endereço de e-mail válido.");
            }
            // Expressão regular padrão para validar o formato de qualquer e-mail
            string regexEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(cudto.Email, regexEmail))
            {
            return BadRequest("Por favor, insira um endereço de e-mail válido.");
             
            }


            //SenhaTRATAMENTOS
            if (cudto.Senha.Length < 8)
            {
                return BadRequest("A senha deve ter no mínimo 8 caracteres.");
            }
            if (cudto.Senha.Length > 32)
            {
                return BadRequest("A senha deve ter no máximo de 32 caracteres.");
            }
            string regexSenha = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,32}$";
            if (!Regex.IsMatch(cudto.Senha, regexSenha))
            {
                return BadRequest("A senha deve conter letras maiúsculas, minúsculas, números e pelo menos um caractere especial.");
            }



            //VALIDAREMAILDUPLICADO
            bool Duplicado = await bank.Usuarios.AnyAsync(x=>x.Email==cudto.Email);
            if (Duplicado)
            {
                return BadRequest("Email já Cadastrado");
            }
            //criptar senha
            string SenhaHash = BCrypt.Net.BCrypt.HashPassword(cudto.Senha);
            

            Usuario us = new Usuario {Nome=cudto.Name,Email=cudto.Email,Senha=SenhaHash,Role=1};

            try
            {
                await bank.Usuarios.AddAsync(us);
                await bank.SaveChangesAsync();
            } 
            
            catch (Exception ex)

            {
                return StatusCode(500, $"Erro interno ao salvar no banco: {ex.Message}");
            }






           return Ok ("Cadastrado com Sucesso.");
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login ([FromBody]LoginUser us)
        {
            us.Email=us.Email.Trim().ToLower();
            string EmailRecebido=us.Email;
            string SenhaRecebida=us.Senha;
            var UserSelect = bank.Usuarios.FirstOrDefault(x=>x.Email==EmailRecebido);
            if (UserSelect == null)
            {
                return BadRequest("Email ou Senha Incorretas");
            }
            //OK ENCONTROU O EMAIL VAMOS VERFICAR SE A SENHA ESTA CERTA
            bool Senhahash = BCrypt.Net.BCrypt.Verify(SenhaRecebida,UserSelect.Senha);
            if (!Senhahash)
            {
                return BadRequest("Email ou Senha Incorretas");
            }
            //senha igual com a do email cadastrado tudo certo vamos dar o token
            string Token = GerarTokenJwt(EmailRecebido,UserSelect.Role,UserSelect.Nome);
            string Mensagem = "Login Realizado com Sucesso";
            return Ok (new {Token,Mensagem});
        }

        

        [HttpGet("verificartt")]
        [Authorize]
        public async Task<IActionResult> hitmanverification()
        {
            // O .NET extrai automaticamente as informações de dentro do Token recebido no Header
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                var nome = User.FindFirst(ClaimTypes.Name)?.Value;
                var GetObj= await bank.Usuarios.FirstOrDefaultAsync(x=>x.Email==email);
                if (GetObj == null)
            {
                return BadRequest("Algo de errado email nao conta no banco de dados");
                
            }
            int id =  GetObj.Id;
            return Ok(new {email,role,nome,id});
        }
    }
        
}