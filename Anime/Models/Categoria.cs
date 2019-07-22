using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Anime.Models
{
    [Table("Categoria")]
    public class Categoria
    {
        [Key]
        public int IDCategoria { get; set; }
        public string DescCategoria { get; set; }

        public override string ToString()
        {
            return DescCategoria;
        }
    }
}