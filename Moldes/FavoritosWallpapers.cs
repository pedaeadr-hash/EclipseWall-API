using System.ComponentModel.DataAnnotations;

namespace Gen
{
    public class FavoritosWallpaper
    {
        [Key]
        public int Id {get;set;}
        public required string UserId {get;set;}
        public required string WallId {get;set;}
    
    }
}