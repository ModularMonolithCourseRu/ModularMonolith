using AutoMapper;
using Shop.Entities.Models;
using Shop.UseCases.Emails.Dtos;

namespace Shop.UseCases.Emails.Utils;

public class EmailsProfile : Profile
{
    public EmailsProfile()
    {
        CreateMap<Email, EmailDto>();
    }
}