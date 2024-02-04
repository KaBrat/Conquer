using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class ProvincesGenerator
{
    public (Color32[] provinces, HashSet<Color32> provinceColors) GenerateProvinces(Color32[] terrainMap, Vector2Int mapSize, int provincesMaxSize)
    {
        var provincesMap = new Color32[terrainMap.Length];
        Array.Copy(terrainMap, provincesMap, terrainMap.Length);

        var found = true;
        var startingPosition = new Vector2Int();

        var provinceColors = new HashSet<Color32>();

        while (found)
        {
            found = false;

            var randomStartX = UnityEngine.Random.Range(0, mapSize.x - 1);
            var randomStartY = UnityEngine.Random.Range(0, mapSize.y - 1);

            for (var x = randomStartX; x < mapSize.x; x++)
            {
                for (var y = randomStartY; y < mapSize.y; y++)
                {
                    var pixelColor = provincesMap[y * mapSize.x + x];
                    if (ColorHelper.SelectableTerrainColors.Contains(pixelColor))
                    {
                        found = true;
                        startingPosition.x = x;
                        startingPosition.y = y;
                        break;
                    }
                }
                if (found)
                    break;
                for (var y = 0; y < randomStartY; y++)
                {
                    var pixelColor = provincesMap[y * mapSize.x + x];
                    if (ColorHelper.SelectableTerrainColors.Contains(pixelColor))
                    {
                        found = true;
                        startingPosition.x = x;
                        startingPosition.y = y;
                        break;
                    }
                }
                if (found)
                    break;
            }

            for (var x = 0; x < randomStartX; x++)
            {
                for (var y = randomStartY; y < mapSize.y; y++)
                {
                    var pixelColor = provincesMap[y * mapSize.x + x];
                    if (ColorHelper.SelectableTerrainColors.Contains(pixelColor))
                    {
                        found = true;
                        startingPosition.x = x;
                        startingPosition.y = y;
                        break;
                    }
                }
                if (found)
                    break;
                for (var y = 0; y < randomStartY; y++)
                {
                    var pixelColor = provincesMap[y * mapSize.x + x];
                    if (ColorHelper.SelectableTerrainColors.Contains(pixelColor))
                    {
                        found = true;
                        startingPosition.x = x;
                        startingPosition.y = y;
                        break;
                    }
                }
                if (found)
                    break;
            }

            if (found)
            {
                var size = UnityEngine.Random.Range(provincesMaxSize / 4, provincesMaxSize);
                var colorsToReplace = ColorHelper.SelectableTerrainColors.ToArray();
                var stateColor = ColorHelper.AddNewRandomColorToList(provinceColors);
                PaintHelper.FloodPaint(provincesMap, mapSize.x, mapSize.y, startingPosition, colorsToReplace, stateColor, size);
            }
        }

        return (provincesMap, provinceColors);
    }
}