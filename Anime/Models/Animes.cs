using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Anime.Models
{
    [Table("Animes")]
    public class Animes
    {
     

        [Key]
        public int IDAnime { get; set; }
        public string NomeAnime { get; set; }
        public string Estudio { get; set; }
        public string Duracao { get; set; }
        public string Imagem { get; set; }
        public Categoria Categoria { get; set; }
        public List<Temporada> Temporadas { get; set; }
        public string Descricao { get; set; }
        public int QtdEpsTotal { get; set; }
       

    }
}