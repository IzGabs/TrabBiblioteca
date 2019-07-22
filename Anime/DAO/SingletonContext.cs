using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Anime.Models;

namespace Anime.DAO
{
    public class SingletonContext
    {
        private SingletonContext() { }
        private static BibliotecaContext ctx;
        public static BibliotecaContext GetInstance()
        {
            if (ctx == null)
            {
                ctx = new BibliotecaContext();
            }
            return ctx;

        }
    }
}