using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour, IEndTurn
{
    public RoundsManager RoundsManager;
    public ProvincesMap ProvincesMap;
    public List<Player> Players;
    public int NumberOfPlayers = 2;
    public int MaxRounds = 3;

    void SetPlayerStartingLocations()
    {
        var northguard = this.ProvincesMap.Provinces.FirstOrDefault(p => p.Name == "Northguard");
        northguard.Owner = Players.FirstOrDefault(p => p.id == 1);
        northguard.FootmenCount = 2;
        var summershore = this.ProvincesMap.Provinces.FirstOrDefault(p => p.Name == "Summershore");
        summershore.Owner = Players.FirstOrDefault(p => p.id == 2);
        summershore.FootmenCount = 2;
    }

    void Start()
    {
        this.Players = Player.createPlayers(NumberOfPlayers);
        //this.SetPlayerStartingLocations();
        this.RoundsManager = new RoundsManager(MaxRounds);
        this.RoundsManager.NewRound(this.Players);
    }

    public (Player prev, Player current) EndTurn()
    {
        var prev = RoundsManager.Turns.Peek().currentPlayer;
        this.RoundsManager.EndTurn();

        var RoundEnded = this.RoundsManager.Turns.Count == 0;
        if (RoundEnded)
        {
            if (RoundsManager.currentRound == RoundsManager.maxRounds)
            {
                SceneManager.LoadScene(0);
                return (null, null);
            }
            this.RoundsManager.NewRound(this.Players);
        }

        var current = RoundsManager.Turns.Peek().currentPlayer;
        return (prev, current);
    }
}
