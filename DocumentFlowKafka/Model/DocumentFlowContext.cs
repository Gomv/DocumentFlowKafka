using Microsoft.EntityFrameworkCore;

namespace DocumentFlowKafka.Model
{
    public class DocumentFlowContext : DbContext
    {
        public DbSet<Documents> Documents { get; set; }
        public DbSet<Flows> Flows { get; set; }
        public DbSet<Type> Type { get; set; }
        public DbSet<Users> Users { get; set; }
        public DocumentFlowContext(DbContextOptions<DocumentFlowContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
