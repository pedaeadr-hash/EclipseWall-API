using System.ComponentModel.DataAnnotations;

namespace Gen
{
    public class Usuario
    {
        [Key]
        public int Id {get;set;}
        public required string Nome {get;set;}
        public required string Email {get;set;}
        public required string Senha {get;set;}
        public required int Role {get;set;}
        
    }
}