using System;
using System.Collections.Generic;
using BookApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neo.Models;
using Neo4jClient;

namespace BookApp.API.Controllers {

  [Route ("api/[controller]")]
  [ApiController]
  [AllowAnonymous]
  public class BookGraphController : ControllerBase {

    public BookGraphController () {

    }

    [HttpGet ("get-movies")]
    public IEnumerable<Movie> GetMovies () {

      var client = new GraphClient (new Uri ("http://localhost:7474/db/data"), "neo4j", "parola");
      client.Connect ();

      var newMovie = new Movie { Title = "Fanka", Released = 1900, TagLine = "Bla bla tag line" };
      var newBook = new Book { Title = "Winnie the Pooh", AddedOn = DateTime.Now, AvarageRating = 100, Description = "Winnie the Pooh description" };
      client.Cypher
        .Create ("(book:FANKA {newBook})")
        .WithParam ("newBook", newBook)
        .ExecuteWithoutResults ();

      var movies = client.Cypher
        .Match ("(m:Movie)")
        .Return (m => m.As<Movie> ())
        .Limit (10)
        .Results;

      return movies;
    }
  }
}