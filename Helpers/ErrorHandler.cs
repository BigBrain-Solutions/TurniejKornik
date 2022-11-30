using KornikTournament.Enums;

namespace KornikTournament.Helpers;

public static class ErrorHandler
{
    public static string HandleError(ErrorTypes? type)
    {
        return type switch
        {
            ErrorTypes.WrongPassword => "Złe hasło",
            ErrorTypes.NullForm => "Wypełnij formularz",
            ErrorTypes.TooManyTeams => "Próbujesz stworzyć zbyt dużo drużyn",
            ErrorTypes.AgentIsOnTheList => "Agent jest już na liście",
            ErrorTypes.TeamExists => "Drużyna już istnieje",
            ErrorTypes.ClassNotValid => "Zły format klasy lub klasa nie istnieje",
            ErrorTypes.TeamDeleted => "Usunąłeś drużyne",
            ErrorTypes.ParticipantDeleted => "Usunąłeś osobę z drużyny",
            _ => string.Empty
        };
    }
}