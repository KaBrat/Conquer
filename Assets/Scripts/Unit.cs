using UnityEngine;
using TMPro;

public class Unit : MonoBehaviour
{
    public int Amount = 1;

    public void SetAmount(int amount)
    {
        this.Amount = amount;
        this.gameObject.GetComponentInChildren<TextMeshPro>().SetText(this.Amount.ToString());
    }

    public void AddAmount(int amount)
    {
        this.Amount += amount;
        this.gameObject.GetComponentInChildren<TextMeshPro>().SetText(this.Amount.ToString());
    }
    public void SetMaterial(Material material)
    {
        this.gameObject.GetComponent<MeshRenderer>().material = material;
    }

    void Start()
    {
        this.SetAmount(1);
    }
}
