using Anime.Models.PropAnimes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Anime.Models
{
    [Table("Usuarios")]
    public class Usuario
    {

        [Key]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Voce precisa preencher o campo Nome!")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Voce precisa preencher o campo Sobrenome!")]
        [Display(Name = "Sobrenome")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "Voce precisa preencher o campo Email!")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        //[EmailAddress] tbm tem essa ai
        public string Email { get; set; }


        [Required(ErrorMessage = "Voce precisa preencher o campo Nickname!")]
        [Display(Name = "Nickname")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Voce precisa preencher o campo Login!")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Voce precisa preencher o campo Senha!")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Voce precisa preencher o campo Confirmar Senha!")]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmarSenha")]
        [Compare("Senha", ErrorMessage = " As senhas precisam estar iguais!")]
       // [NotMapped] // pra nao aparecer no banco
        public string ConfirmarSenha { get; set; }

        public int Nivel { get; set; }

        public string Role { get; set; }

        public List<Assistidos> AnimesAssistidos { get; set; }
        public List<Favoritos> AnimesFavoritos { get; set; }
        public List<AssistindoAnime> AssistindoAnime{ get; set; }


    }
}