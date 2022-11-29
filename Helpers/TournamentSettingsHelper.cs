using KornikTournament.Enums;
using KornikTournament.Models;
using Newtonsoft.Json;

namespace KornikTournament.Helpers;

public static class TournamentSettingsHelper
{
    public static TournamentSettings Read()
    {
        var path = Environment.CurrentDirectory + "/TournamentSettings.json";

        var json = File.ReadAllText(path);
        
        var settings = JsonConvert.DeserializeObject<TournamentSettings>(json);

        try
        {
            return settings ?? new TournamentSettings();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new TournamentSettings();
        }
    }

    public static void Write(TournamentSettings settings)
    {
        var path = Environment.CurrentDirectory + "/TournamentSettings.json";
        
        var json = JsonConvert.SerializeObject(settings);
        
        File.WriteAllText(path, json);
    }

    public static ETournamentState GetTournamentState()
    {
        var settings = Read();

        
        if (settings.StartDate.ToUniversalTime() < DateTime.Now.ToUniversalTime() && settings.EndDate.ToUniversalTime() > DateTime.Now.ToUniversalTime())
        {
            settings.Ended = false;
            Write(settings);
            return ETournamentState.Started;
        }

        if (settings.EndDate.ToUniversalTime() < DateTime.Now.ToUniversalTime())
        {
            settings.Ended = true;
            Write(settings);
            return ETournamentState.Ended;
        }
        
        return ETournamentState.OnGoing;
    }
}