using Anime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anime.DAO
{
    public class UsuarioDAO
    {
        private static BibliotecaContext ctx = SingletonContext.GetInstance();
        public static bool Adiciona(Usuario usuario) //Add user 
        {

            if (BuscaPorNickname(usuario) == null)
            {
                ctx.Usuarios.Add(usuario);
                ctx.SaveChanges();
                return true;
            }
            return false;

        }
        public IList<Usuario> Lista() //List Users
        {
            return ctx.Usuarios.ToList();
        }
        public static Usuario BuscarUsuarioPorId(int? id)//Buscar User ID 
        {
            return ctx.Usuarios.Find(id);
        }
        public static void Atualiza(Usuario usuario) //Update User 
        {
            ctx.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }
        public Usuario Busca(string login, string senha) //Buscar 
        {

            return ctx.Usuarios.FirstOrDefault(u => u.Login == login && u.Senha == senha);

        }
        public static Usuario BuscaPorNickname(Usuario usuario) //Buscar ID 
        {
            return ctx.Usuarios.FirstOrDefault(u => u.Nickname == usuario.Nickname);
        }
        public static bool CadastrarAdm(Usuario usuario)//Cadastrar ADM 
        {
            ctx.Usuarios.Add(usuario);
            ctx.SaveChanges();
            return true;
        }
        public static bool VerificaAdm(Usuario usuario)//Validar ADM 
        {
            if (BuscaPorNickname(usuario) == null)
            {
                return true;
            }
            return false;
        }
        public Usuario BuscaNivel(Usuario usuario)//Buscar Nível Usuario
        {
            return ctx.Usuarios.Find(usuario.Nivel);
        }
        public static Usuario BuscarPrimeiro() => ctx.Usuarios.FirstOrDefault(x => x.Login.Equals("Admin")); //Valida se há alguém cadastrado no banco

    }
}