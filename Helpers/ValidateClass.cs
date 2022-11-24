using System.Text.RegularExpressions;

namespace KornikTournament.Helpers;

public static class ValidateClass
{
    public static bool Validate(this string input)
    {
        var regex = new Regex("^[1-5][a-zA-Z][1]|^[1-5][a-zA-Z][a-zA-Z][1]|^[1-5][a-zA-Z][a-zA-Z]|^[1-5][a-zA-Z]");

        return regex.IsMatch(input);
    }
}