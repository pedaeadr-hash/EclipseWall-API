using Microsoft.EntityFrameworkCore;
namespace Gen
{
    public class BankDb : DbContext
    {
        
        public DbSet<Usuario> Usuarios {get;set;}
        public DbSet<Wallpaper> Wallpapers {get;set;}
        public DbSet<FavoritosWallpaper> FavoritosWallpapers {get;set;}
        public DbSet<IconsUser> IconsUsers {get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
                var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
                optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
