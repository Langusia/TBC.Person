using TBC.Persons.Domain.Enums;

namespace TBC.Persons.Domain.Values;

public class MultiLanguageString
{
    protected MultiLanguageString()
    {
    }

    public MultiLanguageString(string georgian, string english)
        : this()
    {
        Georgian = georgian;
        English = english;
    }

    public string Georgian { get; private set; }

    public string English { get; private set; }

    public string Translate(Language language) => language switch
    {
        Language.Georgian => Georgian,
        Language.English => English,
        _ => throw new Exception("Not Supported Language")
    };
}