using Microsoft.EntityFrameworkCore;

namespace ContactosWEB.Models.Contexto
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Contacto> Contactos { get; set; }
    }
}
