using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Game : MonoBehaviour
{
    public Player[] Players;
    public GameObject TilePrefab;
    public GamePhase CurrentTurn;
    public Stack<Player> turnOrder = new Stack<Player>();

    public Tile SelectedTile;
    public bool OwnTileSelected;
    public Tile TargetTile;
    public bool EnemyTileSelected;

    // Start is called before the first frame update
    void Start()
    {
        var tile = Instantiate(TilePrefab, new Vector3(-10, 3, 0), Quaternion.Euler(0, 90, -90));
        tile.GetComponent<Tile>().SetOwner(Players[0]);

        var tile2 = Instantiate(TilePrefab, new Vector3(-13, -9, 0), Quaternion.Euler(0, 90, -90));
        tile2.GetComponent<Tile>().SetOwner(Players[0]);

        var tile3 = Instantiate(TilePrefab, new Vector3(7, 6, 0), Quaternion.Euler(0, 90, -90));
        tile3.GetComponent<Tile>().SetOwner(Players[1]);

        var tile4 = Instantiate(TilePrefab, new Vector3(9, -8, 0), Quaternion.Euler(0, 90, -90));
        tile4.GetComponent<Tile>().SetOwner(Players[1]);

        AddPlayersToTurn();

        NextPlayerPlacePhase();
    }

    void Update()
    {
        switch (this.CurrentTurn.Phase)
        {
            case Phase.Place:
                if (AllPlayersActed())
                {
                    this.CurrentTurn.Phase = Phase.Conquer;
                    AddPlayersToTurn();
                    NextPlayerConquerPhase();
                    break;
                }
                if (CurrentTurn.PlayerIsDone())
                {
                    NextPlayerPlacePhase();
                    break;
                }
                break;

            case Phase.Conquer:
                if (CurrentTurn.PlayerIsDone())
                {
                    NextPlayerConquerPhase();
                    break;
                }
                break;
        }
    }

    void AddPlayersToTurn()
    {
        for (int i = Players.Length - 1; i >= 0; i--)
        {
            this.turnOrder.Push(Players[i]);
        }
    }

    bool AllPlayersActed()
    {
        return this.turnOrder.Count == 0 && CurrentTurn.PlayerIsDone();
    }

    void NextPlayerPlacePhase()
    {
        var nextPlayer = turnOrder.Pop();
        this.CurrentTurn = new PlacePhase(nextPlayer);
        this.CurrentTurn.Start();
    }

    void NextPlayerConquerPhase()
    {
        var nextPlayer = turnOrder.Pop();
        this.CurrentTurn = new ConquerPhase(nextPlayer);
        this.CurrentTurn.Start();
    }

    public abstract class GamePhase
    {
        public Phase Phase;
        public Player Player;
        public abstract bool PlayerIsDone();
        public abstract void Start();
        public GamePhase(Player player)
        {
            this.Player = player;
        }
    }

    public class PlacePhase : GamePhase
    {
        public PlacePhase(Player player) : base(player)
        {
            this.Phase = Phase.Place;
        }
        public override void Start()
        {
            this.Player.FreeUnits = 3;
        }

        public override bool PlayerIsDone()
        {
            return this.Player.FreeUnits == 0;
        }
    }

    public class ConquerPhase : GamePhase
    {
        public ConquerPhase(Player player) : base(player)
        {
            this.Phase = Phase.Conquer;
        }
        public override void Start()
        { }

        public override bool PlayerIsDone()
        {
            return false;
        }
    }

    public enum Phase
    {
        Place,
        Conquer,
    }
}
