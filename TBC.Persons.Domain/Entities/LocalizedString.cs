namespace TBC.Persons.Domain.Entities;

public class LocalizedString
{
    public int Id { get; set; }
    public string Culture { get; set; }
    public string ResourceKey { get; set; }
    public string ResourceValue { get; set; }
}