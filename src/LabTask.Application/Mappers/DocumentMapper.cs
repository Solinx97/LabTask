using AutoMapper;
using LabTask.Application.DTOs;
using LabTask.Domain.Aggregates;
using LabTask.Domain.Entities;

namespace LabTask.Application.Mappers;

public class DocumentMapper : Profile
{
    public DocumentMapper()
    {
        CreateMap<DocumentDto, Document>().ReverseMap();
        CreateMap<CommentDto, Comment>().ReverseMap();
        CreateMap<LinkDto, Link>().ReverseMap();
    }
}
