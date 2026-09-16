using Microsoft.EntityFrameworkCore;

namespace DocumentFlowKafka.Model
{
    public class DocumentFlowContext : DbContext
    {

        public DocumentFlowContext(DbContextOptions<DocumentFlowContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
