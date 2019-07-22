using Anime.Models;
using Anime.Models.PropAnimes;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace Anime.DAO.Anime
{
    public class BibliotecaDAO
    {

        private static BibliotecaContext ctx = SingletonContext.GetInstance();

        public static List<Animes> ListaAnimes()
        {

            return ctx.Animes.ToList();

        }

        public static List<Categoria> RetornarCategoria()
        {
            return ctx.Categorias.ToList();
        }

        public static List<Categoria> RetornarCategoriaUnica(string categoria)
        {
            List<Categoria> categorias = RetornarCategoria();
            List<Categoria> novalista = new List<Categoria>();

            foreach (Categoria item in categorias)
            {
                if (item.DescCategoria == categoria)
                {
                    novalista.Add(item);
                }
            }
            return novalista;
        }

        // /////////////                  /////////////              /////////// Biblioteca Usuario 


        public static bool AdicionarAnimeBiblioteca(Usuario aux, int? id, Assistidos a) //Adicionar os animes á lista do usuário com uma simples alteração
        {
            Assistidos ass = new Assistidos();//Auxiliar

            Usuario u = AnimesAssistidos(aux);// Me trás todos os animes assistidos desse usuário
            try
            {
                ass = u.AnimesAssistidos.Find(x => x.Anime.IDAnime.Equals(id));//valida se a id já está lá 
            }
            catch (System.InvalidOperationException)
            {

                return false;
            }
            

            if (ass == null)// Se não for nulo, adicione esse anime nos assistidos
            {
                u.AnimesAssistidos.Add(a); // Adiciona esse anime a lista de assistidos
                ctx.Entry(u).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
                return true;

            }
            return false;
        }
        public static Usuario AnimesAssistidos(Usuario user)//Estou buscando em AnimesAssistidos(dentro do Usuario) o Anime
        {
            return ctx.Usuarios.
                Include("AnimesAssistidos.Anime").
                Include("AnimesAssistidos.Anime.Categoria").
                Include("AnimesAssistidos.Anime.Temporadas").
                Include("AnimesAssistidos.Anime.Temporadas.Episodios").
                FirstOrDefault(x => x.Login.Equals(user.Login));
        }
        public static Usuario AdicionarAssistindoAnime(Usuario user, AssistindoAnime a) //Adicionar anime a assistindo
        {
            if (user.AssistindoAnime == null)//Isso aqui é só pra iniciar a lista, se não fizer isso vem excessão de nullpointer
            { user.AssistindoAnime = new List<AssistindoAnime>(); }

            user.AssistindoAnime.Add(a);
            ctx.Entry(user).State = System.Data.Entity.EntityState.Modified;
            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges

                ctx.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
               
            }
            
            return user;

        }
        public static void ExcluirAssistidos(Usuario u, Assistidos a) //Excluir dos animes assistidos
        {
            u = AnimesAssistidos(u);//Traz todos so animes assistidos do usuário
            u.AnimesAssistidos.Remove(a);//Remove da lista
            ctx.Entry(u).State = System.Data.Entity.EntityState.Modified;//Altera
            ctx.SaveChanges();//Salva
        }
        public static Assistidos BuscarAnimeAssistido(Usuario u, int? id)  //Busca o Assistido daquele usuário 
        {
         return  u.AnimesAssistidos.FirstOrDefault(x => x.IDANimeAssistido == id);
        }
        public static int TempoTotalAssistidoAll(Usuario u)  //Calcular tempo total de todos os animes
        {
            u = BibliotecaDAO.AnimesAssistidos(u);
            int aux = 0;
            foreach (Assistidos item in u.AnimesAssistidos)
            {
                aux += item.Anime.QtdEpsTotal * 20;
            }
           
            return aux;
        }
    }
}