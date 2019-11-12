using BookApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookApp.API.Data {
  public class DataContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>> {
    public DataContext (DbContextOptions<DataContext> options) : base (options) { }

    protected override void OnModelCreating (ModelBuilder modelBuilder) {

      base.OnModelCreating (modelBuilder);
      modelBuilder.Entity<UserRole> (userRole => {
        userRole.HasKey (ur => new { ur.UserId, ur.RoleId });

        userRole.HasOne (ur => ur.Role).WithMany (r => r.UserRoles)
          .HasForeignKey (ur => ur.RoleId).IsRequired ();
      });

      modelBuilder.Entity<BookCatalog> ().HasKey (sc => new { sc.BookId, sc.CatalogId });

      modelBuilder.Entity<BookCatalog> ()
        .HasOne (bc => bc.Book)
        .WithMany (b => b.BookCatalogs)
        .OnDelete (DeleteBehavior.Restrict);

      modelBuilder.Entity<BookCatalog> ()
        .HasOne (bc => bc.Book)
        .WithMany (c => c.BookCatalogs)
        .OnDelete (DeleteBehavior.Restrict);
    }

    public DbSet<Discussion> Discussions { get; set; }
    public DbSet<DiscussionItem> DiscussionItem { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<BookListActions> BookListActions { get; set; }
    public DbSet<UserFollowers> UserFollowers { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<BookTags> BookTags { get; set; }
    public DbSet<BookCatalogPreferences> BookCatalogPreferences { get; set; }
    public DbSet<UserBookCategoriesPreferences> UserBookCategoriesPreferences { get; set; }
  }
}