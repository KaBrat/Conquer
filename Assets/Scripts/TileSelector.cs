using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public bool Active;
    public void ToggleSelector(Player player)
    {
        if (this.Active)
        {
            this.Active = false;
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            this.Active = true;
            this.gameObject.GetComponent<MeshRenderer>().material = player.Material;
            this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void SetActive(Player player)
    {
        this.Active = true;
        this.gameObject.GetComponent<MeshRenderer>().material = player.Material;
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    public void SetInactive()
    {
        this.Active = false;
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
