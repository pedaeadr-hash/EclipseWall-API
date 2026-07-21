using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Gen
{
    public class IconsUser
    {
        [Key]
        public int IdTable {get;set;}
        public required int UserId {get;set;}
        public required string UrlIcon {get;set;}
    }
}