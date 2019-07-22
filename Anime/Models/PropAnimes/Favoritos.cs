using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Anime.Models
{ 
        [Table("AnimesFavoritos")]
        public class Favoritos
        {
            public Favoritos()
            {
                DataAssistido = DateTime.Now;
            }
            [Key]
            public int IDANimeFavorito { get; set; }
            public Animes AnimeFavorito { get; set; }
            public DateTime DataAssistido { get; set; }
        }
}