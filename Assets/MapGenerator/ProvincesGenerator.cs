using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProvincesGenerator
{
    public void Hello()
    {

    }

    public (Color32[] provinces, List<Color32> provinceColors) GenerateProvinces(Color32[] Terrain, Vector2Int mapSize, int provincesMaxSize)
    {
        var provinces = new Color32[Terrain.Length];
        Array.Copy(Terrain, provinces, Terrain.Length);

        var found = true;
        var startingPosition = new Vector2();

        var provinceColors = new List<Color32>();

        while (found)
        {
            found = false;

            for (var x = 0; x < mapSize.x; x++)
            {
                for (var y = 0; y < mapSize.y; y++)
                {
                    var pixelColor = provinces[y * mapSize.x + x];
                    if (pixelColor == Color.green || pixelColor == Color.yellow)
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
                var colorsToReplace = new Color32[] { Color.green, Color.yellow };
                var stateColor = ColorHelper.AddNewRandomColorToList(provinceColors);
                PaintHelper.FloodPaint(provinces, mapSize.x, mapSize.y, startingPosition, colorsToReplace, stateColor, size);
            }
        }

        return (provinces, provinceColors);
    }
}