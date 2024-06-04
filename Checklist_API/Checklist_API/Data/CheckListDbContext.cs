using Check_List_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace Check_List_API.Data;

public class CheckListDbContext : DbContext
{
    public CheckListDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<CheckList> CheckList { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);      

        modelBuilder.Entity<CheckList>()
            .Property(x => x.Id)
            .HasConversion(
                id => id.checklistId,
                value => new ChecklistId(value)
            );
    }
}
