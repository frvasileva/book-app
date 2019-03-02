using AutoMapper;
using BookApp.API.Dtos;
using BookApp.API.Models;

namespace BookApp.API.Helpers
{
  public class AutoMapperProfiles : Profile
  {

    public AutoMapperProfiles()
    {
      CreateMap<Book, BookDetailsDto>();
      CreateMap<Book, BookCreateDto>();
      CreateMap<Book, BookPreviewDto>();
    }
  }
}