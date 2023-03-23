using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IEndTurn
{
    public RoundsManager roundsManager;
    public List<Player> players;
    public int numberOfPlayers = 2;
    public int maxRounds = 3;

    void Start()
    {
        this.players = Player.createPlayers(numberOfPlayers);
        roundsManager = new RoundsManager(maxRounds);
        roundsManager.NewRound(this.players);
    }

    public (Player prev, Player current) EndTurn()
    {
        var prev = roundsManager.Turns.Peek().currentPlayer;
        this.roundsManager.EndTurn();

        var RoundEnded = this.roundsManager.Turns.Count == 0;
        if (RoundEnded)
        {
            if (roundsManager.currentRound == roundsManager.maxRounds)
            {
                SceneManager.LoadScene(0);
                return (null, null);
            }
            this.roundsManager.NewRound(this.players);
        }

        var current = roundsManager.Turns.Peek().currentPlayer;
        return (prev, current);
    }
}
