using Gen;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);
//cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCors", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // A porta do seu React
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// Configuração do JWT
var key = Encoding.ASCII.GetBytes("Gj10ksao924kalju2399merda82ikdii2ne");
builder.Services.AddAuthentication(options =>
{
options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
options.TokenValidationParameters = new TokenValidationParameters
{
ValidateIssuerSigningKey = true,
IssuerSigningKey = new SymmetricSecurityKey(key),
ValidateIssuer = false,
ValidateAudience = false
};
});


builder.Services.AddControllers();

//conectar o banco de dados 
builder.Services.AddDbContext<BankDb>();


var app = builder.Build(); // Alterado de 'App' para 'app' (convenção C#)

// --- ORDEM DOS MIDDLEWARES (CRUCIAL) ---

// O CORS deve vir logo no início para interceptar requisições Preflight (OPTIONS) do React
app.UseCors("MyCors"); 

// Primeiro autentica (Quem é você?)
app.UseAuthentication(); 

// Depois autoriza (Você pode entrar aqui?)
app.UseAuthorization(); 

// Por fim, mapeia as rotas para os Controllers

app.MapControllers();


// Nota: A partir do .NET 6+, você também pode usar apenas: app.MapControllers();
// Mas a posição DEVE ser após o UseAuthorization.

app.Run();