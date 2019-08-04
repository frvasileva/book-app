using AutoMapper;
using BookApp.API.Dtos;
using BookApp.API.Models;
using Neo4jClient;

namespace BookApp.API.Helpers {
  public class AutoMapperProfiles : Profile {

    public AutoMapperProfiles () {
      CreateMap<BookCreateDto, Book> ()
        .ForMember (dest => dest.FriendlyUrl, opt => {
          opt.MapFrom (src => Url.GenerateFriendlyUrl (src.Title));
        });

      CreateMap<Book, BookDetailsDto> ().ForMember (dest => dest.PhotoPath, opt => {
        opt.MapFrom (src => this.PhotoUrlMap (src.PhotoPath));
      });

      CreateMap<Book, BookPreviewDto> ().ForMember (dest => dest.PhotoPath, opt => {
        opt.MapFrom (src => this.PhotoUrlMap (src.PhotoPath));
      });
      CreateMap<BookPreviewDto, Book> ();

      CreateMap<BookItemDto, Book> ();
      CreateMap<Book, BookItemDto> ();

      CreateMap<BookItemDto, BookCreateDto> ();
      CreateMap<BookCreateDto, BookItemDto> ();

      CreateMap<AuthorCreateDto, Author> ();
      CreateMap<Author, AuthorCreateDto> ();

      CreateMap<Catalog, CatalogCreateDto> ();
      CreateMap<CatalogCreateDto, Catalog> ();

      CreateMap<CatalogPureDto, Catalog> ();
      CreateMap<Catalog, CatalogPureDto> ();

      CreateMap<BookCatalog, BookListItemDto> ();
      CreateMap<BookListItemDto, BookCatalog> ();

      CreateMap<BookCatalog, BookCatalogCreateDto> ();
      CreateMap<BookCatalogCreateDto, BookCatalog> ();

      CreateMap<BookCatalogListDto, BookCatalog> ();
      CreateMap<BookCatalog, BookCatalogListDto> ();

      CreateMap<User, UserForRegisterDto> ();
      CreateMap<UserForRegisterDto, User> ();

      CreateMap<UserProfileDto, User> ();
      CreateMap<User, UserProfileDto> ();

      CreateMap<User, UserForLoginDto> ();
      CreateMap<UserForLoginDto, User> ();

      CreateMap<User, UserProfileEditDto> ();
      CreateMap<UserProfileEditDto, User> ();

      CreateMap<Photo, PhotoForReturnDto> ();
      CreateMap<PhotoForCreationDto, Photo> ();
      CreateMap<Photo, PhotoForCreationDto> ();

      CreateMap<Node<ProfileDto>, ProfileDto> ();
      CreateMap<ProfileDto, Node<ProfileDto>> ();

      CreateMap<Node<BookDetailsDto>, BookDetailsDto> ();
      CreateMap<BookDetailsDto, Node<BookDetailsDto>> ();
    }

    private string PhotoUrlMap (string photoPath) {
      if (photoPath == "" || photoPath == null) {
        return "http://www.prakashgold.com/Images/noimg.jpg";
      }

      return photoPath;
    }

  }
}