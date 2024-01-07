using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour, IEndTurn
{
    public MapManager MapManager;
    public RoundsManager RoundsManager;
    public List<Player> Players;
    public int NumberOfPlayers = 2;
    public int MaxRounds = 3;

    void SetPlayerStartingLocations()
    {
        foreach (var player in this.Players) 
        {
            var startingProvince = this.MapManager.ProvincesMap.Provinces.FirstOrDefault(p => p.Owner == null);
            startingProvince.ChangeOwnerOnMap(player, this.MapManager.TerrainMap);
            startingProvince.FootmenCount = 2;
        }
    }

    void Start()
    {
        this.Players = Player.createPlayers(NumberOfPlayers);
        this.SetPlayerStartingLocations();
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
