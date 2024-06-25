using Checklist_API.Features.Checklists.Entity;
using Checklist_API.Features.JWT.Entity;
using Checklist_API.Features.Users.Entity;
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

        modelBuilder.Entity<CheckList>()  // Strongly typed id deklarert
            .Property(x => x.Id)
            .HasConversion(
                id => id.checklistId,
                value => new ChecklistId(value)
         );

        modelBuilder.Entity<CheckList>()  // Strongly typed id deklarert
            .Property(x => x.UserId)
            .HasConversion(
               id => id.userId,
               value => new UserId(value)
         );

        modelBuilder.Entity<CheckList>()  // set up primary key
            .HasKey(x => x.Id);

        modelBuilder.Entity<CheckList>() // setter opp relationship og foreignkey
            .HasOne(x => x.User)
            .WithMany(x => x.Checklists)
            .HasForeignKey(u => u.UserId);

        modelBuilder.Entity<CheckList>() // validering med FluentAPI
            .Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<CheckList>() // validering med FluentAPI
            .Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<CheckList>() // validering med FluentAPI
            .Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<CheckList>() // validering med FluentAPI
            .Property(x => x.Priority)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<CheckList>() // validering med FluentAPI
            .Property(x => x.AssignedTo)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<CheckList>() // validering med FluentAPI
            .Property(x => x.Comments)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<CheckList>() // validering med FluentAPI
            .Property(x => x.DueDate)
            .IsRequired();


        modelBuilder.Entity<CheckList>() // validering med FluentAPI
            .Property(x => x.DateCreated)
            .IsRequired();

        modelBuilder.Entity<CheckList>() // validering med FluentAPI
            .Property(x => x.DateUpdated)
            .IsRequired(); 
        
        modelBuilder.Entity<CheckList>() // validering med FluentAPI
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

        modelBuilder.Entity<User>()
          .HasKey(x => x.Id);


        modelBuilder.Entity<User>()
           .HasMany(u => u.Checklists)
           .WithOne(c => c.User);
        // .HasForeignKey(u => u.UserId);

        modelBuilder.Entity<User>()
            .Property(x => x.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(x => x.LastName)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(x => x.PhoneNumber)
            .HasMaxLength(8)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(x => x.Email)
            .HasMaxLength(30)
            .IsRequired();

        modelBuilder.Entity<User>()
           .Property(x => x.HashedPassword)
           .IsRequired();

        modelBuilder.Entity<User>()
           .Property(x => x.Salt)
           .IsRequired();

        modelBuilder.Entity<User>() 
            .Property(x => x.DateCreated)
            .IsRequired();

        modelBuilder.Entity<User>() 
            .Property(x => x.DateUpdated)
            .IsRequired();


        #endregion

        #region JWTRole

        modelBuilder.Entity<JWTRole>()
          .Property(x => x.Id)
          .HasConversion(
              id => id.jwtRoleId,
              value => new JwtRoleId(value)
          );

        modelBuilder.Entity<JWTRole>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<JWTRole>()
            .HasMany(x => x.JWTUserRoles)
            .WithOne(x => x.JWTRole);

        modelBuilder.Entity<JWTRole>()
            .Property(x => x.RoleName)
            .HasMaxLength(20)
            .IsRequired();

        modelBuilder.Entity<JWTRole>()
           .Property(x => x.DateCreated)
           .IsRequired();

        modelBuilder.Entity<JWTRole>()
            .Property(x => x.DateUpdated)
            .IsRequired();

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

        modelBuilder.Entity<JWTUserRole>()
        .Property(x => x.JwtRoleId)
        .HasConversion(
            id => id.jwtRoleId,
            value => new JwtRoleId(value));

        modelBuilder.Entity<JWTUserRole>()
         .HasKey(x => x.Id);

        modelBuilder.Entity<JWTUserRole>()
         .HasOne(j => j.JWTRole)
         .WithMany(j => j.JWTUserRoles)
         .HasForeignKey(j => j.JwtRoleId);

        modelBuilder.Entity<JWTUserRole>()
         .HasOne(u => u.User)
         .WithMany(j => j.JWTUserRoles)
         .HasForeignKey(u => u.UserId);

        modelBuilder.Entity<JWTUserRole>()
         .Property(x => x.DateCreated)
         .IsRequired();

        modelBuilder.Entity<JWTUserRole>()
         .Property(x => x.DateUpdated)
         .IsRequired();
        #endregion
    }
}
