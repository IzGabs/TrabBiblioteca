using Anime.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Anime
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            if (UsuarioDAO.BuscarPrimeiro() == null)
            {
                Dados.Inserir();
            }
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
