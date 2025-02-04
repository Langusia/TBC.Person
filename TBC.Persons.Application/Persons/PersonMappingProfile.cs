using AutoMapper;
using TBC.Persons.Application.Persons.Commands.Create;
using TBC.Persons.Application.Persons.Commands.Update;
using TBC.Persons.Domain.Entities;

namespace TBC.Persons.Application.Persons;

public class PersonMappingProfile : Profile
{
    public PersonMappingProfile()
    {
        CreateMap<CreatePersonCommand, Person>()
            .ForPath(x => x.FirstName.Georgian, opts =>
                opts.MapFrom(src => src.FirstName))
            .ForPath(x => x.FirstName.English, opts =>
                opts.MapFrom(src => src.FirstNameEng))
            .ForPath(x => x.LastName.Georgian, opts =>
                opts.MapFrom(src => src.LastName))
            .ForPath(x => x.LastName.English, opts =>
                opts.MapFrom(src => src.LastNameEng));

        CreateMap<UpdatePersonCommand, Person>()
            .ForPath(x => x.FirstName.Georgian, opts =>
            {
                opts.Condition(x => x.Source.FirstName is not null);
                opts.MapFrom(src => src.FirstName);
            })
            .ForPath(x => x.FirstName.English, opts =>
            {
                opts.Condition(x => x.Source.FirstNameEng is not null);
                opts.MapFrom(src => src.FirstNameEng);
            })
            .ForPath(x => x.LastName.Georgian, opts =>
            {
                opts.Condition(x => x.Source.LastName is not null);
                opts.MapFrom(src => src.LastName);
            })
            .ForPath(x => x.LastName.English, opts =>
            {
                opts.Condition(x => x.Source.LastNameEng is not null);
                opts.MapFrom(src => src.LastNameEng);
            })
            .ForMember(x => x.PersonalNumber, opts =>
                opts.PreCondition(x => !string.IsNullOrWhiteSpace(x.PersonalNumber)))
            .ForMember(x => x.Gender, opts =>
                opts.PreCondition(x => x.Gender is not null))
            .ForMember(x => x.DateOfBirth, opts =>
                opts.PreCondition(x => x.DateOfBirth is not null))
            .ForMember(x => x.CityId, opts =>
                opts.PreCondition(x => x.CityId is not null))
            .ForMember(x => x.PhoneNumbers, opts =>
                opts.PreCondition(x => x.PhoneNumbers is not null));
    }
}