using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Anime.Models;
using Anime.DAO;

namespace Anime.Controllers
{
    [RoutePrefix("Home/api/Web")]
    public class WebController : ApiController
    {

        [HttpGet]
        [Route("Animes/Lista")]
        ///GET: api/web/animes/lista
        public IList<Animes> Animes()
        {
            return AnimeDAO.ListaAnimes();
        }

        [HttpGet]
        [Route("AnimesPorId/{id}")]
        //GET: api/web/animesporid/1
        public Animes AnimePorId(int id)
        {
            return AnimeDAO.BuscarAnimesPorId(id);
        }

        //POST: api/Web/CadastrarAnime/Cadastro
        [HttpPost]
        [Route("CadastrarAnime/Cadastro")]
        public IHttpActionResult CadastrarProduto(Animes a)
        {
            if (!ModelState.IsValid || a == null)
            {
                return BadRequest(ModelState);
            }

            if (AnimeDAO.AdicionarAnime(a))
            {
                return Created("", a);
            }
            return Conflict();
        }

}
}
