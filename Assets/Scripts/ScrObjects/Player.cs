using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayer", menuName = "Player")]
public class Player : ScriptableObject
{
    public Material Material;
    public int Number;

    public int FreeUnits = 0;
}
