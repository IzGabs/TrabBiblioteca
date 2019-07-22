using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Anime.Models.PropAnimes
{
    [Table("AssistindoAnime")]
    public class AssistindoAnime
    {
        public AssistindoAnime()
        {
            DataComeco = DateTime.Now;
        }
        [Key]
        public int IDAnimeSendoAssistido { get; set; }
        public DateTime DataComeco { get; set; }
        public int TempoConclusão { get; set; }
        public Animes Anime { get; set; }
        public Usuario Usuario { get; set; }
        public Temporada TemporadaAtual { get; set; }
        public Episodio UltimoEP { get; set; }
    }
}