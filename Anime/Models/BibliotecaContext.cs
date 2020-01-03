using Anime.Models;
using Anime.Models.PropAnimes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Anime.DAO
{
    public class BibliotecaContext : DbContext
    {
        //Criei a context de anime, episodio, temporada, categoria. Assim, vai ser criado uma tabela para todos. 
        public BibliotecaContext() : base("AnimeBancoM") { }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Episodio> Episodios { get; set; }
        public DbSet<Temporada> Temporadas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Animes> Animes { get; set; }
        public DbSet<Assistidos> AnimesAssistidos { get; set; }
        public DbSet<Favoritos> AnimesFavoritos { get; set; }
        public DbSet<AssistindoAnime> AssistindoAnime { get; set; }
    }
}