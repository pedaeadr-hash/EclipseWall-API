using System.ComponentModel.DataAnnotations;

namespace Gen
{
    public class Wallpaper
    {
        [Key]
        public int Id {get;set;}
        public required string Nome {get;set;}
        public required string Url {get;set;}
        public required string Categoria {get;set;}
        public required int Downloads {get;set;}
        
    }
}