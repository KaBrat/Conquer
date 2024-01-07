using System.Collections.Generic;
using UnityEngine;

public class Province
{
    public string Name { get; private set; }
    public Player Owner { get; set; }
    public int FootmenCount { get; set; }
    public Color32 Color { get; internal set; }
    public List<ColorWithPosition> BorderPixels { get; set; }
    public List<ColorWithPosition> Pixels { get; set; }

    public bool highLighted = false;

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

    public void ChangeOwner(Player newOwner, Texture2D terrain)
    {
        this.Owner = newOwner;
        this.ColorBorderInPlayerColors(terrain);
    }

    public void ColorBorderInPlayerColors(Texture2D terrain)
    {
        if (this.Owner != null)
        {
            var terrainPixels = terrain.GetPixels32();

            foreach (var borderPixel in this.BorderPixels)
            {
                terrainPixels[borderPixel.Position.y * terrain.width + borderPixel.Position.x] = this.Owner.Color;
            }

            terrain.SetPixels32(terrainPixels);
            terrain.Apply();
        }
    }

    public void HighlightTerrain(Texture2D terrain, Texture2D provinces)
    {
        this.highLighted = true;

        var terrainPixels = terrain.GetPixels32();
        var provincesPixels = provinces.GetPixels32();

        if (this.Pixels == null)
        {
            this.Pixels = ColorHelper.ExtractColorsWithPositions(provincesPixels, provinces.width, provinces.height, (color) => color.Equals(this.Color));
        }
        foreach (var pixel in this.Pixels)
        {
            var normalTerrainColor = terrainPixels[pixel.Position.y * terrain.width + pixel.Position.x];
            var highlightedTerrainColor = new Color32(normalTerrainColor.r, normalTerrainColor.g, normalTerrainColor.b, 180);
            terrainPixels[pixel.Position.y * terrain.width + pixel.Position.x] = highlightedTerrainColor;
        }

        terrain.SetPixels32(terrainPixels);
        terrain.Apply();
    }

    public void UnHighlightTerrain(Texture2D terrain, Texture2D provinces)
    {
        this.highLighted = false;

        var terrainPixels = terrain.GetPixels32();
        var provincesPixels = provinces.GetPixels32();

        if (this.Pixels == null)
        {
            this.Pixels = ColorHelper.ExtractColorsWithPositions(terrainPixels, terrain.width, terrain.height, (color) => color.Equals(this.Color));
        }
        foreach (var pixel in this.Pixels)
        {
            var highlightedColor = terrainPixels[pixel.Position.y * terrain.width + pixel.Position.x];
            var normalColor = new Color32(highlightedColor.r, highlightedColor.g, highlightedColor.b, 255);
            terrainPixels[pixel.Position.y * terrain.width + pixel.Position.x] = normalColor;
        }

        terrain.SetPixels32(terrainPixels);
        terrain.Apply();
    }

    public void Select(Texture2D terrain, Texture2D provinces)
    {
        if (!this.highLighted)
        {
            this.HighlightTerrain(terrain, provinces);
        }

        var terrainPixels = terrain.GetPixels32();
        var provincesPixels = provinces.GetPixels32();

        foreach (var borderPixel in this.BorderPixels)
        {
            if (this.Owner == null)
                terrainPixels[borderPixel.Position.y * terrain.width + borderPixel.Position.x] = ColorHelper.selectedBorderColor;
            else
            {
                var highlightedBorderColor = new Color32(this.Owner.Color.r, this.Owner.Color.g, this.Owner.Color.b, 180);
                terrainPixels[borderPixel.Position.y * provinces.width + borderPixel.Position.x] = highlightedBorderColor;
            }
            provincesPixels[borderPixel.Position.y * provinces.width + borderPixel.Position.x] = ColorHelper.selectedBorderColor;
        }
        terrain.SetPixels32(terrainPixels);
        terrain.Apply();

        provinces.SetPixels32(provincesPixels);
        provinces.Apply();
    }

    public void Deselect(Texture2D terrain, Texture2D provinces)
    {
        if (this.highLighted)
        {
            this.UnHighlightTerrain(terrain, provinces);
        }

        var terrainPixels = terrain.GetPixels32();
        var provincesPixels = provinces.GetPixels32();

        foreach (var borderPixel in this.BorderPixels)
        {
            provincesPixels[borderPixel.Position.y * terrain.width + borderPixel.Position.x] = ColorHelper.borderColor;
            if (this.Owner == null)
                terrainPixels[borderPixel.Position.y * provinces.width + borderPixel.Position.x] = ColorHelper.borderColor;
            else
                terrainPixels[borderPixel.Position.y * provinces.width + borderPixel.Position.x] = this.Owner.Color;
        }
        terrain.SetPixels32(terrainPixels);
        terrain.Apply();

        provinces.SetPixels32(provincesPixels);
        provinces.Apply();
    }
}