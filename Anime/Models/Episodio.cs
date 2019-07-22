using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Anime.Models
{
    [Table("Episodios")]
    public class Episodio
    {
        [Key]
        public int IDEpisodio { get; set; }
        public int NumEpisodio { get; set; }
        public string NomeEpisodio { get; set; }
    }
}