using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Anime.Models
{
    [Table("Temporadas")]
    public class Temporada
    {
        
        [Key]
        public int IDTemporada { get; set; }
        public string Estacao { get; set; }
        public string Ano { get; set; }
        public List<Episodio> Episodios { get; set; }
    }
}