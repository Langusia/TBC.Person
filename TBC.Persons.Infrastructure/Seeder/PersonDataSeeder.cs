using Microsoft.EntityFrameworkCore;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Enums;
using TBC.Persons.Domain.Values;
using TBC.Persons.Infrastructure.Db.Contexts;

namespace TBC.Persons.Infrastructure.Seeder;

public class PersonDbDataSeeder
{
    public static async Task Seed(ApplicationDbContext context)
    {
        await PersonsSeeder.Seed(context);
        await CitiesSeeder.Seed(context);

        await context.SaveChangesAsync();
    }

    public static async Task Seed(ReportingDbContext context)
    {
        await PersonsSeeder.Seed(context);
        await CitiesSeeder.Seed(context);

        await context.SaveChangesAsync();
    }
}

public class CitiesSeeder
{
    public static async Task Seed(ApplicationDbContext context)
    {
        if (!await context.Set<City>().AnyAsync())
        {
            var cities = GetCities();

            await context.Set<City>().AddRangeAsync(cities);
        }
    }

    public static async Task Seed(ReportingDbContext context)
    {
        if (!await context.Set<City>().AnyAsync())
        {
            var cities = GetCities();

            await context.Set<City>().AddRangeAsync(cities);
        }
    }

    private static IEnumerable<City> GetCities()
    {
        return new List<City>
        {
            new() { Name = new MultiLanguageString("სვანეთი", "Svaneti") },
            new() { Name = new MultiLanguageString("რუსთავი", "Rustavi") },
            new() { Name = new MultiLanguageString("საქრეთი", "Sakreti") }
        };
    }
}

public class PersonsSeeder
{
    public static async Task Seed(ApplicationDbContext context)
    {
        if (!await context.Set<Person>().AnyAsync())
        {
            var persons = GetPersons();

            await context.Set<Person>().AddRangeAsync(persons);
        }
    }

    public static async Task Seed(ReportingDbContext context)
    {
        if (!await context.Set<Person>().AnyAsync())
        {
            var persons = GetPersons();

            await context.Set<Person>().AddRangeAsync(persons);
        }
    }

    private static IEnumerable<Person> GetPersons()
    {
        var tbilisiCity = new City
        {
            Name = new MultiLanguageString("თბილისი", "Tbilisi")
        };

        var kutaisiCity = new City
        {
            Name = new MultiLanguageString("ქუთაისი", "Kutaisi")
        };

        var tbilisiPhones = new List<PhoneNumber>
        {
            new()
            {
                Number = "557001001",
                Type = PhoneType.Mobile
            },
            new()
            {
                Number = "0322001001",
                Type = PhoneType.Office
            },
            new()
            {
                Number = "0322002002",
                Type = PhoneType.Home
            },
        };
        var kutaisiPhones = new List<PhoneNumber>
        {
            new()
            {
                Number = "557004004",
                Type = PhoneType.Mobile
            },
            new()
            {
                Number = "0322004004",
                Type = PhoneType.Office
            },
            new()
            {
                Number = "0322005005",
                Type = PhoneType.Home
            },
        };

        return new List<Person>
        {
            new()
            {
                FirstName = new MultiLanguageString("წრიპა", "Tsripa"),
                LastName = new MultiLanguageString("მალადოი", "Molodoy"),
                Gender = Gender.Male,
                DateOfBirth = new DateTime(1977, 7, 7),
                PersonalNumber = "22222222222",
                City = kutaisiCity,
                PhoneNumbers = kutaisiPhones
            },
            new()
            {
                FirstName = new MultiLanguageString("ჯონ", "John"),
                LastName = new MultiLanguageString("პროქსი", "Proxy"),
                Gender = Gender.Male,
                DateOfBirth = new DateTime(1977, 7, 7),
                PersonalNumber = "11111111111",
                City = tbilisiCity,
                PhoneNumbers = tbilisiPhones
            }
        };
    }
}