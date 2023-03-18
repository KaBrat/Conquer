using UnityEngine;

public class Tile : MonoBehaviour
{
    public Player Owner;
    public Game Game;
    public bool selected;

    void Start()
    {
        this.Game = GameObject.FindObjectOfType<Game>();
    }

    public void SetOwner(Player player)
    {
        this.Owner = player;
        this.gameObject.GetComponentInChildren<Unit>().SetMaterial(player.Material);
    }

    private void OnMouseDown()
    {
        switch (this.Game.CurrentTurn.Phase)
        {
            case Game.Phase.Place:
                if (IsOwnedByCurrentPlayer())
                {
                    this.Game.CurrentTurn.Player.FreeUnits--;
                    this.gameObject.GetComponentInChildren<Unit>().AddAmount(1);
                }
                break;
            case Game.Phase.Conquer:
                if (this.IsOwnedByCurrentPlayer())
                {
                    // select own units
                    if (!this.Game.OwnTileSelected && !this.Game.EnemyTileSelected && !this.selected)
                    {
                        this.selected = true;
                        this.Game.OwnTileSelected = true;
                        this.gameObject.GetComponentInChildren<TileSelector>().SetActive(this.Game.CurrentTurn.Player);
                        return;
                    }

                    // deselect own units
                    if (this.Game.OwnTileSelected && this.selected && !this.Game.EnemyTileSelected)
                    {
                        this.selected = false;
                        this.Game.OwnTileSelected = false;
                        this.gameObject.GetComponentInChildren<TileSelector>().SetInactive();
                        return;
                    }
                }
                if (!this.IsOwnedByCurrentPlayer())
                {
                    // attack enemy units
                    if (this.Game.OwnTileSelected && !this.Game.EnemyTileSelected && !this.selected)
                    {
                        this.selected = true;
                        this.Game.EnemyTileSelected = true;
                        this.gameObject.GetComponentInChildren<TileSelector>().SetActive(this.Game.CurrentTurn.Player);

                        //attack
                        return;
                    }

                    // deselect enemy units
                    if (this.Game.OwnTileSelected && this.Game.EnemyTileSelected && this.selected)
                    {
                        this.selected = false;
                        this.Game.EnemyTileSelected = false;
                        this.gameObject.GetComponentInChildren<TileSelector>().SetInactive();
                        return;
                    }
                }
                break;
        }
    }

    private void OnMouseEnter()
    {
        var currentPosition = this.gameObject.transform.position;
        this.gameObject.transform.position = new Vector3(currentPosition.x - 0.1f, currentPosition.y + 0.1f, currentPosition.z);
    }

    private void OnMouseExit()
    {
        var currentPosition = this.gameObject.transform.position;
        this.gameObject.transform.position = new Vector3(currentPosition.x + 0.1f, currentPosition.y - 0.1f, currentPosition.z);
    }

    private bool IsOwnedByCurrentPlayer()
    {
        return this.Game.CurrentTurn.Player.Number == Owner.Number;
    }
}
