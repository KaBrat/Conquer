using System.Collections;
using System.Collections.Generic;

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
    public Player(int id)
    {
        this.id = id;
    }
    public int id;

    public static List<Player> createPlayers(int amount)
    {
        var players = new List<Player>();
        for (int i = 1; i <= amount; i++)
        {
            players.Add(new Player(i));
        }
        return players;
    }
}


