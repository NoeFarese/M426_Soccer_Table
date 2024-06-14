namespace soccer_table;

public class Program
{
    public static void Main(string[] args)
    {
        List<string> log = [];
        log.Add("Bitte geben Sie den Pfad zum Ordner der Liga ein:");
        Console.WriteLine(log[0]);
        string? leagueFolder = "";
        while (string.IsNullOrEmpty(leagueFolder))
        {
            leagueFolder = Console.ReadLine();
            if (string.IsNullOrEmpty(leagueFolder))
            {
                log[0] = "Input war null oder leer. Bitte geben Sie einen gültigen Pfad ein.";
                DisplayLog(log);
            }

            if (!Directory.Exists(leagueFolder))
            {
                log[0] = "Der Order konnte nicht gefunden werden. Bitte geben Sie einen existierenden Order ein.";
                DisplayLog(log);
                leagueFolder = "";
            }
        }

        log.Add(leagueFolder);

        log.Add("Geben Sie den gewünschten Spieltag ein (optional, Leer lassen für alle):");
        DisplayLog(log);
        string? inputDay = Console.ReadLine();
        int? targetDay = (int?)null;
        if (!string.IsNullOrEmpty(inputDay))
        {
            if (int.TryParse(inputDay, out int tempDay))
            {
                targetDay = tempDay;
            }
            else
            {
                while (!int.TryParse(inputDay, out int tempDay2))
                {
                    log[2] = "Ungültige Eingabe. Bitte geben Sie eine Zahl eine oder lassen Sie die Eingabe leer";
                    DisplayLog(log);
                    inputDay = Console.ReadLine();
                }

                targetDay = int.Parse(inputDay);
            }
        }

        List<Team> leagueTable = GenerateLeagueTable(leagueFolder, targetDay);

        DisplayLeagueTable(leagueTable);
    }

    public static List<Team> GenerateLeagueTable(string leagueFolder, int? targetDay)
    {
        List<Team> teams = new List<Team>();
        string[] dayFiles = Directory.GetFiles(leagueFolder, "day*.txt");

        foreach (string dayFile in dayFiles)
        {
            int day = int.Parse(Path.GetFileNameWithoutExtension(dayFile).Substring(3));
            if (targetDay.HasValue && day > targetDay)
                break;

            string[] lines = File.ReadAllLines(dayFile);
            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                string homeTeamName = parts[0].Substring(0, parts[0].Length - 2).Trim();
                string awayTeamName = parts[1].Substring(1).Trim();
                int homeGoals = int.Parse(parts[0].Substring(parts[0].Length - 1));
                int awayGoals = int.Parse(parts[1].Substring(0, 1));

                UpdateTeamStats(teams, homeTeamName, homeGoals, awayGoals);
                UpdateTeamStats(teams, awayTeamName, awayGoals, homeGoals);
            }
        }

        // Sort by points, goal difference, wins, and name
        return teams.OrderByDescending(t => t.Points)
            .ThenByDescending(t => t.GoalDifference)
            .ThenByDescending(t => t.Wins)
            .ThenBy(t => t.Name)
            .ToList();
    }

    public static void UpdateTeamStats(List<Team> teams, string teamName, int goalsFor, int goalsAgainst)
    {
        Team? team = teams.FirstOrDefault(t => t.Name == teamName);
        if (team == null)
        {
            team = new Team { Name = teamName };
            teams.Add(team);
        }

        team.GoalsFor += goalsFor;
        team.GoalsAgainst += goalsAgainst;

        if (goalsFor > goalsAgainst)
            team.Wins++;
        else if (goalsFor < goalsAgainst)
            team.Losses++;
        else
            team.Draws++;

        team.Points = team.Wins * 3 + team.Draws;
    }

    public static void DisplayLeagueTable(List<Team> teams)
    {
        Console.WriteLine("{0,4}  {1,-20}{2,7} {3,4} {4,4} {5,4} {6,4} {7,4} {8,4}",
            "Rang", "Name", "Punkte", "S", "N", "U", "T+", "T-", "TD");

        int rank = 1;
        foreach (var team in teams)
        {
            Console.WriteLine("{0,4}  {1,-20}{2,7} {3,4} {4,4} {5,4} {6,4} {7,4} {8,4}",
                rank++, team.Name, team.Points, team.Wins, team.Losses, team.Draws, team.GoalsFor, team.GoalsAgainst,
                team.GoalDifference);
        }

        Console.ReadKey();
    }

    public static void DisplayLog(List<string> log)
    {
        Console.Clear();
        for (int i = 0; i < log.Count; i++)
        {
            Console.WriteLine(log[i]);
        }
    }
}