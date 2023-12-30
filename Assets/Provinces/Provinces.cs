using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Province
{
    public string Name { get; private set; }
    public Player Owner { get; set; }
    public int FootmenCount { get; set; }
    public Color32 Color { get; internal set; }
    public List<MapPixel> BorderPixels { get; set; }

    public Province(string name, Player owner, int footmenCount, Color32 color)
    {
        Name = name;
        Owner = owner;
        FootmenCount = footmenCount;
        Color = color;
    }

    public Province(string name, Color32 color)
    {
        Name = name;
        Color = color;
    }

    public Color32[] Deselect(Color32[] map, Vector2 mapDimension)
    {
        foreach (var borderPixel in this.BorderPixels)
        {
            map[(int)borderPixel.Position.y * (int)mapDimension.x + (int)borderPixel.Position.x] = this.Color;
        }
        return map;
    }

    public Color32[] Select(Color32[] map, Vector2 mapDimension)
    {
        foreach (var borderPixel in this.BorderPixels)
        {
            map[(int)borderPixel.Position.y * (int)mapDimension.x + (int)borderPixel.Position.x] = borderPixel.Color;
        }
        return map;
    }
}