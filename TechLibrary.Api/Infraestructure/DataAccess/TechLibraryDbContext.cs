using Microsoft.EntityFrameworkCore;
using TechLibrary.Api.Domain.Entities;

namespace TechLibrary.Api.Infraestructure;

// Contexto para o banco de dados
public class TechLibraryDbContext : DbContext
{
    // Referência para tabelas
    public DbSet<User> Users { get; set; }
    
    // Conexão com o banco de dados SQLite
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=DB/TechLibraryDb.db");
    }
}