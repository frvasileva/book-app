using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Dtos;
using BookApp.API.Helpers;
using BookApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApp.API.Dtos {
  public class CatalogItemDto {

    public CatalogItemDto () {
      this.Books = new List<BookItemDto> ();
    }
    
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsPublic { get; set; }
    public int UserId { get; set; }
    public string FriendlyUrl { get; set; }
    public string UserFriendlyUrl { get; set; }
    public DateTime Created { get; set; }
    public List<BookItemDto> Books { get; set; }
  }
}

// id?: number;
//   name: string;
//   isPublic: boolean;
//   userId: number;
//   userFriendlyUrl: string;
//   books: Book[];
//   friendlyUrl: string;
//   user: any;
//   created: Date;