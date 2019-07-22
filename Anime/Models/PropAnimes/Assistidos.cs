using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anime.Models
{
    [Table("AnimesAssistidos")]
    public class Assistidos
    {
        public Assistidos()
        {
            DataAssistido = DateTime.Now;
        }
        [Key]
        public  int IDANimeAssistido { get; set; }
        public Animes Anime { get; set; }
        public DateTime DataAssistido { get; set; }
       

    }
}