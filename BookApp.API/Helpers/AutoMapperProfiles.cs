using AutoMapper;
using BookApp.API.Dtos;
using BookApp.API.Models;

namespace BookApp.API.Helpers
{
  public class AutoMapperProfiles : Profile
  {

    public AutoMapperProfiles()
    {
      CreateMap<BookCreateDto, Book>()
     .ForMember(dest => dest.FriendlyUrl, opt =>
      {
        opt.MapFrom(src => Url.GenerateFriendlyUrl(src.Title));
      });

      CreateMap<Book, BookDetailsDto>().ForMember(dest => dest.PhotoPath, opt =>
      {
        opt.MapFrom(src => this.PhotoUrlMap(src.PhotoPath));
      });

      CreateMap<Book, BookPreviewDto>().ForMember(dest => dest.PhotoPath, opt =>
      {
        opt.MapFrom(src => this.PhotoUrlMap(src.PhotoPath));
      });

      CreateMap<BookActionDto, BookListActions>();
      
      CreateMap<AuthorCreateDto, Author>();
      CreateMap<Author, AuthorCreateDto>();

      CreateMap<BookList, BookListCreateDto>();
      CreateMap<BookListCreateDto, BookList>();

      CreateMap<BookList, BookListItemDto>();
      CreateMap<BookListItemDto, BookList>();
    }

    private string PhotoUrlMap(string photoPath)
    {
      if (photoPath == "" && photoPath != null)
      {
        return "http://www.prakashgold.com/Images/noimg.jpg";
      }

      return photoPath;
    }

  }
}