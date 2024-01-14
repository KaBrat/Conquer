using System.Collections.Generic;
using UnityEngine;

public class Province
{
    public string Name { get; private set; }
    public Player Owner { get; set; }
    public int FootmenCount { get; set; }
    public Color32 Color { get; internal set; }
    public int[] PixelIndices { get; set; }
    public int[] BorderPixelIndices { get; set; }

    public bool highLighted = false;

    public Province(string name, Color32 color, int[] pixelIndices, int[] borderPixelIndices)
    {
        this.Name = name;
        this.Color = color;
        this.PixelIndices = pixelIndices;
        this.BorderPixelIndices = borderPixelIndices;
    }

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

    public void ChangeOwnerOnMap(Player newOwner, Map map)
    {
        this.Owner = newOwner;
        this.ColorBorderInPlayerColors(map);
    }

    public void ColorBorderInPlayerColors(Map map)
    {
        if (this.Owner != null)
        {
            var terrainPixels = map.GetPixels32();

            foreach (var borderPixelIndice in this.BorderPixelIndices)
            {
                terrainPixels[borderPixelIndice] = this.Owner.Color;
            }

            map.updateSprite(terrainPixels);
        }
    }

    public void HighlightTerrain(Map terrain, Map provinces)
    {
        this.highLighted = true;

        var terrainPixels = terrain.GetPixels32();
        var provincesPixels = provinces.GetPixels32();

        foreach (var pixelIndice in this.PixelIndices)
        {
            var normalTerrainColor = terrainPixels[pixelIndice];
            var highlightedTerrainColor = ColorHelper.GetHighlightedColor(normalTerrainColor);
            terrainPixels[pixelIndice] = highlightedTerrainColor;
        }

        terrain.updateSprite(terrainPixels);
    }

    public void UnHighlightTerrain(Map terrain, Map provinces)
    {
        this.highLighted = false;

        var terrainPixels = terrain.GetPixels32();
        var provincesPixels = provinces.GetPixels32();

        foreach (var pixelIndice in this.PixelIndices)
        {
            var highlightedColor = terrainPixels[pixelIndice];
            var normalColor = ColorHelper.GetOriginalProvinceColor(highlightedColor);
            terrainPixels[pixelIndice] = normalColor;
        }

        terrain.updateSprite(terrainPixels);
    }

    public void Select(Map terrain, Map provinces)
    {
        if (!this.highLighted)
        {
            this.HighlightTerrain(terrain, provinces);
        }

        var terrainPixels = terrain.GetPixels32();
        var provincesPixels = provinces.GetPixels32();

        foreach (var borderPixelIndice in this.BorderPixelIndices)
        {
            if (this.Owner == null)
                terrainPixels[borderPixelIndice] = ColorHelper.selectedBorderColor;
            else
            {
                var highlightedBorderColor = ColorHelper.GetHighlightedColor(this.Owner.Color);
                terrainPixels[borderPixelIndice] = highlightedBorderColor;
            }
            provincesPixels[borderPixelIndice] = ColorHelper.selectedBorderColor;
        }
        terrain.updateSprite(terrainPixels);
        provinces.updateSprite(provincesPixels);
    }

    public void Deselect(Map terrain, Map provinces)
    {
        if (this.highLighted)
        {
            this.UnHighlightTerrain(terrain, provinces);
        }

        var terrainPixels = terrain.GetPixels32();
        var provincesPixels = provinces.GetPixels32();

        foreach (var borderPixelIndice in this.BorderPixelIndices)
        {
            provincesPixels[borderPixelIndice] = ColorHelper.borderColor;
            if (this.Owner == null)
                terrainPixels[borderPixelIndice] = ColorHelper.borderColor;
            else
                terrainPixels[borderPixelIndice] = this.Owner.Color;
        }
        terrain.updateSprite(terrainPixels);
        provinces.updateSprite(provincesPixels);
    }
}