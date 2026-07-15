using Gen;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpPost("upwall")]
        public async Task<IActionResult> uploadWall (Wallpaper wall)
        {
            await Bank.Wallpapers.AddAsync(wall);
            Bank.SaveChanges();
            return Ok ("upload feito com sucesso");
        }
        [HttpGet("wall")]
        public async Task<IActionResult> extrairwall(int carregar = 1, int limit = 10, int ordem = 0)
        {
        int registrosParaPular = (carregar - 1) * limit;
        List<Wallpaper> lista = new List<Wallpaper>();
        if (ordem == 0)
            {
                lista = await Bank.Wallpapers
                                .OrderBy(x => x.Downloads)
                                .Skip(registrosParaPular)
                                .Take(limit)
                                .ToListAsync();
            }
        else
            {
                lista = await Bank.Wallpapers
                                .OrderByDescending(x => x.Downloads)
                                .Skip(registrosParaPular)
                                .Take(limit)
                                .ToListAsync();
            }    
        
        

            int countwall = await Bank.Wallpapers.CountAsync();

            // Correção: Arredonda para cima para não cortar a última página incompleta
            int limitecarregarmais = (int)Math.Ceiling((double)countwall / limit);

            return Ok(new { lista, limitecarregarmais });
        }







        [HttpGet("categoriasuniq")]
        public async Task<IActionResult> Unicoscategoria()
        {
            var listcategoria = await Bank.Wallpapers.Select(x=>x.Categoria).Distinct().ToListAsync();
            return Ok(listcategoria);
        }
        
    }

}