using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class RoundsManager
{
    public int maxRounds;
    public int currentRound = 0;
    public Queue<Turn> Turns;
    public RoundsManager(int maxRounds)
    {
        this.maxRounds = maxRounds;
    }
    public void NewRound(List<Player> players)
    {
        this.currentRound++;
        this.Turns = new Queue<Turn>();
        foreach (var player in players)
        {
            this.Turns.Enqueue(new Turn(player));
        }
    }
    public void EndTurn()
    {
        this.Turns.Dequeue();
    }
}

public class Turn
{
    public Turn(Player player)
    {
        this.currentPlayer = player;
    }
    public Player currentPlayer;
}

public class Player
{
    public int id;
    public Color32 Color;
    public Player(int id, Color color)
    {
        this.id = id;
        this.Color = color;
    }

    public static List<Player> createPlayers(int amount)
    {
        var players = new List<Player>();
        for (int i = 1; i <= amount; i++)
        {
            var notTakenColor = ColorHelper.PlayerColors.Where(c => !players.Any(p => p.Color.Equals(c))).ToList().FirstOrDefault();
            players.Add(new Player(i, notTakenColor));
        }
        return players;
    }
}


