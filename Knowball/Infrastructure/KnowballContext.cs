using Knowball.Domain;
using Knowball.Models;
using Microsoft.EntityFrameworkCore;

namespace Knowball.Infrastructure
{
    public class KnowballContext(DbContextOptions<KnowballContext> options) : DbContext(options)
    {
    public DbSet<Campeonato> Campeonatos { get; set; }
    public DbSet<Equipe> Equipes { get; set; }
    public DbSet<Arbitro> Arbitros { get; set; }
    public DbSet<Partida> Partidas { get; set; }
    public DbSet<Participacao> Participacoes { get; set; }
    public DbSet<Arbitragem> Arbitragens { get; set; }
    public DbSet<Denuncia> Denuncias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Participacao>()
                .HasKey(p => new { p.IdPartida, p.IdEquipe });

            modelBuilder.Entity<Arbitragem>()
                .HasKey(a => new { a.IdPartida, a.IdArbitro });

            modelBuilder.Entity<Denuncia>()
                .HasIndex(d => d.Protocolo)
                .IsUnique();

            modelBuilder.Entity<Campeonato>()
                .Property(c => c.Categoria)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<Arbitro>()
                .Property(a => a.Status)
                .HasDefaultValueSql("'Ativo'")
                .HasMaxLength(20);

            modelBuilder.Entity<Denuncia>()
                .Property(d => d.Status)
                .HasDefaultValueSql("'Em Análise'")
                .HasMaxLength(20);

            base.OnModelCreating(modelBuilder);
        }
    }
}
