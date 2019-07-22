using Anime.Models;
using Anime.Models.PropAnimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anime.DAO.Anime
{
    public class AssistindoAnimeDAO
    {
        private static BibliotecaContext ctx = SingletonContext.GetInstance();
        public static void AdicionarAssistido(AssistindoAnime assistindoAnim)
        {
            ctx.AssistindoAnime.Add(assistindoAnim);
            ctx.SaveChanges();
        }
        public static List<AssistindoAnime> AnimesSendoAssistidos(Usuario u)//Busca q inclui todos os includes
        {
            return ctx.AssistindoAnime.
                Include("Anime").
                Include("Anime.Temporadas").
                Include("Anime.Temporadas.Episodios").
                Include("Anime.Categoria").
                Include("Usuario").
                Include("TemporadaAtual").
                Include("UltimoEP").
                Where(x => x.Usuario.Login.Equals(u.Login)).ToList();
        }
        public static AssistindoAnime BuscarAnimeSendoAssistido(AssistindoAnime asn)//Busca q inclui todos os includes
        {
            return ctx.AssistindoAnime.
                Include("Anime").
                Include("Anime.Temporadas").
                Include("Anime.Temporadas.Episodios").
                Include("Anime.Categoria").
                Include("Usuario").
                Include("TemporadaAtual").
                Include("UltimoEP").
                SingleOrDefault(x => x.IDAnimeSendoAssistido == asn.IDAnimeSendoAssistido);
        }
        public static void AtualizarASN(AssistindoAnime asn)
        {
            ctx.Entry(asn).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }
        public static bool VerificarMaxTemp(AssistindoAnime asn)
        {
            List<Episodio> ep = asn.TemporadaAtual.Episodios.ToList();

            int a = ep.Max(t => t.NumEpisodio);

            if (a == (asn.UltimoEP.NumEpisodio))
            {
                return true;
            }
            return false;
        }
        public static void RemoverAnimeAsN(AssistindoAnime asn)
        {
            ctx.AssistindoAnime.Remove(asn);
            ctx.SaveChanges();
        }
        public static int TempoConclusaoTemporada(AssistindoAnime asn)
        {
            int total = asn.TemporadaAtual.Episodios.Count;
            int epsAssistidos = asn.UltimoEP.NumEpisodio;
            int res = total - epsAssistidos;
            return res * 20;
        }
        public static AssistindoAnime BuscarExANS(Usuario u, Animes a) //Vê se esse anime já está sendo assistido
        {
            AssistindoAnime ans = ctx.AssistindoAnime.FirstOrDefault(x =>
            x.Anime.IDAnime.Equals(a.IDAnime) && 
            x.Usuario.UsuarioId.Equals(u.UsuarioId));
            if (ans != null) // Procure em 'AssistindoAnime' um usuario que esteja assistindo aquele anime
            {
                return ans;
            }
            return null;
        }
    }
}