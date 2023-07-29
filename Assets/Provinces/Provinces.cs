using UnityEngine;

public class Province
{
    public string Name { get; private set; }
    public Player Owner { get; set; }
    public int FootmenCount { get; set; }
    public Color Color { get; internal set; }

    public Province(string name, Player owner, int footmenCount, Color color)
    {
        Name = name;
        Owner = owner;
        FootmenCount = footmenCount;
        Color = color;
    }

    public Province(string name, Color color)
    {
        Name = name;
        Color = color;
    }
}