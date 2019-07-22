using Anime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anime.DAO.Anime
{
    public class TemporadasDAO
    {
        private static BibliotecaContext ctx = SingletonContext.GetInstance();


        public static List<Temporada> ListaTemporadas() => ctx.Temporadas.ToList();
        public static Temporada BuscarTempPorId(int? id) => ctx.Temporadas.Find(id);

        public static Temporada AddTemporada(Temporada temps, int qtde)
        {

            if (temps.Episodios == null)
            {
                temps.Episodios = new List<Episodio>();
            }

            for (int i = 1; i < qtde + 1; i++)
            {
                Episodio ep = new Episodio();
                ep.NomeEpisodio = "Episódio";
                ep.NumEpisodio = i;
                temps.Episodios.Add(ep);
            }

            return temps;
        }
        public static void AddStart(Temporada t)
        {
            ctx.Temporadas.Add(t);
            ctx.SaveChanges();
        }
    }
}