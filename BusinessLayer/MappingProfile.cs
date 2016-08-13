using System;
using System.Linq;
using AutoMapper;
using BusinessLayer.DTOs;
using DataLayer.DomainModels;

namespace BusinessLayer
{
    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Article, ArticleDTO>()
                .ForMember(dest => dest.Issue, opt => opt.MapFrom(src => src.Issue.GetEnumDescription()))
                .ForMember(dest => dest.PublicationYear, opt => opt.MapFrom(src => src.PublicationYear.GetEnumDescription()))
                .ForMember(dest => dest.Authors, opt => opt.MapFrom(src => src.Authors.Select(author => author.FullName)))
                .ForMember(dest => dest.Subjects, opt => opt.MapFrom(src => src.Subjects.Select(subject => subject.Name)));

            //CreateMap<ArticleDTO, Article>()
            //    .ForMember(dest => dest.Issue, opt => opt.MapFrom(src => (Issues) Enum.Parse(typeof(Issues), src.Issue)))
            //    .ForMember(dest => dest.PublicationYear, opt => opt.MapFrom(src => (PublicationYears) Enum.Parse(typeof(PublicationYears), src.PublicationYear)));

            //CreateMap<Author, AuthorDTO>()
            //    .ForMember(dest => dest.Suffix, opt => opt.MapFrom(src => src.Suffix.GetEnumDescription()))
            //    .ForMember(dest => dest.Articles, opt => opt.MapFrom(src => src.Articles.Select(article => article.Title)));
            CreateMap<Author, AuthorDTO>()
                .ForMember(dest => dest.Articles, opt => opt.MapFrom(src => src.Articles.Select(article => article.Title)));

            //CreateMap<AuthorDTO, Author>()
            //    .ForMember(dest => dest.Suffix, opt => opt.MapFrom(src => (Suffixes) Enum.Parse(typeof(Suffixes), src.Suffix)));

            CreateMap<Subject, SubjectDTO>()
                .ForMember(dest => dest.Articles, opt => opt.MapFrom(src => src.Articles.Select(article => article.Title)));

            //CreateMap<SubjectDTO, Subject>();
        }
    }
}
