using Microsoft.EntityFrameworkCore;
namespace Gen
{
    public class BankDb : DbContext
    {
        public DbSet<Usuario> Usuarios {get;set;}
        public DbSet<Wallpaper> Wallpapers {get;set;}
        public DbSet<FavoritosWallpaper> FavoritosWallpapers {get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=EclipseWalls;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}