using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class ProvincesGenerator
{
    public (Color32[] provinces, HashSet<Color32> provinceColors) GenerateProvinces(Color32[] Terrain, Vector2Int mapSize, int provincesMaxSize)
    {
        var provinces = new Color32[Terrain.Length];
        Array.Copy(Terrain, provinces, Terrain.Length);

        var found = true;
        var startingPosition = new Vector2();

        var provinceColors = new HashSet<Color32>();

        while (found)
        {
            found = false;

            for (var x = 0; x < mapSize.x; x++)
            {
                for (var y = 0; y < mapSize.y; y++)
                {
                    var pixelColor = provinces[y * mapSize.x + x];
                    if (pixelColor.Equals(ColorHelper.grassGreen) || pixelColor.Equals(ColorHelper.sandYellow))
                    {
                        found = true;
                        startingPosition.x = x;
                        startingPosition.y = y;
                    }
                }
            }

            if (found)
            {
                var size = UnityEngine.Random.Range(provincesMaxSize / 2, provincesMaxSize);
                var colorsToReplace = ColorHelper.LandColors.ToArray();
                var stateColor = ColorHelper.AddNewRandomColorToList(provinceColors);
                PaintHelper.FloodPaint(provinces, mapSize.x, mapSize.y, startingPosition, colorsToReplace, stateColor, size);
            }
        }

        return (provinces, provinceColors);
    }
}