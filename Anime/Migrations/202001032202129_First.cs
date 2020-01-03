namespace Anime.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Animes",
                c => new
                    {
                        IDAnime = c.Int(nullable: false, identity: true),
                        NomeAnime = c.String(),
                        Estudio = c.String(),
                        Duracao = c.String(),
                        Imagem = c.String(),
                        Descricao = c.String(),
                        QtdEpsTotal = c.Int(nullable: false),
                        Categoria_IDCategoria = c.Int(),
                    })
                .PrimaryKey(t => t.IDAnime)
                .ForeignKey("dbo.Categoria", t => t.Categoria_IDCategoria)
                .Index(t => t.Categoria_IDCategoria);
            
            CreateTable(
                "dbo.Categoria",
                c => new
                    {
                        IDCategoria = c.Int(nullable: false, identity: true),
                        DescCategoria = c.String(),
                    })
                .PrimaryKey(t => t.IDCategoria);
            
            CreateTable(
                "dbo.Temporadas",
                c => new
                    {
                        IDTemporada = c.Int(nullable: false, identity: true),
                        Estacao = c.String(),
                        Ano = c.String(),
                        Animes_IDAnime = c.Int(),
                    })
                .PrimaryKey(t => t.IDTemporada)
                .ForeignKey("dbo.Animes", t => t.Animes_IDAnime)
                .Index(t => t.Animes_IDAnime);
            
            CreateTable(
                "dbo.Episodios",
                c => new
                    {
                        IDEpisodio = c.Int(nullable: false, identity: true),
                        NumEpisodio = c.Int(nullable: false),
                        NomeEpisodio = c.String(),
                        Temporada_IDTemporada = c.Int(),
                    })
                .PrimaryKey(t => t.IDEpisodio)
                .ForeignKey("dbo.Temporadas", t => t.Temporada_IDTemporada)
                .Index(t => t.Temporada_IDTemporada);
            
            CreateTable(
                "dbo.AnimesAssistidos",
                c => new
                    {
                        IDANimeAssistido = c.Int(nullable: false, identity: true),
                        DataAssistido = c.DateTime(nullable: false),
                        Anime_IDAnime = c.Int(),
                        Usuario_UsuarioId = c.Int(),
                    })
                .PrimaryKey(t => t.IDANimeAssistido)
                .ForeignKey("dbo.Animes", t => t.Anime_IDAnime)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_UsuarioId)
                .Index(t => t.Anime_IDAnime)
                .Index(t => t.Usuario_UsuarioId);
            
            CreateTable(
                "dbo.AnimesFavoritos",
                c => new
                    {
                        IDANimeFavorito = c.Int(nullable: false, identity: true),
                        DataAssistido = c.DateTime(nullable: false),
                        AnimeFavorito_IDAnime = c.Int(),
                        Usuario_UsuarioId = c.Int(),
                    })
                .PrimaryKey(t => t.IDANimeFavorito)
                .ForeignKey("dbo.Animes", t => t.AnimeFavorito_IDAnime)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_UsuarioId)
                .Index(t => t.AnimeFavorito_IDAnime)
                .Index(t => t.Usuario_UsuarioId);
            
            CreateTable(
                "dbo.AssistindoAnime",
                c => new
                    {
                        IDAnimeSendoAssistido = c.Int(nullable: false, identity: true),
                        DataComeco = c.DateTime(nullable: false),
                        TempoConclusão = c.Int(nullable: false),
                        Anime_IDAnime = c.Int(),
                        TemporadaAtual_IDTemporada = c.Int(),
                        UltimoEP_IDEpisodio = c.Int(),
                        Usuario_UsuarioId = c.Int(),
                    })
                .PrimaryKey(t => t.IDAnimeSendoAssistido)
                .ForeignKey("dbo.Animes", t => t.Anime_IDAnime)
                .ForeignKey("dbo.Temporadas", t => t.TemporadaAtual_IDTemporada)
                .ForeignKey("dbo.Episodios", t => t.UltimoEP_IDEpisodio)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_UsuarioId)
                .Index(t => t.Anime_IDAnime)
                .Index(t => t.TemporadaAtual_IDTemporada)
                .Index(t => t.UltimoEP_IDEpisodio)
                .Index(t => t.Usuario_UsuarioId);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        UsuarioId = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Sobrenome = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Nickname = c.String(nullable: false),
                        Login = c.String(nullable: false),
                        Senha = c.String(nullable: false),
                        ConfirmarSenha = c.String(nullable: false),
                        Nivel = c.Int(nullable: false),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.UsuarioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssistindoAnime", "Usuario_UsuarioId", "dbo.Usuarios");
            DropForeignKey("dbo.AnimesFavoritos", "Usuario_UsuarioId", "dbo.Usuarios");
            DropForeignKey("dbo.AnimesAssistidos", "Usuario_UsuarioId", "dbo.Usuarios");
            DropForeignKey("dbo.AssistindoAnime", "UltimoEP_IDEpisodio", "dbo.Episodios");
            DropForeignKey("dbo.AssistindoAnime", "TemporadaAtual_IDTemporada", "dbo.Temporadas");
            DropForeignKey("dbo.AssistindoAnime", "Anime_IDAnime", "dbo.Animes");
            DropForeignKey("dbo.AnimesFavoritos", "AnimeFavorito_IDAnime", "dbo.Animes");
            DropForeignKey("dbo.AnimesAssistidos", "Anime_IDAnime", "dbo.Animes");
            DropForeignKey("dbo.Temporadas", "Animes_IDAnime", "dbo.Animes");
            DropForeignKey("dbo.Episodios", "Temporada_IDTemporada", "dbo.Temporadas");
            DropForeignKey("dbo.Animes", "Categoria_IDCategoria", "dbo.Categoria");
            DropIndex("dbo.AssistindoAnime", new[] { "Usuario_UsuarioId" });
            DropIndex("dbo.AssistindoAnime", new[] { "UltimoEP_IDEpisodio" });
            DropIndex("dbo.AssistindoAnime", new[] { "TemporadaAtual_IDTemporada" });
            DropIndex("dbo.AssistindoAnime", new[] { "Anime_IDAnime" });
            DropIndex("dbo.AnimesFavoritos", new[] { "Usuario_UsuarioId" });
            DropIndex("dbo.AnimesFavoritos", new[] { "AnimeFavorito_IDAnime" });
            DropIndex("dbo.AnimesAssistidos", new[] { "Usuario_UsuarioId" });
            DropIndex("dbo.AnimesAssistidos", new[] { "Anime_IDAnime" });
            DropIndex("dbo.Episodios", new[] { "Temporada_IDTemporada" });
            DropIndex("dbo.Temporadas", new[] { "Animes_IDAnime" });
            DropIndex("dbo.Animes", new[] { "Categoria_IDCategoria" });
            DropTable("dbo.Usuarios");
            DropTable("dbo.AssistindoAnime");
            DropTable("dbo.AnimesFavoritos");
            DropTable("dbo.AnimesAssistidos");
            DropTable("dbo.Episodios");
            DropTable("dbo.Temporadas");
            DropTable("dbo.Categoria");
            DropTable("dbo.Animes");
        }
    }
}
