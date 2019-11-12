using System;
using System.Linq;
using BookApp.API.Dtos;
using BookApp.API.Helpers;
using BookApp.API.Models;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Dtos;
using BookApp.API.Helpers;
using BookApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApp.API.Data {
  public partial class GraphRepository : IGraphRepository {

    #region ImportData
    public void ImportBooks () {
      var strFilePath = "D:\\diploma\\diploma\\BookApp.API\\BookDataImports\\books.csv";
      var data = ConvertCSVtoDataTable (strFilePath);
      var fakeBook = new Book ();
      for (int i = 0; i < data.Rows.Count; i++) {
        var item = data.Rows[i];

        var book = new Book ();
        if (book.Title != "")
          book.Title = item.ItemArray[9].ToString ();
        else {
          book.Title = item.ItemArray[10].ToString ();
        }
        book.Description = item.ItemArray[10].ToString ();
        book.PhotoPath = item.ItemArray[21].ToString ();
        book.FriendlyUrl = Url.GenerateFriendlyUrl (item.ItemArray[9].ToString ());
        book.PublisherId = 0;
        book.AuthorId = 0;
        book.AddedOn = DateTime.Now;
        book.Description = item.ItemArray[10].ToString ();
        book.ExternalId = Int32.Parse (item.ItemArray[1].ToString ());
        book.ISBN = item.ItemArray[5].ToString ();
        book.AvarageRating = Convert.ToDouble (item.ItemArray[12]);
        book.UserId = 0;

        var authorName = item.ItemArray[7].ToString ();
        //var author = this.AddAuthor (authorName, book.Id, book.UserId);
        //book.AuthorId = author.Id;

        this.AddBook (book);

        var author = this.AddAuthor (authorName, book.Id, book.UserId);
        //TODO: Update author ID in book object
      }
    }
    public void ImportTags () {
      var strFilePath = "D:\\diploma\\diploma\\BookApp.API\\BookDataImports\\tags.csv";
      var data = ConvertCSVtoDataTable (strFilePath);
      var fakeTag = new Tag ();
      for (int i = 0; i < data.Rows.Count; i++) {
        var item = data.Rows[i];

        var catalog = new Catalog ();
        catalog.AddedOn = DateTime.Now;
        catalog.Name = item.ItemArray[1].ToString ();
        catalog.FriendlyUrl = Url.GenerateFriendlyUrl (item.ItemArray[1].ToString ());
        catalog.ExternalId = Int32.Parse (item.ItemArray[0].ToString ());
        catalog.UserId = 0;
        catalog.IsPublic = true;

        var result = _graphClient.Cypher
          .Create ("(catalog:Catalog {catalog})")
          .WithParam ("catalog", catalog)
          .Return<Node<CatalogCreateDto>> ("catalog").Results.Single ().Data;

        _graphClient.Cypher
          .Match ("(profile:Profile)", "(cat:Catalog)")
          .Where ((ProfileDto profile) => profile.Id == catalog.UserId)
          .AndWhere ((CatalogCreateDto cat) => cat.Id == catalog.Id)
          .CreateUnique ("(profile)-[r:CATALOG_ADDED {date}]->(cat)")
          .WithParam ("date", new { addedOn = DateTime.Now }).ExecuteWithoutResults ();
      }
    }
    public void ImportBookTags () {
      var strFilePath = "D:\\diploma\\diploma\\BookApp.API\\BookDataImports\\book_tags.csv";
      var data = ConvertCSVtoDataTable (strFilePath);
      var fakeBookTag = new BookTags ();
      for (int i = 0; i < data.Rows.Count; i++) {
        var item = data.Rows[i];
        var bookTag = new BookTags ();

        var bookExternalId = Int32.Parse (item.ItemArray[0].ToString ());
        var categoryExternalId = Int32.Parse (item.ItemArray[1].ToString ());
        bookTag.Count = Int32.Parse (item.ItemArray[2].ToString ());
        bookTag.AddedOn = DateTime.Now;

        _graphClient.Cypher
          .Match ("(book:Book)", "(catalog:Catalog)")
          .Where ((Book book) => book.ExternalId == bookExternalId)
          .AndWhere ((Catalog catalog) => catalog.ExternalId == categoryExternalId)
          .Create ("(book)-[r:BOOK_ADDED_TO_CATALOG {info}]->(catalog)")
          .WithParam ("info", new { addedOn = DateTime.Now, userId = 0 })
          .ExecuteWithoutResults ();
        // .Return ((catalog, book, r) => new {
        //   cat = catalog.As<Catalog> (),
        //     bk = book.As<Book> ()
        // });
        // var res = result.Return.result.;
        //   this.AddCatalog()
      }
    }
        #endregion ImportData

        #region Helpers
        private static DataTable ConvertCSVtoDataTable(string strFilePath = "")
        {

            StreamReader sr = new StreamReader(strFilePath);
            string[] headers = sr.ReadLine().Split(',');
            DataTable dt = new DataTable();
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }

        #endregion Helpers
    }
}