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

        var provinceColors = new HashSet<Color32>();

        while (FindStartingPosition(provincesMap, mapSize, out Vector2Int startingPosition))
        {
            var size = UnityEngine.Random.Range(provincesMaxSize / 4, provincesMaxSize);
            var colorsToReplace = ColorHelper.SelectableTerrainColors.ToArray();
            var stateColor = ColorHelper.AddNewRandomColorToList(provinceColors);
            PaintHelper.FloodPaint(provincesMap, mapSize.x, mapSize.y, startingPosition, colorsToReplace, stateColor, size);
        }

        return (provincesMap, provinceColors);
    }

    private bool FindStartingPosition(Color32[] provincesMap, Vector2Int mapSize, out Vector2Int startingPosition)
    {
        // Generate random starting coordinates within the map boundaries
        var randomStartX = UnityEngine.Random.Range(0, mapSize.x - 1);
        var randomStartY = UnityEngine.Random.Range(0, mapSize.y - 1);

        // Iterate over a range of x values, wrapping around if necessary
        for (var x = randomStartX; x < randomStartX + mapSize.x; x++)
        {
            // Iterate over a range of y values, wrapping around if necessary
            for (var y = randomStartY; y < randomStartY + mapSize.y; y++)
            {
                // Use modular arithmetic to get wrapped coordinates within the map
                var clampedX = x % mapSize.x;
                var clampedY = y % mapSize.y;

                // Retrieve the color of the current pixel in the provinces map
                var pixelColor = provincesMap[clampedY * mapSize.x + clampedX];

                // Check if the pixel color is one of the selectable terrain colors
                if (ColorHelper.SelectableTerrainColors.Contains(pixelColor))
                {
                    // If a matching color is found, set the starting position and return true
                    startingPosition = new Vector2Int(clampedX, clampedY);
                    return true;
                }
            }
        }

        // If no matching color is found, set startingPosition to zero and return false
        startingPosition = Vector2Int.zero;
        return false;
    }
}