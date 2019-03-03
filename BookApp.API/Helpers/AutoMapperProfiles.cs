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
      CreateMap<Book, BookDetailsDto>();
      CreateMap<Book, BookPreviewDto>();
    }
  }
}