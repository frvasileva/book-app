using BookApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApp.API.Data {
  public class DataContext : DbContext {
    public DataContext (DbContextOptions<DataContext> options) : base (options) { }

    protected override void OnModelCreating (ModelBuilder modelBuilder) {

      modelBuilder.Entity<BookCatalog> ().HasKey (sc => new { sc.BookId, sc.CatalogId });

      modelBuilder.Entity<BookCatalog> ()
        .HasOne (bc => bc.Book)
        .WithMany (b => b.BookCatalogs)
        .HasForeignKey (bc => bc.BookId)
        .OnDelete (DeleteBehavior.SetNull);

      modelBuilder.Entity<BookCatalog> ()
        .HasOne (bc => bc.Book)
        .WithMany (c => c.BookCatalogs)
        .HasForeignKey (bc => bc.CatalogId)
        .OnDelete (DeleteBehavior.SetNull);

      // modelBuilder.Entity<BookAuthor> ()
      //   .HasOne (pt => pt.Book)
      //   .WithMany (p => p.AuthorsLink)
      //   .HasForeignKey (pt => pt.BookId);

      // modelBuilder.Entity<BookAuthor> ()
      //   .HasOne (pt => pt.Author)
      //   .WithMany (t => t.BooksLink)
      //   .HasForeignKey (pt => pt.AuthorId);
    }

    public DbSet<Value> Values { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Quote> Quotes { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<BookListActions> BookListActions { get; set; }
    public DbSet<Catalog> Catalogs { get; set; }
    public DbSet<BookCatalog> BookCatalog { get; set; }
  }
}