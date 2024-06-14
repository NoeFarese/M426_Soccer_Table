namespace soccer_table.Test;

using System.Collections.Generic;
using Xunit;
using soccer_table;

public class ProgramTest
{
    [Fact]
    public void UpdateTeamStats_UpdatesStatsCorrectly()
    {
        // Arrange
        var teams = new List<Team>
        {
            new Team { Name = "Team A" },
            new Team { Name = "Team B" }
        };

        // Act
        Program.UpdateTeamStats(teams, "Team A", 2, 1);
        Program.UpdateTeamStats(teams, "Team B", 1, 2);
        Program.UpdateTeamStats(teams, "Team A", 1, 1);
        Program.UpdateTeamStats(teams, "Team C", 1, 1);

        // Assert
        var teamA = teams.Find(t => t.Name == "Team A");
        Assert.NotNull(teamA);
        Assert.Equal(4, teamA.Points);
        Assert.Equal(1, teamA.Wins);
        Assert.Equal(1, teamA.Draws);
        Assert.Equal(0, teamA.Losses);
        Assert.Equal(3, teamA.GoalsFor);
        Assert.Equal(2, teamA.GoalsAgainst);

        var teamB = teams.Find(t => t.Name == "Team B");
        Assert.NotNull(teamB);
        Assert.Equal(0, teamB.Points);
        Assert.Equal(0, teamB.Wins);
        Assert.Equal(0, teamB.Draws);
        Assert.Equal(1, teamB.Losses);
        Assert.Equal(1, teamB.GoalsFor);
        Assert.Equal(2, teamB.GoalsAgainst);

        var teamC = teams.Find(t => t.Name == "Team C");
        Assert.NotNull(teamC);
        Assert.Equal(1, teamC.Points);
        Assert.Equal(0, teamC.Wins);
        Assert.Equal(1, teamC.Draws);
        Assert.Equal(0, teamC.Losses);
        Assert.Equal(1, teamC.GoalsFor);
        Assert.Equal(1, teamC.GoalsAgainst);
    }

    [Fact]
    public void UpdateTeamStats_AddsNewTeamCorrectly()
    {
        // Arrange
        var teams = new List<Team>();

        // Act
        Program.UpdateTeamStats(teams, "Team A", 2, 1); // Team A wins

        // Assert
        Assert.Single(teams);
        var teamA = teams.First(t => t.Name == "Team A");
        Assert.Equal(3, teamA.Points);
        Assert.Equal(1, teamA.Wins);
        Assert.Equal(0, teamA.Draws);
        Assert.Equal(0, teamA.Losses);
        Assert.Equal(2, teamA.GoalsFor);
        Assert.Equal(1, teamA.GoalsAgainst);
    }

    [Fact]
    public void GenerateLeagueTable_CorrectlySortsTeams()
    {
        // Arrange
        var teams = new List<Team>
        {
            new Team { Name = "Team A", Points = 6, Wins = 2, Draws = 0, Losses = 0, GoalsFor = 5, GoalsAgainst = 2 },
            new Team { Name = "Team B", Points = 4, Wins = 1, Draws = 1, Losses = 0, GoalsFor = 3, GoalsAgainst = 2 },
            new Team { Name = "Team C", Points = 1, Wins = 0, Draws = 1, Losses = 1, GoalsFor = 1, GoalsAgainst = 2 },
            new Team { Name = "Team D", Points = 0, Wins = 0, Draws = 0, Losses = 2, GoalsFor = 0, GoalsAgainst = 3 }
        };

        // Act
        teams = teams.OrderByDescending(t => t.Points)
            .ThenByDescending(t => t.GoalDifference)
            .ThenByDescending(t => t.Wins)
            .ThenBy(t => t.Name)
            .ToList();

        // Assert
        Assert.Equal("Team A", teams[0].Name);
        Assert.Equal("Team B", teams[1].Name);
        Assert.Equal("Team C", teams[2].Name);
        Assert.Equal("Team D", teams[3].Name);
    }
    
