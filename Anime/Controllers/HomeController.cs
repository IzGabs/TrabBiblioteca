using Anime.DAO;
using Anime.DAO.Anime;
using Anime.Filtro;
using Anime.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;


namespace Anime.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Adiciona(Usuario usuario)
        {
            if (ModelState.IsValid)
            {

                if (UsuarioDAO.Adiciona(usuario))
                {

                    return RedirectToAction("Perfil", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Nickname já existe!");
                    return View("Index");
                }
            }
            else
            {

                return View("Index");
            }

        }

        [AutorizacaoFilter]
        public ActionResult Perfil()
        {
            Usuario u = Session["usuarioLogado"] as Usuario; //Pega o usuário na Session

            List<Assistidos> listaaux = new List<Assistidos>();//Lista Auxiliar para levar todos os assistidos

            u = BibliotecaDAO.AnimesAssistidos(u); //chama o método para poder pegar todos os animes assistidos

            foreach (Assistidos aux in u.AnimesAssistidos) //A partir disso eu pego todos os assistidos dele
            {
                aux.Anime = AnimeDAO.BuscarPorNomeInclude(aux.Anime);
                listaaux.Add(aux);//Adiciono para a lista auxiliar
            }
            bool a;
            ViewBag.Contador = listaaux.Count;

            ViewBag.Total = BibliotecaDAO.TempoTotalAssistidoAll(u);
            return View(listaaux);
        }

        public ActionResult Biblioteca()
        {

            ViewBag.Animes = AnimeDAO.ListaAnimes();

            return View();
        }

        [HttpPost]
        public ActionResult Autentica(String login, String senha)
        {
            UsuarioDAO dao = new UsuarioDAO();
            Usuario usuario = dao.Busca(login, senha);

            if (usuario != null)
            {
                //if (User.IsInRole("Administrador"))
                //{
                if (usuario.Nivel.Equals(1))
                {
                    TempData["Nickname"] = usuario.Nickname;
                    Session["usuarioLogado"] = usuario;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    //}
                    //else
                    //{
                    TempData["Nickname"] = usuario.Nickname;
                    TempData["Email"] = usuario.Email;
                    TempData.Keep("Nickname"); 

                    Session["usuarioLogado"] = usuario;
                    return RedirectToAction("Perfil", "Home");
                }
                //}
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Nick()
        {
            Usuario usuario = new Usuario();
            ViewBag.Nickname = usuario.Nickname;

            return View(UsuarioDAO.BuscaPorNickname(usuario));
        }

        public ActionResult PerfilAdm()
        {
            return View();
        }
        
        public ActionResult ConfigEmail()
        {
            return View();
        }
        public ActionResult ConfigNickname()
        {
            return View();
        }
        public ActionResult ConfigImagem()
        {
            return View();
        }

        public ActionResult AlterarSenha()
        {
            Usuario u = Session["usuarioLogado"] as Usuario;
            return View(u);
        }

        [HttpPost]
        public ActionResult AlterarSenha(Usuario usuario)
        {
            Usuario u = new Usuario();
            u = UsuarioDAO.BuscarUsuarioPorId(usuario.UsuarioId);

            u.Senha = usuario.Senha;
            u.ConfirmarSenha = usuario.ConfirmarSenha;

            UsuarioDAO.Atualiza(u);

            if (u.Nivel.Equals(1))
            {
                return RedirectToAction("PerfilAdm", "Home");
            }
            else
            {
                return RedirectToAction("Perfil", "Home");
            }
        }

        [HttpPost]
        public ActionResult AlterarEmail(Usuario usuario)
        {
            Usuario u = UsuarioDAO.BuscarUsuarioPorId(usuario.UsuarioId);
            u.Email = usuario.Email;
            UsuarioDAO.Atualiza(u);

            if (usuario.Nivel.Equals(1))
            {
                return RedirectToAction("PerfilAdm", "Home");
            }
            else
            {
                return RedirectToAction("Perfil", "Home");
            }
        }
        [HttpPost]
        public ActionResult AlterarImagem(Usuario usuario)
        {
            Usuario u = UsuarioDAO.BuscarUsuarioPorId(usuario.UsuarioId);
            UsuarioDAO.Atualiza(u);

            if (usuario.Nivel.Equals(1))
            {
                return RedirectToAction("PerfilAdm", "Home");
            }
            else
            {
                return RedirectToAction("Perfil", "Home");
            }
        }
        [HttpPost]
        public ActionResult AlterarNickname(Usuario usuario)
        {
            Usuario u = new Usuario();

            u = UsuarioDAO.BuscarUsuarioPorId(usuario.UsuarioId);

            u.Nickname = usuario.Nickname;
            UsuarioDAO.Atualiza(u);

            if (usuario.Nivel.Equals(1))
            {
                return RedirectToAction("PerfilAdm", "Home");
            }
            else
            {
                return RedirectToAction("Perfil", "Home");
            }
        }

        public ActionResult WebServices()
        {
            return View();
        }

        public ActionResult Json()
        {
            return View();
        }
        public ActionResult Configuracao()
        {
            return View();
        }
        
    }
}