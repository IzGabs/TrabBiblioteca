using Anime.DAO;
using Anime.DAO.Anime;
using Anime.Filtro;
using Anime.Models;
using Anime.Models.PropAnimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anime.Controllers
{
    public class BibliotecaController : Controller
    {
        // GET: Biblioteca
        public ActionResult Index() //Retorna View Index
        {
            return View();
        }

        public ActionResult BibliotecaAnimes() //Retorna a biblioteca com todos os animes cadastrados pelo ADM
        {
            ViewBag.Categorias = BibliotecaDAO.RetornarCategoria();


            return View(BibliotecaDAO.ListaAnimes());
        }

        public ActionResult AnimesAssistidos()//Lista todos os animes marcados como "assistido"
        {

            Usuario u = Session["usuarioLogado"] as Usuario; //Pega o usuário na Session

            List<Assistidos> listaaux = new List<Assistidos>();//Lista Auxiliar para levar todos os assistidos

            u = BibliotecaDAO.AnimesAssistidos(u); //chama o método para poder pegar todos os animes assistidos

            foreach (Assistidos aux in u.AnimesAssistidos) //A partir disso eu pego todos os assistidos dele
            {
                aux.Anime = AnimeDAO.BuscarPorNomeInclude(aux.Anime);
                listaaux.Add(aux);//Adiciono para a lista auxiliar
            }

            ViewBag.Total = listaaux.Count;
            return View(listaaux); //Envio todos os assistidos para a View
        }

        public ActionResult AdicionarAssistidos(int? id) //Adiciona o anime completo como "assistido"
        {
            Usuario user = Session["usuarioLogado"] as Usuario;
            Animes a = AnimeDAO.BuscarAnimesPorId(id);//Pega o anime inteiro a partir de sua ID
            if (user.AnimesAssistidos == null)//Isso aqui é só pra iniciar a lista, se não fizer isso vem excessão de nullpointer
            {
                user.AnimesAssistidos = new List<Assistidos>();
            }
            Assistidos aux = new Assistidos
            {
                Anime = a // Recebe o anime e coloca ele como 'Assistido"
            };

           List<AssistindoAnime> asn = AssistindoAnimeDAO.AnimesSendoAssistidos(user);
            foreach (AssistindoAnime item in asn)
            {
                if (item.Anime.NomeAnime.Equals(a.NomeAnime))
                {
                    AssistindoAnimeDAO.RemoverAnimeAsN(item);
                    break;
                }
            }

            if (BibliotecaDAO.AdicionarAnimeBiblioteca(user, id, aux) == true)// Se retornar false, o anime já está cadastrado
            {

                return RedirectToAction("Perfil", "Home");// Redireciona para o perfil como "sucesso"
            }
            TempData["MsgErro"] = "O anime " + a.NomeAnime + " já está adicionado a 'Assistidos' !! ";
            return RedirectToAction("BibliotecaAnimes", "Biblioteca");//Retorna para a Biblioteca para ele adicionar outro
        }
        public ActionResult ExcluirAssistidos(int? id) // Funcionando
        {
            Usuario user = Session["usuarioLogado"] as Usuario; //Pega o usuário na Session
           Assistidos helps = BibliotecaDAO.BuscarAnimeAssistido(user,id); // Procura o "assistindo"
            BibliotecaDAO.ExcluirAssistidos(user, helps);//Exclui
            return RedirectToAction("AnimesAssistidos", "Biblioteca");//Retorna
        } 

        public ActionResult AssistindoAnime(int? id) // Faz o dropdown de temporadas
        {
            Usuario user = Session["usuarioLogado"] as Usuario; //Pega o usuário na Session
            Animes a = AnimeDAO.BuscarPorIDInclude(id);

            if (a != null) // Primeiro acesso nunca vai ser null
            {
                ViewBag.Temporada = new SelectList(a.Temporadas.ToList(), "IDTemporada", "Estacao"); //DropDown
                TempData["SaveInfo"] = a; //Salva as informações para quando alterar
            }
            else //Vai entrar aqui quando o cara mudar de temporada na view
            {
                a = TempData["SaveInfo"] as Animes; //Vai pegar o anime salvo
                ViewBag.Temporada = new SelectList(a.Temporadas.ToList().OrderByDescending(e => e.Estacao), "IDTemporada", "Estacao"); //Dropdown
                ViewBag.NameTemp = TempData["SaveChan"]; //Pega o valor salvo lá na  EpisodiosTemporada
            }

            List<Episodio> eps = TempData["EpisodiosTemp"] as List<Episodio>; //Lista de episodios dql temp

            if (eps != null) //se os eps forem nulos, não crie viewbag
            {
                ViewBag.Episodios = new SelectList(eps.ToList(), "IDEpisodio", "NumEpisodio"); //Cria Viewbag dos eps
            }

            return View(a); //Retorna o anime
        }

        public ActionResult AnimesAssistindo() // Faz a lista dos animes que estão marcados como "assistindo" do usuário logado
        {
            Usuario user = Session["usuarioLogado"] as Usuario; //Pega o usuário na Session
            List<AssistindoAnime> x = AssistindoAnimeDAO.AnimesSendoAssistidos(user);
            
            foreach (AssistindoAnime item in x)
            {
                item.TempoConclusão = AssistindoAnimeDAO.TempoConclusaoTemporada(item);
               
            }
            
            return View(x);
        }

        [HttpPost]
        public ActionResult AssistindoAnime(Animes a, int? Episodios) //Adiciona o "assistindo" para aquele usuário
        {
            Usuario user = Session["usuarioLogado"] as Usuario; //Pega o usuário na Session
            Temporada t = TempData["SaveTemp"] as Temporada;
            AssistindoAnime an = new AssistindoAnime();
            a = AnimeDAO.BuscarPorIDInclude(a.IDAnime);

            an = AssistindoAnimeDAO.BuscarExANS(user, a);
            if ( an != null) //Se não for nulo, ele já marcou como "estou assistindo", ou seja, apenas atualiza esse
            {
                an = AssistindoAnimeDAO.BuscarAnimeSendoAssistido(an); // Essa busca tem todos os includes 
                an.TemporadaAtual = t;
                an.UltimoEP = EpisodioDAO.BuscarEPporID(Episodios);
                AssistindoAnimeDAO.AtualizarASN(an);
            }
            else
            {
                an = new AssistindoAnime();
                an.Anime = a;
                an.Usuario = user;
                an.UltimoEP = EpisodioDAO.BuscarEPporID(Episodios);
                an.TemporadaAtual = t;
                AssistindoAnimeDAO.AdicionarAssistido(an);
            }

            return RedirectToAction("AnimesAssistindo", "Biblioteca");
        }
        [HttpPost]
        public ActionResult EpisodiosTemporada(int? Temporada) //Entra aqui quando o cara muda de temporada. Envia todos os episódios de X temporada 
        {
            Temporada t = TemporadasDAO.BuscarTempPorId(Temporada); //busca a temporada

            TempData["EpisodiosTemp"] = t.Episodios.ToList(); //salva a lista de episodios
            TempData["SaveChan"] = t.Estacao + "~" + t.Ano; //Salva o nome da estacao
            TempData["SaveTemp"] = t;

            return RedirectToAction("AssistindoAnime", "Biblioteca");
        }

        public ActionResult AtualizarEpAnime(int? id)//Uma Cópia do adicionar praticamente
        {
            Usuario user = Session["usuarioLogado"] as Usuario; //Pega o usuário na Session
            AssistindoAnime asn = new AssistindoAnime();
            Temporada t = TempData["SaveTemp1"] as Temporada;
            List<Episodio> eps = new List<Episodio>();

            if (id == null)
            {
                asn = TempData["ASNAUX"] as AssistindoAnime;
            }
            else
            {
                asn.IDAnimeSendoAssistido = (int)id;
                asn = AssistindoAnimeDAO.BuscarAnimeSendoAssistido(asn);
                TempData["ASNAUX"] = asn;
            }

            if (t == null) //Se for nulo, é o primeiro acesso
            {
                ViewBag.Temporada = new SelectList(asn.Anime.Temporadas.ToList().OrderByDescending(e => e.Estacao.Equals(asn.TemporadaAtual.Estacao)), "IDTemporada", "Estacao", "Ano");
                eps = asn.TemporadaAtual.Episodios.ToList(); //Lista de episodios dql temp
                if (eps != null) //se os eps forem nulos, não crie viewbag
                {
                    ViewBag.Episodios = new SelectList(eps.ToList().OrderByDescending(e => e.NumEpisodio.Equals(asn.UltimoEP.NumEpisodio)), "IDEpisodio", "NumEpisodio"); //Cria Viewbag dos eps
                }
            }
            else
            {
                asn.TemporadaAtual = t;
                eps = asn.TemporadaAtual.Episodios.ToList(); //Lista de episodios dql temp
                ViewBag.Temporada = new SelectList(asn.Anime.Temporadas.ToList().OrderByDescending(e => e.Estacao.Equals(asn.TemporadaAtual.Estacao)), "IDTemporada", "Estacao", "Ano");
                ViewBag.Episodios = new SelectList(eps.ToList().OrderByDescending(e => e.NumEpisodio.Equals(asn.UltimoEP.NumEpisodio)), "IDEpisodio", "NumEpisodio"); //Cria Viewbag dos eps
                
            }

            return View(asn); //Retorna o anime sendo assistido
        }
        

        [HttpPost]
        public ActionResult AtualizarEpAnime(AssistindoAnime asn, int? Episodios) //Alterar POST
        {
            Usuario user = Session["usuarioLogado"] as Usuario; //Pega o usuário na Session         
           
            asn = AssistindoAnimeDAO.BuscarAnimeSendoAssistido(asn);
            asn.UltimoEP = EpisodioDAO.BuscarEPporID(Episodios);

            if (AssistindoAnimeDAO.VerificarMaxTemp(asn))
            {
              //Fazer


                AssistindoAnimeDAO.AtualizarASN(asn);
            }
            else
            {
                AssistindoAnimeDAO.AtualizarASN(asn);
            }
          
           
            return RedirectToAction("AnimesAssistindo", "Biblioteca");

        }

        [HttpPost]
        public ActionResult EpisodiosTemporadaEdit(int? Temporada) //Entra aqui quando o cara muda de temporada. Envia todos os episódios de X temporada 
        {
            Temporada t = TemporadasDAO.BuscarTempPorId(Temporada); //busca a temporada
            TempData["SaveChan1"] = t.Estacao + "~ " + t.Ano; //Salva o nome da estacao

            TempData["SaveTemp1"] = t;

            return RedirectToAction("AtualizarEpAnime", "Biblioteca");
        }
        public ActionResult RemoverAssistindo(int? id) // Remover anime de assistidos
        {

            AssistindoAnime asn = new AssistindoAnime();
            asn.IDAnimeSendoAssistido = (int) id;
            asn = AssistindoAnimeDAO.BuscarAnimeSendoAssistido(asn);
            if (asn != null)
            {
                AssistindoAnimeDAO.RemoverAnimeAsN(asn);
            }
            return RedirectToAction("AnimesAssistindo", "Biblioteca");
        }

        /// ///////////////////////////////////////////////////////////// ---- Filtro CATEGORIAS -- //////////////////////


        [AutorizacaoFilter]
        public ActionResult AnimesEcchi()
        {
            ViewBag.Categoria = "Ecchi";
            return View(BibliotecaDAO.ListaAnimes());
        }
        [AutorizacaoFilter]
        public ActionResult AnimesHorror()
        {
            ViewBag.Categoria = "Horror";
            return View(BibliotecaDAO.ListaAnimes());
        }
        [AutorizacaoFilter]
        public ActionResult AnimesShonen()
        {
            ViewBag.Categoria = "Shonen";
            return View(BibliotecaDAO.ListaAnimes());
        }
        [AutorizacaoFilter]
        public ActionResult AnimesSeinen()
        {
            ViewBag.Categoria = "Seinen";
            return View(BibliotecaDAO.ListaAnimes());
        }
        [AutorizacaoFilter]
        public ActionResult AnimesHentai()
        {
            ViewBag.Categoria = "Hentai";
            return View(BibliotecaDAO.ListaAnimes());
        }
        [AutorizacaoFilter]
        public ActionResult AnimesJosei()
        {
            ViewBag.Categoria = "Josei";
            return View(BibliotecaDAO.ListaAnimes());
        }
        [AutorizacaoFilter]
        public ActionResult AnimesShoujo()
        {
            ViewBag.Categoria = "Shoujo";
            return View(BibliotecaDAO.ListaAnimes());
        }
        [AutorizacaoFilter]
        public ActionResult AnimesKodomo()
        {
            ViewBag.Categoria = "Kodomo";
            return View(BibliotecaDAO.ListaAnimes());
        }


    }
}