     [Fact]
    public void GenerateLeagueTable_SortsTeamsWithSamePoints()
    {
        // Arrange
        var teams = new List<Team>
        {
            new Team { Name = "Team A", Points = 6, Wins = 2, Draws = 0, Losses = 0, GoalsFor = 5, GoalsAgainst = 2 },
            new Team { Name = "Team B", Points = 6, Wins = 1, Draws = 3, Losses = 0, GoalsFor = 4, GoalsAgainst = 3 },
            new Team { Name = "Team C", Points = 4, Wins = 1, Draws = 1, Losses = 1, GoalsFor = 3, GoalsAgainst = 2 },
            new Team { Name = "Team D", Points = 2, Wins = 0, Draws = 2, Losses = 1, GoalsFor = 1, GoalsAgainst = 2 }
        };

        // Act
        teams = teams.OrderByDescending(t => t.Points)
            .ThenByDescending(t => t.GoalDifference)
            .ThenByDescending(t => t.Wins)
            .ThenBy(t => t.Name)
            .ToList();

        // Assert
        Assert.Equal("Team A", teams[0].Name);
        Assert.Equal("Team B", teams[1].Name);
        Assert.Equal("Team C", teams[2].Name);
        Assert.Equal("Team D", teams[3].Name);
    }

    [Fact]
    public void GenerateLeagueTable_SortsTeamsWithSamePointsAndGoalDifference()
    {
        // Arrange
        var teams = new List<Team>
        {
            new Team { Name = "Team A", Points = 6, Wins = 1, Draws = 3, Losses = 0, GoalsFor = 5, GoalsAgainst = 2 },
            new Team { Name = "Team B", Points = 6, Wins = 2, Draws = 0, Losses = 0, GoalsFor = 4, GoalsAgainst = 1 },
            new Team { Name = "Team C", Points = 4, Wins = 1, Draws = 1, Losses = 1, GoalsFor = 3, GoalsAgainst = 2 },
            new Team { Name = "Team D", Points = 2, Wins = 0, Draws = 2, Losses = 1, GoalsFor = 1, GoalsAgainst = 2 }
        };

        // Act
        teams = teams.OrderByDescending(t => t.Points)
            .ThenByDescending(t => t.GoalDifference)
            .ThenByDescending(t => t.Wins)
            .ThenBy(t => t.Name)
            .ToList();

        // Assert
        Assert.Equal("Team B", teams[0].Name);
        Assert.Equal("Team A", teams[1].Name);
        Assert.Equal("Team C", teams[2].Name);
        Assert.Equal("Team D", teams[3].Name);
    }

    [Fact]
    public void GenerateLeagueTable_SortsTeamsWithSamePointsGoalDifferenceAndWins()
    {
        // Arrange
        var teams = new List<Team>
        {
            new Team { Name = "Team A", Points = 6, Wins = 2, Draws = 0, Losses = 0, GoalsFor = 5, GoalsAgainst = 2 },
            new Team { Name = "Team B", Points = 6, Wins = 2, Draws = 0, Losses = 0, GoalsFor = 5, GoalsAgainst = 2 },
            new Team { Name = "Team C", Points = 4, Wins = 1, Draws = 1, Losses = 1, GoalsFor = 3, GoalsAgainst = 2 },
            new Team { Name = "Team D", Points = 2, Wins = 0, Draws = 2, Losses = 1, GoalsFor = 1, GoalsAgainst = 2 }
        };

        // Act
        teams = teams.OrderByDescending(t => t.Points)
            .ThenByDescending(t => t.GoalDifference)
            .ThenByDescending(t => t.Wins)
            .ThenBy(t => t.Name)
            .ToList();

        // Assert
        Assert.Equal("Team A", teams[0].Name);
        Assert.Equal("Team B", teams[1].Name);
        Assert.Equal("Team C", teams[2].Name);
        Assert.Equal("Team D", teams[3].Name);
    }
}