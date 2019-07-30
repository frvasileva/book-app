using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApp.API.Dtos;
using Neo4jClient;

namespace BookApp.API.Data {
  public class BookGraphRepository : IBookGraphRepository {
    private readonly IGraphClient _graphClient;
    public BookGraphRepository (IGraphClient graphClient) {
      _graphClient = graphClient;
      _graphClient.Connect ();
    }

    public BookCreateDto AddBook (BookCreateDto book) {

      var result = _graphClient.Cypher
        .Create ("(book:Book {bookDto})")
        .WithParam ("bookDto", book).Return<Node<BookCreateDto>> ("book").Results.Single ();

      return new BookCreateDto ();
    }

    public Task<List<BookPreviewDto>> GetAll () {
      throw new NotImplementedException ();
    }

    public Task<BookDetailsDto> GetBook (string friendlyUrl) {
      throw new NotImplementedException ();
    }

    public Task<List<BookPreviewDto>> GetBooksAddedByUser (string friendlyUrl) {
      throw new NotImplementedException ();
    }
  }
}