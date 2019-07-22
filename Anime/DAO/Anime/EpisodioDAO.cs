using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Anime.Models;
using Anime.Models.PropAnimes;

namespace Anime.DAO.Anime
{
    public class EpisodioDAO
    {
        private static BibliotecaContext ctx = SingletonContext.GetInstance();
        public static Episodio BuscarEPporID(int? id) => ctx.Episodios.Find(id);
    }
}