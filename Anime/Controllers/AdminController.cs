using Anime.Models;
using Anime.DAO;
using Anime.DAO.Anime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anime.Controllers
{
    public class AdminController : Controller
    {

       
        public ActionResult Index() // GET: Admin
        {
            return View();
        }
        public ActionResult AdicionarAnime()//  Get Adicionar anime
        {
            ViewBag.Categorias = new SelectList(CategoriaDAO.RetornarCategoria(), "IDCategoria", "DescCategoria");
            return View();
        }
        [HttpPost]
        public ActionResult AdicionarAnime(int? Categorias, Animes a, HttpPostedFileBase AnimeImagem)//Post Adicionar Anime
        {
            ViewBag.Categorias = new SelectList(CategoriaDAO.RetornarCategoria(), "IDCategoria", "DescCategoria");

            if (AnimeDAO.BuscarPorNome(a) == null)
            {
                a.Categoria = CategoriaDAO.BuscarCategoriaPorID(Categorias);
                if (AnimeImagem == null)
                {
                    a.Imagem = "SemImagem.jpg";
                }
                else
                {
                    string c = System.IO.Path.Combine(Server.MapPath("~/Imagem/"), AnimeImagem.FileName);
                    AnimeImagem.SaveAs(c);
                    a.Imagem = AnimeImagem.FileName;
                }
                if (AnimeDAO.AdicionarAnime(a))
                {
                    return RedirectToAction("Index", "Admin");
                }

                return View(a);
            }
            ModelState.AddModelError("", "Esse anime já está cadastrado!");
            return View(a);
        }
        public ActionResult AdicionarTemp(int? id) //  Get Adicionar Temporada
        {
            ViewBag.Temporadas = new SelectList(TemporadasDAO.ListaTemporadas(), "IDTemporada", "Estacao");
            ViewBag.Msgs = "Cadastrado com sucesso, adicione as temporadas";
            var a = AnimeDAO.BuscarPorIDInclude(id);

            // ViewBag.Anime = TempData["AdtempAnime"];

            return View(a);
        }
        [HttpPost]
        public ActionResult AdicionarTemp(int? id, int? Temporadas, string txtAno, int QtdeEps) //Post Adicionar Temporada
        {
            //Busca as temporadas
            ViewBag.Temporadas = new SelectList(TemporadasDAO.ListaTemporadas(), "IDTemporada", "Estacao");
            //Adiciona o ano ao objeto
            Animes a = new Animes();
            Temporada temp = new Temporada { Ano = txtAno };
            if (Temporadas != null && txtAno != null)
            {
                //Pegar o objeto todo
                a = AnimeDAO.BuscarPorID(id);

                //Trazer a temporada, de acordo com o ID recebido
                var back = TemporadasDAO.BuscarTempPorId(Temporadas);
                temp.Estacao = back.Estacao;
                Temporada t = TemporadasDAO.AddTemporada(temp, QtdeEps);

                //Precisa iniciar uma lista, caso ela esteja nula
                if (a.Temporadas == null)
                {
                    a.Temporadas = new List<Temporada>();
                }
                a.Temporadas.Add(t);


                a.QtdEpsTotal += t.Episodios.Count;
                
                AnimeDAO.AtualizarAnime(a);
                TempData["Msgs"] = "Cadastrado com sucesso. Procure o anime e adicione temporadas!";
                return RedirectToAction("Index", "Admin");
            }
            ModelState.AddModelError("", "Não deixe valores nulos! ");
            return View(a);
        }
        public ActionResult RemoverAnime(int? id)// GEt Remover Anime
        {
            Animes a = AnimeDAO.BuscarPorID(id);
            if (a != null)
            {
                AnimeDAO.RemoverAnime(a);
                TempData["Msgs"] = "Anime Deletado com sucesso! ";
                return RedirectToAction("Index", "Admin");
            }

            ModelState.AddModelError("", "Esse anime não está cadastrado na base!");
            return RedirectToAction("Index", "Admin");

        }
        public ActionResult PesquisarAnime()// Get Pesquisar Anime
        {
            Animes a = TempData["PesqAnime"] as Animes;
            ViewBag.Msg = TempData["Msgs"];
            if (a != null)
            {
                return View(a);
            }
            return RedirectToAction("Admin", "Index");
        }
        [HttpPost]
        public ActionResult PesquisarAnime(string NomeAnime) //Post Pesquisar Anime
        {
            Animes a = new Animes();
            a.NomeAnime = NomeAnime;
            Animes b = AnimeDAO.BuscarPorNomeInclude(a);
            if (b != null)
            {
                TempData["PesqAnime"] = b;
                return RedirectToAction("PesquisarAnime", "Admin");
            }

            ModelState.AddModelError("", "Esse anime não está cadastrado!");

            TempData["Msgs"] = "Esse anime não está cadastrado na base!";
            return RedirectToAction("Index", "Admin");
        }
        public ActionResult AlterarAnime(int? id) // Get Alterar Anime
        {
            ViewBag.Categorias = new SelectList(CategoriaDAO.RetornarCategoria(), "IDCategoria", "DescCategoria");
            Animes a = AnimeDAO.BuscarPorID(id);
            return View(AnimeDAO.BuscarPorNomeInclude(a));
        }
        [HttpPost]
        public ActionResult AlterarAnime(int? Categorias, Animes anime, HttpPostedFileBase AnimeImagem) //Post Alterar Anime
        {
            ViewBag.Categorias = new SelectList(CategoriaDAO.RetornarCategoria(), "IDCategoria", "DescCategoria");
            Animes a = new Animes();
            a = AnimeDAO.BuscarPorID(anime.IDAnime);
            var temp = AnimeDAO.BuscarPorNome(anime);
            //Coloca um nome para a img primeiro
            if (temp == null || temp.NomeAnime.Equals(anime.NomeAnime))
            {
                if (AnimeImagem != null)
                {
                    if (AnimeImagem.FileName != anime.Imagem)
                    {

                        string c = System.IO.Path.Combine(Server.MapPath("~/Imagem/"), AnimeImagem.FileName);
                        AnimeImagem.SaveAs(c);
                        a.Imagem = AnimeImagem.FileName;
                    }
                }
                else
                {
                    a.Imagem = anime.Imagem;
                }
                //Depois compara
                if (a != anime)
                {
                    if (Categorias != null)
                    {
                        a.Categoria = CategoriaDAO.BuscarCategoriaPorID(Categorias);
                    }
                    
                    a.NomeAnime = anime.NomeAnime;
                    a.Descricao = anime.Descricao;
                    a.Duracao = anime.Duracao;
                    a.Estudio = anime.Estudio;

                    AnimeDAO.AtualizarAnime(a);
                    TempData["Msgs"] = "Alterado com sucesso";
                    return RedirectToAction("Index", "Admin");
                }
                ModelState.AddModelError("", "Esse nome de anime já está cadastrado!");
                return View(anime);
            }
            ModelState.AddModelError("", "Não há alterações! ");
            return View(anime);

        }
        public ActionResult AdicionarCategoria() => View();//Get AdicionarCategoria 
        [HttpPost]
        public ActionResult AdicionarCategoria(Categoria c) // Post Adicionar Categoria
        {
            if (ModelState.IsValid)
            {
                if (CategoriaDAO.BuscarCategoriaPorNome(c) == null)
                {
                    CategoriaDAO.AddCategoria(c);
                }
                else
                {
                    ModelState.AddModelError("", "Essa categoria já está cadastrada! ");
                    return View(c);
                }

                TempData["Msgs"] = "Categoria adicionada com sucesso! ";
                return RedirectToAction("Index", "Admin");
            }
            ModelState.AddModelError("", "Não deixe valores nulos! ");
            return View(c);


            
        }
    }
}