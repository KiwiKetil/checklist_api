using Checklist_API.Features.Checklists.Entity;
using Checklist_API.Features.Login.Entity;
using Checklist_API.Features.User.Entity;
using Microsoft.EntityFrameworkCore;

namespace Check_List_API.Data;

public class CheckListDbContext : DbContext
{
    public CheckListDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<CheckList> CheckList { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<JWTRole> JWTRole { get; set; }
    public DbSet<JWTUserRole> JWTUserRole { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region CheckList

        modelBuilder.Entity<CheckList>()
            .Property(x => x.Id)
            .HasConversion(
                id => id.checklistId,
                value => new ChecklistId(value)
         );

        modelBuilder.Entity<CheckList>()
            .Property(x => x.UserId)
            .HasConversion(
               id => id.userId,
               value => new UserId(value)
         );

        modelBuilder.Entity<CheckList>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<CheckList>()
            .HasOne(x => x.User)
            .WithMany(x => x.Checklists)
            .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<CheckList>()
            .Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<CheckList>()
            .Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<CheckList>()
            .Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<CheckList>()
            .Property(x => x.Priority)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<CheckList>()
            .Property(x => x.AssignedTo)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<CheckList>()
            .Property(x => x.Comments)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<CheckList>()
            .Property(x => x.DueDate)
            .IsRequired();


        modelBuilder.Entity<CheckList>()
            .Property(x => x.DateCreated)
            .IsRequired();

        modelBuilder.Entity<CheckList>()
            .Property(x => x.DateUpdated)
            .IsRequired(); 
        
        modelBuilder.Entity<CheckList>()
            .Property(x => x.DateCompleted)
            .IsRequired();

        #endregion

        #region User

        modelBuilder.Entity<User>()
          .Property(x => x.Id)
          .HasConversion(
              id => id.userId,
              value => new UserId(value)
          );

        #endregion

        #region JWTRole

        modelBuilder.Entity<JWTRole>()
          .Property(x => x.Id)
          .HasConversion(
              id => id.jwtRoleId,
              value => new JwtRoleId(value)
          );

        #endregion

        #region JWTUserRole

        modelBuilder.Entity<JWTUserRole>()
          .Property(x => x.Id)
          .HasConversion(
              id => id.jwtUserRoleId,
              value => new JwtUserRoleId(value)
          );

        modelBuilder.Entity<JWTUserRole>()
       .Property(x => x.UserId)
       .HasConversion(
           id => id.userId,
           value => new UserId(value)
       );

        #endregion
    }
}
