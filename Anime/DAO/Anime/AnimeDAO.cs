using Anime.Models;
using System.Collections.Generic;
using System.Linq;

namespace Anime.DAO
{
    public class AnimeDAO
    {
        private static BibliotecaContext ctx = SingletonContext.GetInstance();
        public static bool AdicionarAnime(Animes Animes)//Adicionar anime ao banco 
        {
            if (BuscarPorNome(Animes) == null)
            {
                ctx.Animes.Add(Animes);
                ctx.SaveChanges();
                return true;
            }
            return false;
        }
        public static Animes BuscarPorNome(Animes a) => ctx.Animes.SingleOrDefault(x => x.NomeAnime.Equals(a.NomeAnime)); //Buscar anime por nome
        public static Animes BuscarPorID(int? id) => ctx.Animes.Find(id); //Buscar por ID
        public static IList<Animes> ListaAnimes() => ctx.Animes.ToList(); //Todos os animes
        public static void AtualizarAnime(Animes Animes)//Atualizar o Anime
        {
            ctx.Entry(Animes).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }
        public static void RemoverAnime(Animes anime)//Remover Anime
        {
            ctx.Animes.Remove(anime);
            ctx.SaveChanges();

        }
        public static Animes BuscarAnimesPorId(int? id)//Buscar por ID
        {
            return ctx.Animes.Find(id);
        }
        public static Animes BuscarPorNomeInclude(Animes a)//Busca q inclui todos os includes 
        {
            return ctx.Animes.
            Include("Temporadas").
            Include("Temporadas.Episodios").
            Include("Categoria").
            SingleOrDefault(x => x.NomeAnime.Equals(a.NomeAnime));
        }
        public static Animes BuscarPorIDInclude(int? id)//Busca por ID mas inclui todos os includes 
        {
         return   ctx.Animes.
             Include("Temporadas").
             Include("Temporadas.Episodios").
             Include("Categoria").
             SingleOrDefault(x => x.IDAnime == id);
        }
    }   
}

