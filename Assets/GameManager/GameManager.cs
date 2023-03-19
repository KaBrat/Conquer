using System.Runtime.CompilerServices;
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public class Player
    {
        public Player(int id)
        {
            this.id = id;
        }
        public int id;
    }

    public int Round = 0;
    public int maxRounds = 3;
    public Queue<Player> turn = new Queue<Player>();
    public int numberOfPlayers = 2;
    public List<Player> Players;

    public Player currentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        this.Players = createPlayers(numberOfPlayers);
        StartNewRound();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private List<Player> createPlayers(int amount)
    {
        var players = new List<Player>();
        for (int i = 1; i <= amount; i++)
        {
            players.Add(new Player(i));
        }
        return players;
    }

    public Player GetCurrentPlayer()
    {
        return this.currentPlayer;
    }

    public void EndTurn()
    {
        Player dequeuedPlayer;
        var b = turn.TryDequeue(out dequeuedPlayer);
        if (!b)
        {
            if (this.Round >= maxRounds)
            {
                SceneManager.LoadScene(0);
                return;
            }
            StartNewRound();
        }
        Player newPlayer;
        b = turn.TryPeek(out newPlayer);
        if (!b)
        {
            if (this.Round >= maxRounds)
            {
                SceneManager.LoadScene(0);
                return;
            }
            StartNewRound();
        }
        this.currentPlayer = turn.Peek();
    }

    public void StartNewRound()
    {
        this.Round++;
        foreach (var player in Players)
        {
            turn.Enqueue(player);
        }
        this.currentPlayer = turn.Peek();
    }
}
