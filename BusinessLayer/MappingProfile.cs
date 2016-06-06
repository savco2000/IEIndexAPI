using System;
using AutoMapper;
using BusinessLayer.ViewModels;
using DataLayer.DomainModels;

namespace BusinessLayer
{
    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Article, ArticleVM>()
                .ForMember(dest => dest.Issue, opt => opt.MapFrom(src => src.Issue.GetEnumDescription()))
                .ForMember(dest => dest.PublicationYear, opt => opt.MapFrom(src => src.PublicationYear.GetEnumDescription()));

            CreateMap<ArticleVM, Article>()
                .ForMember(dest => dest.Issue, opt => opt.MapFrom(src => (Issues) Enum.Parse(typeof(Issues), src.Issue)))
                .ForMember(dest => dest.PublicationYear, opt => opt.MapFrom(src => (PublicationYears) Enum.Parse(typeof(PublicationYears), src.PublicationYear)));

            CreateMap<Author, AuthorVM>()
                .ForMember(dest => dest.Suffix, opt => opt.MapFrom(src => src.Suffix.GetEnumDescription()));

            CreateMap<AuthorVM, Author>()
                .ForMember(dest => dest.Suffix, opt => opt.MapFrom(src => (Suffixes) Enum.Parse(typeof(Suffixes), src.Suffix)));

            CreateMap<Subject, SubjectVM>();

            CreateMap<SubjectVM, Subject>();
        }
    }
}
