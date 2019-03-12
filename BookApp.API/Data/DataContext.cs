using BookApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApp.API.Data
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    { }

    public DbSet<Value> Values { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Quote> Quotes { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<BookListActions> BookListActions { get; set; }

    public DbSet<BookCatalog> BookCatalog { get; set; }

    // public DbSet<Message> Messages { get; set; }

  }
}