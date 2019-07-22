using Anime.DAO.Anime;
using Anime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anime.DAO
{
    public class Dados
    {
        public static void Inserir()
        {
            Usuario usuario = new Usuario
            {
                UsuarioId = 10,
                Nome = "Adm",
                Sobrenome = "Adm",
                Email = "biel_s.carvalho2@hotmail.com",
                Nickname = "Administrador",
                Login = "Admin",
                Senha = "Admin",
                ConfirmarSenha = "Admin",
                Nivel = 1,
                Role = "Administrador"
            };

            Categoria c = new Categoria{ DescCategoria = "Shoujo" };
            CategoriaDAO.AddCategoria(c);
            c.DescCategoria = "Shonen";
            CategoriaDAO.AddCategoria(c);
            c.DescCategoria = "Seinen";
            CategoriaDAO.AddCategoria(c);
            c.DescCategoria = "Ecchi";
            CategoriaDAO.AddCategoria(c);

            Temporada t = new Temporada { Estacao = "Verão" };
            TemporadasDAO.AddStart(t);
            t.Estacao = "Outono";
            TemporadasDAO.AddStart(t);
            t.Estacao = "Inverno";
            TemporadasDAO.AddStart(t);
            t. Estacao = "Primavera";
            TemporadasDAO.AddStart(t);


            UsuarioDAO.CadastrarAdm(usuario);

        }
    }
}