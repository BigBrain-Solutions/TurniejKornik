using System.Text.RegularExpressions;

namespace KornikTournament.Helpers;

public static class EmailValidatorHelper
{
    public static bool ValidateEmail(this string email)
    {
        var regex = new Regex(@"^[\w\.]+@([\w-]+\.)+[\w-]{2,4}$");

        return regex.IsMatch(email);
    }
}