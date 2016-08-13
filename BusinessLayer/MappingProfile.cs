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
            
            CreateMap<Author, AuthorDTO>()
                .ForMember(dest => dest.Articles, opt => opt.MapFrom(src => src.Articles.Select(article => article.Title)));
            
            CreateMap<Subject, SubjectDTO>()
                .ForMember(dest => dest.Articles, opt => opt.MapFrom(src => src.Articles.Select(article => article.Title)));
        }
    }
}
