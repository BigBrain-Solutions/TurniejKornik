# TurniejKornik
TurniejKornik is a application for managing ``league of legends`` tournament

### Application provides:

- Creating your team and assigning your teammates
- Tournament annoucments
- Free Agents

- Admin panel:
   - Creating annoucments
   - Managing all teams
  

## How to Install

### Without docker apporach

1. Open ``appsettings.json`` and ``appsettings.Development.json``
1. Change ``AdminPass`` to your own LONG >25 chars password

Admin pass is used to Log In to admin panel

1. Make sure you have Postgres running
1. In ``appsettings.json`` and ``appsettings.Development.json``

Change ``ConnectionStrings`` => ``DbString`` to: 
```
"Server=localhost;Port=5432;Database=KornikTournamentDb;User Id=postgres;Password=<your_password>"
```

1. Clone this repo
1. cd ./TurniejKornik
1. Update database with EF Core, if you dont have EF, <a href="https://learn.microsoft.com/en-us/ef/core/cli/dotnet"> install one here </a>
```bash
dotnet ef database update
```

1. Run the Application by:

```bash
dotnet run
```
 
## Tech:
- ASP.NET MVC 7.0
- Postgres Db
