using TBC.Persons.Domain.Enums;

namespace TBC.Persons.Domain.Queries;

public record RelationReportItem
{
    public string FullNameGeorgian { get; init; }
    public string FullNameEnglish { get; init; }

    public RelationType RelationType { get; init; }

    public int RelationCount { get; init; }
}