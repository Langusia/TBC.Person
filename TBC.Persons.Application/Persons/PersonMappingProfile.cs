using AutoMapper;
using TBC.Persons.Application.Persons.Commands.Create;
using TBC.Persons.Application.Persons.Commands.Update;

namespace TBC.Persons.Application.Persons;

public class PersonMappingProfile : Profile
{
    public PersonMappingProfile()
    {
        CreateMap<CreatePersonCommand, Person>();
        CreateMap<UpdatePersonCommand, Person>()
            .ForMember(x => x.PersonalNumber, opts =>
                opts.Condition(x => !string.IsNullOrWhiteSpace(x.PersonalNumber)))
            .ForMember(x => x.FirstName, opts =>
                opts.Condition(x => !string.IsNullOrWhiteSpace(x.FirstName)))
            .ForMember(x => x.LastName, opts =>
                opts.Condition(x => !string.IsNullOrWhiteSpace(x.LastName)))
            .ForMember(x => x.Gender, opts =>
                opts.Condition(x => x.Gender is not null))
            .ForMember(x => x.DateOfBirth, opts =>
                opts.Condition(x => x.DateOfBirth is not null))
            .ForMember(x => x.CityId, opts =>
                opts.Condition(x => x.CityId is not null))
            .ForMember(x => x.PhoneNumbers, opts =>
                opts.Condition(x => x.PhoneNumbers is not null));
        ;
    }
}