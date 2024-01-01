using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Province
{
    public string Name { get; private set; }
    public Player Owner { get; set; }
    public int FootmenCount { get; set; }
    public Color32 Color { get; internal set; }
    public List<ColorWithPosition> BorderPixels { get; set; }

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

    public void Deselect(Texture2D terrain, Texture2D provinces)
    {
        var terrainPixels = terrain.GetPixels32();
        var provincesPixels = provinces.GetPixels32();

        foreach (var borderPixel in this.BorderPixels)
        {
            terrainPixels[(int)borderPixel.Position.y * terrain.width + (int)borderPixel.Position.x] = ColorHelper.borderColor;
            provincesPixels[(int)borderPixel.Position.y * provinces.width + (int)borderPixel.Position.x] = borderPixel.Color;
        }
        terrain.SetPixels32(terrainPixels);
        terrain.Apply();

        provinces.SetPixels32(provincesPixels);
        provinces.Apply();
    }

    public void Select(Texture2D terrain, Texture2D provinces)
    {
        var terrainPixels = terrain.GetPixels32();
        var provincesPixels = provinces.GetPixels32();

        foreach (var borderPixel in this.BorderPixels)
        {
            terrainPixels[(int)borderPixel.Position.y * terrain.width + (int)borderPixel.Position.x] = ColorHelper.selectedBorderColor;
            provincesPixels[(int)borderPixel.Position.y * provinces.width + (int)borderPixel.Position.x] = ColorHelper.selectedBorderColor;
        }
        terrain.SetPixels32(terrainPixels);
        terrain.Apply();

        provinces.SetPixels32(provincesPixels);
        provinces.Apply();
    }
}